namespace Pozitron.QuerySpecification;

/// <summary>
/// Evaluator to apply AsSplitQuery to the query if the specification has AsSplitQuery set to true.
/// </summary>
[EvaluatorDiscovery(Order = 90)]
public sealed class AsSplitQueryEvaluator : IEvaluator, ISpecItemKind
{
    /// <summary>
    /// Gets the singleton instance of the <see cref="AsSplitQueryEvaluator"/> class.
    /// </summary>
    public static readonly AsSplitQueryEvaluator Instance = new();
    private AsSplitQueryEvaluator() { }

    int ISpecItemKind.ItemType => Pozitron.QuerySpecification.ItemType.Flags;

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
