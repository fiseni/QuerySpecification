using System.Data;
using System.Diagnostics;
using System.Reflection;

namespace Pozitron.QuerySpecification;

internal static class LikeExtension
{
    private static readonly MethodInfo _likeMethodInfo = typeof(DbFunctionsExtensions)
        .GetMethod(nameof(DbFunctionsExtensions.Like), [typeof(DbFunctions), typeof(string), typeof(string)])!;

    private static readonly PropertyInfo _functionsProp = typeof(EF).GetProperty(nameof(EF.Functions))!;
    private static readonly MemberExpression _functions = Expression.Property(null, _functionsProp);

    public static IQueryable<T> Like<T>(this IQueryable<T> source, IEnumerable<LikeExpression<T>> likeExpressions)
    {
        Debug.Assert(_likeMethodInfo is not null);
        Debug.Assert(_functionsProp is not null);

        Expression? expr = null;
        var parameter = Expression.Parameter(typeof(T), "x");

        foreach (var likeExpression in likeExpressions)
        {
            var propertySelector = ParameterReplacerVisitor.Replace(
                likeExpression.KeySelector,
                likeExpression.KeySelector.Parameters[0],
                parameter) as LambdaExpression;

            Debug.Assert(propertySelector is not null);

            var patternAsExpression = ((Expression<Func<string>>)(() => likeExpression.Pattern)).Body;

            var efLikeExpression = Expression.Call(
                null,
                _likeMethodInfo,
                _functions,
                propertySelector.Body,
                patternAsExpression);

            expr = expr is null 
                ? efLikeExpression 
                : Expression.OrElse(expr, efLikeExpression);
        }

        return expr is null
            ? source
            : source.Where(Expression.Lambda<Func<T, bool>>(expr, parameter));
    }
}
