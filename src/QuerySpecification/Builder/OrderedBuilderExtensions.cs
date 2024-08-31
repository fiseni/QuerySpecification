using System.Linq.Expressions;

namespace Pozitron.QuerySpecification;

public static class OrderedBuilderExtensions
{
    public static IOrderedSpecificationBuilder<T> ThenBy<T>(
        this IOrderedSpecificationBuilder<T> orderedBuilder,
        Expression<Func<T, object?>> orderExpression)
    {
        ((List<(Expression<Func<T, object?>> OrderExpression, OrderTypeEnum OrderType)>)orderedBuilder.Specification.OrderExpressions)
            .Add((orderExpression, OrderTypeEnum.ThenBy));

        return orderedBuilder;
    }

    public static IOrderedSpecificationBuilder<T> ThenByDescending<T>(
        this IOrderedSpecificationBuilder<T> orderedBuilder,
        Expression<Func<T, object?>> orderExpression)
    {
        ((List<(Expression<Func<T, object?>> OrderExpression, OrderTypeEnum OrderType)>)orderedBuilder.Specification.OrderExpressions)
            .Add((orderExpression, OrderTypeEnum.ThenByDescending));

        return orderedBuilder;
    }
}
