﻿namespace Pozitron.QuerySpecification;

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
        if (condition && !Specification<T, TResult>._isChainDiscarded)
        {
            var state = new SpecState
            {
                Type = StateType.Order,
                Bag = (int)OrderTypeEnum.ThenBy,
                Reference = orderExpression
            };
            orderedBuilder.Specification.Add(state);
        }
        else
        {
            Specification<T, TResult>._isChainDiscarded = true;
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
        if (condition && !Specification<T>._isChainDiscarded)
        {
            var state = new SpecState
            {
                Type = StateType.Order,
                Bag = (int)OrderTypeEnum.ThenBy,
                Reference = orderExpression
            };
            orderedBuilder.Specification.Add(state);
        }
        else
        {
            Specification<T>._isChainDiscarded = true;
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
        if (condition && !Specification<T, TResult>._isChainDiscarded)
        {
            var state = new SpecState
            {
                Type = StateType.Order,
                Bag = (int)OrderTypeEnum.ThenByDescending,
                Reference = orderExpression
            };
            orderedBuilder.Specification.Add(state);
        }
        else
        {
            Specification<T, TResult>._isChainDiscarded = true;
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
        if (condition && !Specification<T>._isChainDiscarded)
        {
            var state = new SpecState
            {
                Type = StateType.Order,
                Bag = (int)OrderTypeEnum.ThenByDescending,
                Reference = orderExpression
            };
            orderedBuilder.Specification.Add(state);
        }
        else
        {
            Specification<T>._isChainDiscarded = true;
        }

        return orderedBuilder;
    }
}
