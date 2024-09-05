using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq.Expressions;
using System.Reflection;

namespace Pozitron.QuerySpecification.EntityFrameworkCore;

public static class LikeExtension
{
    private static readonly MethodInfo _likeMethodInfo = typeof(DbFunctionsExtensions)
        .GetMethod(nameof(DbFunctionsExtensions.Like), new Type[] { typeof(DbFunctions), typeof(string), typeof(string) })
        ?? throw new TargetException("The EF.Functions.Like not found");

    private static readonly MemberExpression _functions = Expression.Property(null, typeof(EF).GetProperty(nameof(EF.Functions))
        ?? throw new TargetException("The EF.Functions not found!"));

    public static IQueryable<T> Like<T>(this IQueryable<T> source, IEnumerable<LikeExpression<T>> likeExpressions)
    {
        Expression? expr = null;
        var parameter = Expression.Parameter(typeof(T), "x");

        foreach (var likeExpression in likeExpressions)
        {
            if (string.IsNullOrEmpty(likeExpression.Pattern))
                continue;

            var propertySelector = ParameterReplacerVisitor.Replace(likeExpression.KeySelector, likeExpression.KeySelector.Parameters[0], parameter) as LambdaExpression;
            _ = propertySelector ?? throw new InvalidExpressionException();

            var patternAsExpression = ((Expression<Func<string>>)(() => likeExpression.Pattern)).Body;

            var EFLikeExpression = Expression.Call(
                                    null,
                                    _likeMethodInfo,
                                    _functions,
                                    propertySelector.Body,
                                    patternAsExpression);

            expr = expr == null ? (Expression)EFLikeExpression : Expression.OrElse(expr, EFLikeExpression);
        }

        return expr == null
            ? source
            : source.Where(Expression.Lambda<Func<T, bool>>(expr, parameter));
    }
}
