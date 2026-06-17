namespace Pozitron.QuerySpecification;

/// <summary>
/// Evaluator to apply AsNoTracking to the query if the specification has AsNoTracking set to true.
/// </summary>
[EvaluatorDiscovery(Order = 110)]
public sealed class AsNoTrackingWithIdentityResolutionEvaluator : IEvaluator, ISpecItemKind
{
    /// <summary>
    /// Gets the singleton instance of the <see cref="AsNoTrackingWithIdentityResolutionEvaluator"/> class.
    /// </summary>
    public static AsNoTrackingWithIdentityResolutionEvaluator Instance = new();
    private AsNoTrackingWithIdentityResolutionEvaluator() { }

    int ISpecItemKind.ItemType => Pozitron.QuerySpecification.ItemType.Flags;

    /// <inheritdoc/>
    public IQueryable<T> Evaluate<T>(IQueryable<T> source, Specification<T> specification) where T : class
    {
        if (specification.AsNoTrackingWithIdentityResolution)
        {
            source = source.AsNoTrackingWithIdentityResolution();
        }

        return source;
    }
}
