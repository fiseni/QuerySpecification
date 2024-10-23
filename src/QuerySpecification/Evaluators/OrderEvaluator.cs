namespace Pozitron.QuerySpecification;

public sealed class OrderEvaluator : IEvaluator, IInMemoryEvaluator
{
    private OrderEvaluator() { }
    public static OrderEvaluator Instance = new();

    public IQueryable<T> Evaluate<T>(IQueryable<T> source, Specification<T> specification) where T : class
    {
        if (specification.IsEmpty) return source;

        IOrderedQueryable<T>? orderedQuery = null;

        foreach (var item in specification._state)
        {
            if (item is OrderByExpression<T> orderByExpression)
            {
                orderedQuery = source.OrderBy(orderByExpression.KeySelector);
            }
            else if (item is OrderByDescendingExpression<T> orderByDescendingExpression)
            {
                orderedQuery = source.OrderByDescending(orderByDescendingExpression.KeySelector);
            }
            else if (item is OrderThenByExpression<T> orderThenByExpression)
            {
                orderedQuery = orderedQuery!.ThenBy(orderThenByExpression.KeySelector);
            }
            else if (item is OrderThenByDescendingExpression<T> orderThenByDescendingExpression)
            {
                orderedQuery = orderedQuery!.ThenByDescending(orderThenByDescendingExpression.KeySelector);
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
        if (specification.IsEmpty) return source;

        IOrderedEnumerable<T>? orderedQuery = null;

        foreach (var item in specification._state)
        {
            if (item is OrderByExpression<T> orderByExpression)
            {
                orderedQuery = source.OrderBy(orderByExpression.KeySelectorFunc);
            }
            else if (item is OrderByDescendingExpression<T> orderByDescendingExpression)
            {
                orderedQuery = source.OrderByDescending(orderByDescendingExpression.KeySelectorFunc);
            }
            else if (item is OrderThenByExpression<T> orderThenByExpression)
            {
                orderedQuery = orderedQuery!.ThenBy(orderThenByExpression.KeySelectorFunc);
            }
            else if (item is OrderThenByDescendingExpression<T> orderThenByDescendingExpression)
            {
                orderedQuery = orderedQuery!.ThenByDescending(orderThenByDescendingExpression.KeySelectorFunc);
            }
        }

        if (orderedQuery is not null)
        {
            source = orderedQuery;
        }

        return source;
    }
}
