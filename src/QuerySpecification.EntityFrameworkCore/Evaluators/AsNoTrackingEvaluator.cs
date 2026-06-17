namespace Pozitron.QuerySpecification;

/// <summary>
/// Evaluator to apply AsNoTracking to the query if the specification has AsNoTracking set to true.
/// </summary>
[EvaluatorDiscovery(Order = 100)]
public sealed class AsNoTrackingEvaluator : IEvaluator, ISpecItemKind
{
    /// <summary>
    /// Gets the singleton instance of the <see cref="AsNoTrackingEvaluator"/> class.
    /// </summary>
    public static AsNoTrackingEvaluator Instance = new();
    private AsNoTrackingEvaluator() { }

    int ISpecItemKind.ItemType => Pozitron.QuerySpecification.ItemType.Flags;

    /// <inheritdoc/>
    public IQueryable<T> Evaluate<T>(IQueryable<T> source, Specification<T> specification) where T : class
    {
        if (specification.AsNoTracking)
        {
            source = source.AsNoTracking();
        }

        return source;
    }
}
