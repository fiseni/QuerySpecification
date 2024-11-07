namespace Pozitron.QuerySpecification;

public sealed class AsTrackingEvaluator : IEvaluator
{
    private AsTrackingEvaluator() { }
    public static AsTrackingEvaluator Instance = new();

    public IQueryable<T> Evaluate<T>(IQueryable<T> source, Specification<T> specification) where T : class
    {
        if (specification.AsTracking)
        {
            source = source.AsTracking();
        }

        return source;
    }
}
