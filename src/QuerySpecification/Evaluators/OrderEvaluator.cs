﻿namespace Pozitron.QuerySpecification;

public class OrderEvaluator : IEvaluator, IInMemoryEvaluator
{
    private OrderEvaluator() { }
    public static OrderEvaluator Instance = new();

    public IQueryable<T> Evaluate<T>(IQueryable<T> source, Specification<T> specification) where T : class
    {
        IOrderedQueryable<T>? orderedQuery = null;

        foreach (var orderExpression in specification.OrderExpressions)
        {
            if (orderExpression.OrderType == OrderTypeEnum.OrderBy)
            {
                orderedQuery = source.OrderBy(orderExpression.KeySelector);
            }
            else if (orderExpression.OrderType == OrderTypeEnum.OrderByDescending)
            {
                orderedQuery = source.OrderByDescending(orderExpression.KeySelector);
            }
            else if (orderExpression.OrderType == OrderTypeEnum.ThenBy)
            {
                orderedQuery = orderedQuery!.ThenBy(orderExpression.KeySelector);
            }
            else if (orderExpression.OrderType == OrderTypeEnum.ThenByDescending)
            {
                orderedQuery = orderedQuery!.ThenByDescending(orderExpression.KeySelector);
            }
        }

        if (orderedQuery is not null)
        {
            source = orderedQuery;
        }

        return source;
    }

    public IEnumerable<T> Evaluate<T>(IEnumerable<T> source, Specification<T> specification)
    {
        IOrderedEnumerable<T>? orderedQuery = null;

        foreach (var orderExpression in specification.OrderExpressions)
        {
            if (orderExpression.OrderType == OrderTypeEnum.OrderBy)
            {
                orderedQuery = source.OrderBy(orderExpression.KeySelectorFunc);
            }
            else if (orderExpression.OrderType == OrderTypeEnum.OrderByDescending)
            {
                orderedQuery = source.OrderByDescending(orderExpression.KeySelectorFunc);
            }
            else if (orderExpression.OrderType == OrderTypeEnum.ThenBy)
            {
                orderedQuery = orderedQuery!.ThenBy(orderExpression.KeySelectorFunc);
            }
            else if (orderExpression.OrderType == OrderTypeEnum.ThenByDescending)
            {
                orderedQuery = orderedQuery!.ThenByDescending(orderExpression.KeySelectorFunc);
            }
        }

        if (orderedQuery is not null)
        {
            source = orderedQuery;
        }

        return source;
    }
}
