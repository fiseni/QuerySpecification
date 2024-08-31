using Microsoft.EntityFrameworkCore;

namespace Pozitron.QuerySpecification.EntityFrameworkCore;

public class IgnoreQueryFiltersEvaluator : IEvaluator
{
    private IgnoreQueryFiltersEvaluator() { }
    public static IgnoreQueryFiltersEvaluator Instance { get; } = new IgnoreQueryFiltersEvaluator();

    public bool IsCriteriaEvaluator { get; } = true;

    public IQueryable<T> GetQuery<T>(IQueryable<T> query, Specification<T> specification) where T : class
    {
        if (specification.Context.IgnoreQueryFilters)
        {
            query = query.IgnoreQueryFilters();
        }

        return query;
    }
}
