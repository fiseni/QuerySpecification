namespace Pozitron.QuerySpecification;

/// <summary>
/// Represents an evaluator for order expressions.
/// </summary>
[EvaluatorDiscovery(Order = -80)]
public sealed class OrderEvaluator : IEvaluator, IMemoryEvaluator
{
    /// <summary>
    /// Gets the singleton instance of the <see cref="OrderEvaluator"/> class.
    /// </summary>
    public static OrderEvaluator Instance = new();
    private OrderEvaluator() { }

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

        foreach (var item in compiledItems)
        {
            if (item.Type == ItemType.Order && item.Reference is Func<T, object?> compiledExpr)
            {
                if (item.Bag == (int)OrderType.OrderBy)
                {
                    orderedQuery = source.OrderBy(compiledExpr);
                }
                else if (item.Bag == (int)OrderType.OrderByDescending)
                {
                    orderedQuery = source.OrderByDescending(compiledExpr);
                }
                else if (item.Bag == (int)OrderType.ThenBy)
                {
                    orderedQuery = orderedQuery!.ThenBy(compiledExpr);
                }
                else if (item.Bag == (int)OrderType.ThenByDescending)
                {
                    orderedQuery = orderedQuery!.ThenByDescending(compiledExpr);
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
