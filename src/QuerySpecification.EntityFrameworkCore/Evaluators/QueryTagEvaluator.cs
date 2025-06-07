namespace Pozitron.QuerySpecification;

/// <summary>
/// Evaluator to apply the query tags to the query.
/// </summary>
[EvaluatorDiscovery(Order = 60)]
public sealed class QueryTagEvaluator : IEvaluator
{

    /// <summary>
    /// Gets the singleton instance of the <see cref="QueryTagEvaluator"/> class.
    /// </summary>
    public static QueryTagEvaluator Instance = new();
    private QueryTagEvaluator() { }

    /// <inheritdoc/>
    public IQueryable<T> Evaluate<T>(IQueryable<T> source, Specification<T> specification) where T : class
    {
        foreach (var item in specification.Items)
        {
            if (item.Type == ItemType.QueryTag && item.Reference is string tag)
            {
                source = source.TagWith(tag);
            }
        }

        return source;
    }
}
