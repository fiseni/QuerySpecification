using Microsoft.EntityFrameworkCore;

namespace Pozitron.QuerySpecification.EntityFrameworkCore;

public class IgnoreQueryFiltersEvaluator : IEvaluator
{
    private IgnoreQueryFiltersEvaluator() { }
    public static IgnoreQueryFiltersEvaluator Instance = new();

    public IQueryable<T> GetQuery<T>(IQueryable<T> query, Specification<T> specification) where T : class
    {
        if (specification.IgnoreQueryFilters)
        {
            query = query.IgnoreQueryFilters();
        }

        return query;
    }
}
