using Microsoft.EntityFrameworkCore;

namespace Pozitron.QuerySpecification.EntityFrameworkCore;

public class AsSplitQueryEvaluator : IEvaluator
{
    private AsSplitQueryEvaluator() { }
    public static AsSplitQueryEvaluator Instance { get; } = new AsSplitQueryEvaluator();

    public bool IsCriteriaEvaluator { get; } = true;

    public IQueryable<T> GetQuery<T>(IQueryable<T> query, Specification<T> specification) where T : class
    {
        if (specification.Context.AsSplitQuery)
        {
            query = query.AsSplitQuery();
        }

        return query;
    }
}
