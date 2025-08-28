using System.Data;
using System.Diagnostics;
using System.Reflection;

namespace Pozitron.QuerySpecification;

internal static class LikeExtension
{
    // We'll name the property Format just so we match the produced SQL query parameter name (in case of interpolated strings).
    private record StringVar(string Format);
    private static readonly PropertyInfo _stringFormatProperty = typeof(StringVar).GetProperty(nameof(StringVar.Format))!;
    private static readonly MemberExpression _functions = Expression.Property(null, typeof(EF).GetProperty(nameof(EF.Functions))!);
    private static readonly MethodInfo _likeMethodInfo = typeof(DbFunctionsExtensions)
        .GetMethod(nameof(DbFunctionsExtensions.Like), [typeof(DbFunctions), typeof(string), typeof(string)])!;


    // It's required so EF can generate parameterized query.
    // In the past I've been creating closures for this, e.g. var patternAsExpression = ((Expression<Func<string>>)(() => pattern)).Body;
    // But, that allocates 168 bytes. So, this is more efficient way.
    private static MemberExpression StringAsExpression(string value)
        => Expression.Property(Expression.Constant(new StringVar(value)), _stringFormatProperty);

    public static IQueryable<T> ApplyLikesAsOrGroup<T>(this IQueryable<T> source, ReadOnlySpan<SpecItem> likeItems)
    {
        Debug.Assert(_likeMethodInfo is not null);

        Expression? combinedExpr = null;
        ParameterExpression? mainParam = null;
        ParameterReplacerVisitor? visitor = null;

        foreach (var item in likeItems)
        {
            if (item.Reference is not SpecLike<T> specLike) continue;

            mainParam ??= specLike.KeySelector.Parameters[0];

            var selectorExpr = specLike.KeySelector.Body;
            if (mainParam != specLike.KeySelector.Parameters[0])
            {
                visitor ??= new ParameterReplacerVisitor(specLike.KeySelector.Parameters[0], mainParam);

                // If there are more than 2 likes, we want to avoid creating a new visitor instance (saving 32 bytes per instance).
                // We're in a sequential loop, no concurrency issues.
                visitor.Update(specLike.KeySelector.Parameters[0], mainParam);
                selectorExpr = visitor.Visit(selectorExpr);
            }

            var patternExpr = StringAsExpression(specLike.Pattern);

            var likeExpr = Expression.Call(
                null,
                _likeMethodInfo,
                _functions,
                selectorExpr,
                patternExpr);

            combinedExpr = combinedExpr is null
                ? likeExpr
                : Expression.OrElse(combinedExpr, likeExpr);
        }

        return combinedExpr is null || mainParam is null
            ? source
            : source.Where(Expression.Lambda<Func<T, bool>>(combinedExpr, mainParam));
    }
}

internal sealed class ParameterReplacerVisitor : ExpressionVisitor
{
    private ParameterExpression _oldParameter;
    private Expression _newExpression;

    internal ParameterReplacerVisitor(ParameterExpression oldParameter, Expression newExpression) =>
        (_oldParameter, _newExpression) = (oldParameter, newExpression);

    internal void Update(ParameterExpression oldParameter, Expression newExpression) =>
        (_oldParameter, _newExpression) = (oldParameter, newExpression);

    protected override Expression VisitParameter(ParameterExpression node) =>
        node == _oldParameter ? _newExpression : node;
}
