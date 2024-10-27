namespace Pozitron.QuerySpecification;

public sealed class AsNoTrackingEvaluator : IEvaluator
{
    private AsNoTrackingEvaluator() { }
    public static AsNoTrackingEvaluator Instance = new();

    public IQueryable<T> Evaluate<T>(IQueryable<T> source, Specification<T> specification) where T : class
    {
        if (specification.AsNoTracking)
        {
            source = source.AsNoTracking();
        }

        return source;
    }
}
