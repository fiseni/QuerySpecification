namespace Pozitron.QuerySpecification;

public class AsNoTrackingWithIdentityResolutionEvaluator : IEvaluator
{
    private AsNoTrackingWithIdentityResolutionEvaluator() { }
    public static AsNoTrackingWithIdentityResolutionEvaluator Instance = new();

    public IQueryable<T> Evaluate<T>(IQueryable<T> source, Specification<T> specification) where T : class
    {
        if (specification.AsNoTrackingWithIdentityResolution)
        {
            source = source.AsNoTrackingWithIdentityResolution();
        }

        return source;
    }
}
