using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq.Expressions;
using System.Reflection;

namespace Pozitron.QuerySpecification.EntityFrameworkCore;

public static class SearchExtension
{
    private static readonly MethodInfo _likeMethodInfo = typeof(DbFunctionsExtensions)
        .GetMethod(nameof(DbFunctionsExtensions.Like), new Type[] { typeof(DbFunctions), typeof(string), typeof(string) })
        ?? throw new TargetException("The EF.Functions.Like not found");

    private static readonly MemberExpression _functions = Expression.Property(null, typeof(EF).GetProperty(nameof(EF.Functions))
        ?? throw new TargetException("The EF.Functions not found!"));

    public static IQueryable<T> Search<T>(this IQueryable<T> source, IEnumerable<SearchExpression<T>> criterias)
    {
        Expression? expr = null;
        var parameter = Expression.Parameter(typeof(T), "x");

        foreach (var criteria in criterias)
        {
            if (string.IsNullOrEmpty(criteria.SearchTerm))
                continue;

            var propertySelector = ParameterReplacerVisitor.Replace(criteria.Selector, criteria.Selector.Parameters[0], parameter) as LambdaExpression;
            _ = propertySelector ?? throw new InvalidExpressionException();

            var searchTermAsExpression = ((Expression<Func<string>>)(() => criteria.SearchTerm)).Body;

            var likeExpression = Expression.Call(
                                    null,
                                    _likeMethodInfo,
                                    _functions,
                                    propertySelector.Body,
                                    searchTermAsExpression);

            expr = expr == null ? (Expression)likeExpression : Expression.OrElse(expr, likeExpression);
        }

        return expr == null
            ? source
            : source.Where(Expression.Lambda<Func<T, bool>>(expr, parameter));
    }
}
