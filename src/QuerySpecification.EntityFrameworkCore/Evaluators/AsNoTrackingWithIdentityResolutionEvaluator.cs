namespace Pozitron.QuerySpecification;

/// <summary>
/// Evaluator to apply AsNoTracking to the query if the specification has AsNoTracking set to true.
/// </summary>
[EvaluatorDiscovery(Order = 110)]
public sealed class AsNoTrackingWithIdentityResolutionEvaluator : IEvaluator
{
    /// <summary>
    /// Gets the singleton instance of the <see cref="AsNoTrackingWithIdentityResolutionEvaluator"/> class.
    /// </summary>
    public static AsNoTrackingWithIdentityResolutionEvaluator Instance = new();
    private AsNoTrackingWithIdentityResolutionEvaluator() { }

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
