using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Pozitron.QuerySpecification;

public static class IncludeExtensions
{
    public static IQueryable<T> Include<T>(this IQueryable<T> source, IncludeExpressionInfo info)
    {
        _ = info ?? throw new ArgumentNullException(nameof(info));

        var queryExpr = Expression.Call(
            typeof(EntityFrameworkQueryableExtensions),
            "Include",
            new Type[] {
                info.EntityType,
                info.PropertyType
            },
            source.Expression,
            info.LambdaExpression
            );

        return source.Provider.CreateQuery<T>(queryExpr);
    }
    public static IQueryable<T> ThenInclude<T>(this IQueryable<T> source, IncludeExpressionInfo info)
    {
        _ = info ?? throw new ArgumentNullException(nameof(info));
        _ = info.PreviousPropertyType ?? throw new ArgumentNullException(nameof(info.PreviousPropertyType));

        var queryExpr = Expression.Call(
            typeof(EntityFrameworkQueryableExtensions),
            "ThenInclude",
            new Type[] {
                info.EntityType,
                info.PreviousPropertyType,
                info.PropertyType
            },
            source.Expression,
            info.LambdaExpression
            );

        return source.Provider.CreateQuery<T>(queryExpr);
    }
}
