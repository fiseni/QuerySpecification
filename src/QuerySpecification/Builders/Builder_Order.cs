namespace Pozitron.QuerySpecification;

public static partial class SpecificationBuilderExtensions
{
    public static IOrderedSpecificationBuilder<T, TResult> OrderBy<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder,
        Expression<Func<T, object?>> keySelector)
        => OrderBy(builder, keySelector, true);

    public static IOrderedSpecificationBuilder<T, TResult> OrderBy<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder,
        Expression<Func<T, object?>> keySelector,
        bool condition)
    {
        if (condition)
        {
            builder.Specification.AddInternal(ItemType.Order, keySelector, (int)OrderType.OrderBy);
        }

        Specification<T, TResult>.IsChainDiscarded = !condition;
        return (SpecificationBuilder<T, TResult>)builder;
    }

    public static IOrderedSpecificationBuilder<T> OrderBy<T>(
        this ISpecificationBuilder<T> builder,
        Expression<Func<T, object?>> keySelector)
        => OrderBy(builder, keySelector, true);

    public static IOrderedSpecificationBuilder<T> OrderBy<T>(
        this ISpecificationBuilder<T> builder,
        Expression<Func<T, object?>> keySelector,
        bool condition)
    {
        if (condition)
        {
            builder.Specification.AddInternal(ItemType.Order, keySelector, (int)OrderType.OrderBy);
        }

        Specification<T>.IsChainDiscarded = !condition;
        return (SpecificationBuilder<T>)builder;
    }

    public static IOrderedSpecificationBuilder<T, TResult> OrderByDescending<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder,
        Expression<Func<T, object?>> keySelector)
        => OrderByDescending(builder, keySelector, true);

    public static IOrderedSpecificationBuilder<T, TResult> OrderByDescending<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder,
        Expression<Func<T, object?>> keySelector,
        bool condition)
    {
        if (condition)
        {
            builder.Specification.AddInternal(ItemType.Order, keySelector, (int)OrderType.OrderByDescending);
        }

        Specification<T, TResult>.IsChainDiscarded = !condition;
        return (SpecificationBuilder<T, TResult>)builder;
    }

    public static IOrderedSpecificationBuilder<T> OrderByDescending<T>(
        this ISpecificationBuilder<T> builder,
        Expression<Func<T, object?>> keySelector)
        => OrderByDescending(builder, keySelector, true);

    public static IOrderedSpecificationBuilder<T> OrderByDescending<T>(
        this ISpecificationBuilder<T> builder,
        Expression<Func<T, object?>> keySelector,
        bool condition)
    {
        if (condition)
        {
            builder.Specification.AddInternal(ItemType.Order, keySelector, (int)OrderType.OrderByDescending);
        }

        Specification<T>.IsChainDiscarded = !condition;
        return (SpecificationBuilder<T>)builder;
    }

    public static IOrderedSpecificationBuilder<T, TResult> ThenBy<T, TResult>(
        this IOrderedSpecificationBuilder<T, TResult> orderedBuilder,
        Expression<Func<T, object?>> orderExpression)
        => ThenBy(orderedBuilder, orderExpression, true);

    public static IOrderedSpecificationBuilder<T, TResult> ThenBy<T, TResult>(
        this IOrderedSpecificationBuilder<T, TResult> orderedBuilder,
        Expression<Func<T, object?>> orderExpression,
        bool condition)
    {
        if (condition && !Specification<T, TResult>.IsChainDiscarded)
        {
            orderedBuilder.Specification.AddInternal(ItemType.Order, orderExpression, (int)OrderType.ThenBy);
        }
        else
        {
            Specification<T, TResult>.IsChainDiscarded = true;
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
        if (condition && !Specification<T>.IsChainDiscarded)
        {
            orderedBuilder.Specification.AddInternal(ItemType.Order, orderExpression, (int)OrderType.ThenBy);
        }
        else
        {
            Specification<T>.IsChainDiscarded = true;
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
        if (condition && !Specification<T, TResult>.IsChainDiscarded)
        {
            orderedBuilder.Specification.AddInternal(ItemType.Order, orderExpression, (int)OrderType.ThenByDescending);
        }
        else
        {
            Specification<T, TResult>.IsChainDiscarded = true;
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
        if (condition && !Specification<T>.IsChainDiscarded)
        {
            orderedBuilder.Specification.AddInternal(ItemType.Order, orderExpression, (int)OrderType.ThenByDescending);
        }
        else
        {
            Specification<T>.IsChainDiscarded = true;
        }

        return orderedBuilder;
    }
}
