namespace Pozitron.QuerySpecification;

/// <summary>
/// Represents an evaluator for order expressions.
/// </summary>
[EvaluatorDiscovery(Order = 50)]
public sealed class OrderEvaluator : IEvaluator, IMemoryEvaluator, ISpecItemKind
{
    /// <summary>
    /// Gets the singleton instance of the <see cref="OrderEvaluator"/> class.
    /// </summary>
    public static readonly OrderEvaluator Instance = new();
    private OrderEvaluator() { }

    int ISpecItemKind.ItemType => Pozitron.QuerySpecification.ItemType.Order;

    /// <inheritdoc/>
    public IQueryable<T> Evaluate<T>(IQueryable<T> source, Specification<T> specification) where T : class
    {
        IOrderedQueryable<T>? orderedQuery = null;

        foreach (var item in specification.Items)
        {
            if (item.Type == ItemType.Order && item.Reference is Expression<Func<T, object?>> expr)
            {
                if (item.Bag == (int)OrderType.OrderBy)
                {
                    orderedQuery = source.OrderBy(expr);
                }
                else if (item.Bag == (int)OrderType.OrderByDescending)
                {
                    orderedQuery = source.OrderByDescending(expr);
                }
                else if (item.Bag == (int)OrderType.ThenBy)
                {
                    orderedQuery = orderedQuery!.ThenBy(expr);
                }
                else if (item.Bag == (int)OrderType.ThenByDescending)
                {
                    orderedQuery = orderedQuery!.ThenByDescending(expr);
                }
            }
        }

        if (orderedQuery is not null)
        {
            source = orderedQuery;
        }

        return source;
    }

    /// <inheritdoc/>
    public IEnumerable<T> Evaluate<T>(IEnumerable<T> source, Specification<T> specification)
    {
        var compiledItems = specification.GetCompiledItems();
        IOrderedEnumerable<T>? orderedQuery = null;

        // The sort is dispatched through SpecOrderCompiled so value-type keys use a typed selector
        // and Comparer<TKey>, avoiding per-element boxing.
        foreach (var item in compiledItems)
        {
            if (item.Type == ItemType.Order && item.Reference is SpecOrderCompiled<T> order)
            {
                if (item.Bag == (int)OrderType.OrderBy)
                {
                    orderedQuery = order.ApplyOrderBy(source, descending: false);
                }
                else if (item.Bag == (int)OrderType.OrderByDescending)
                {
                    orderedQuery = order.ApplyOrderBy(source, descending: true);
                }
                else if (item.Bag == (int)OrderType.ThenBy)
                {
                    orderedQuery = order.ApplyThenBy(orderedQuery!, descending: false);
                }
                else if (item.Bag == (int)OrderType.ThenByDescending)
                {
                    orderedQuery = order.ApplyThenBy(orderedQuery!, descending: true);
                }
            }
        }

        if (orderedQuery is not null)
        {
            source = orderedQuery;
        }

        return source;
    }
}
