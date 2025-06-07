namespace Pozitron.QuerySpecification;

/// <summary>
/// Evaluator to apply AsTracking to the query if the specification has AsTracking set to true.
/// </summary>
[EvaluatorDiscovery(Order = 120)]
public sealed class AsTrackingEvaluator : IEvaluator
{
    /// <summary>
    /// Gets the singleton instance of the <see cref="AsTrackingEvaluator"/> class.
    /// </summary>
    public static AsTrackingEvaluator Instance = new();
    private AsTrackingEvaluator() { }

    /// <inheritdoc/>
    public IQueryable<T> Evaluate<T>(IQueryable<T> source, Specification<T> specification) where T : class
    {
        if (specification.AsTracking)
        {
            source = source.AsTracking();
        }

        return source;
    }
}
