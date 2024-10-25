namespace Pozitron.QuerySpecification;

public static class OrderedBuilderExtensions
{
    public static IOrderedSpecificationBuilder<T, TResult> ThenBy<T, TResult>(
        this IOrderedSpecificationBuilder<T, TResult> orderedBuilder,
        Expression<Func<T, object?>> orderExpression)
        => ThenBy(orderedBuilder, orderExpression, true);

    public static IOrderedSpecificationBuilder<T, TResult> ThenBy<T, TResult>(
        this IOrderedSpecificationBuilder<T, TResult> orderedBuilder,
        Expression<Func<T, object?>> orderExpression,
        bool condition)
    {
        if (condition && !orderedBuilder.IsChainDiscarded)
        {
            orderedBuilder.Specification.AddInternal(StateType.Order, orderExpression, (int)OrderTypeEnum.ThenBy);
        }
        else
        {
            orderedBuilder.IsChainDiscarded = true;
        }

        return orderedBuilder;
    }

    public static IOrderedSpecificationBuilder<T> ThenBy<T>(
        this IOrderedSpecificationBuilder<T> orderedBuilder,
        Expression<Func<T, object?>> orderExpression)
        => ThenBy(orderedBuilder, orderExpression, true);

    public static IOrderedSpecificationBuilder<T> ThenBy<T>(
        this IOrderedSpecificationBuilder<T> orderedBuilder,
        Expression<Func<T, object?>> orderExpression,
        bool condition)
    {
        if (condition && !orderedBuilder.IsChainDiscarded)
        {
            orderedBuilder.Specification.AddInternal(StateType.Order, orderExpression, (int)OrderTypeEnum.ThenBy);
        }
        else
        {
            orderedBuilder.IsChainDiscarded = true;
        }

        return orderedBuilder;
    }

    public static IOrderedSpecificationBuilder<T, TResult> ThenByDescending<T, TResult>(
        this IOrderedSpecificationBuilder<T, TResult> orderedBuilder,
        Expression<Func<T, object?>> orderExpression)
        => ThenByDescending(orderedBuilder, orderExpression, true);

    public static IOrderedSpecificationBuilder<T, TResult> ThenByDescending<T, TResult>(
        this IOrderedSpecificationBuilder<T, TResult> orderedBuilder,
        Expression<Func<T, object?>> orderExpression,
        bool condition)
    {
        if (condition && !orderedBuilder.IsChainDiscarded)
        {
            orderedBuilder.Specification.AddInternal(StateType.Order, orderExpression, (int)OrderTypeEnum.ThenByDescending);
        }
        else
        {
            orderedBuilder.IsChainDiscarded = true;
        }

        return orderedBuilder;
    }

    public static IOrderedSpecificationBuilder<T> ThenByDescending<T>(
        this IOrderedSpecificationBuilder<T> orderedBuilder,
        Expression<Func<T, object?>> orderExpression)
        => ThenByDescending(orderedBuilder, orderExpression, true);

    public static IOrderedSpecificationBuilder<T> ThenByDescending<T>(
        this IOrderedSpecificationBuilder<T> orderedBuilder,
        Expression<Func<T, object?>> orderExpression,
        bool condition)
    {
        if (condition && !orderedBuilder.IsChainDiscarded)
        {
            orderedBuilder.Specification.AddInternal(StateType.Order, orderExpression, (int)OrderTypeEnum.ThenByDescending);
        }
        else
        {
            orderedBuilder.IsChainDiscarded = true;
        }

        return orderedBuilder;
    }
}
