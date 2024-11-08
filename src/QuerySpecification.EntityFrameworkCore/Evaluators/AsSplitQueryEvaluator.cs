namespace Pozitron.QuerySpecification;

/// <summary>
/// Evaluator to apply AsSplitQuery to the query if the specification has AsSplitQuery set to true.
/// </summary>
public sealed class AsSplitQueryEvaluator : IEvaluator
{
    /// <summary>
    /// Gets the singleton instance of the <see cref="AsSplitQueryEvaluator"/> class.
    /// </summary>
    public static AsSplitQueryEvaluator Instance = new();
    private AsSplitQueryEvaluator() { }

    /// <inheritdoc/>
    public IQueryable<T> Evaluate<T>(IQueryable<T> source, Specification<T> specification) where T : class
    {
        if (specification.AsSplitQuery)
        {
            source = source.AsSplitQuery();
        }

        return source;
    }
}
