using Microsoft.EntityFrameworkCore;

namespace Pozitron.QuerySpecification.EntityFrameworkCore;

public class AsNoTrackingWithIdentityResolutionEvaluator : IEvaluator
{
    private AsNoTrackingWithIdentityResolutionEvaluator() { }
    public static AsNoTrackingWithIdentityResolutionEvaluator Instance { get; } = new AsNoTrackingWithIdentityResolutionEvaluator();

    public bool IsCriteriaEvaluator { get; } = true;

    public IQueryable<T> GetQuery<T>(IQueryable<T> query, Specification<T> specification) where T : class
    {
        if (specification.Context.AsNoTrackingWithIdentityResolution)
        {
            query = query.AsNoTrackingWithIdentityResolution();
        }

        return query;
    }
}
