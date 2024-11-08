namespace Pozitron.QuerySpecification;

/// <summary>
/// Evaluator to apply AsNoTracking to the query if the specification has AsNoTracking set to true.
/// </summary>
public sealed class AsNoTrackingEvaluator : IEvaluator
{
    /// <summary>
    /// Gets the singleton instance of the <see cref="AsNoTrackingEvaluator"/> class.
    /// </summary>
    public static AsNoTrackingEvaluator Instance = new();
    private AsNoTrackingEvaluator() { }

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
