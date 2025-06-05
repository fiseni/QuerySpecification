namespace Pozitron.QuerySpecification;

/// <summary>
/// Evaluator to apply IgnoreQueryFilters to the query if the specification has IgnoreQueryFilters set to true.
/// </summary>
[EvaluatorDiscovery(Order = -65)]
public sealed class IgnoreQueryFiltersEvaluator : IEvaluator
{

    /// <summary>
    /// Gets the singleton instance of the <see cref="IgnoreQueryFiltersEvaluator"/> class.
    /// </summary>
    public static IgnoreQueryFiltersEvaluator Instance = new();
    private IgnoreQueryFiltersEvaluator() { }

    /// <inheritdoc/>
    public IQueryable<T> Evaluate<T>(IQueryable<T> source, Specification<T> specification) where T : class
    {
        if (specification.IgnoreQueryFilters)
        {
            source = source.IgnoreQueryFilters();
        }

        return source;
    }
}
