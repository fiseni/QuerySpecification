namespace Pozitron.QuerySpecification;

public class PaginationEvaluator : IEvaluator, IInMemoryEvaluator
{
    private PaginationEvaluator() { }
    public static PaginationEvaluator Instance { get; } = new PaginationEvaluator();

    public bool IsCriteriaEvaluator { get; } = false;

    public IQueryable<T> GetQuery<T>(IQueryable<T> query, Specification<T> specification) where T : class
    {
        // If skip is 0, avoid adding to the IQueryable. It will generate more optimized SQL that way.
        if (specification.Context.Skip != null && specification.Context.Skip != 0)
        {
            query = query.Skip(specification.Context.Skip.Value);
        }

        if (specification.Context.Take != null)
        {
            query = query.Take(specification.Context.Take.Value);
        }

        return query;
    }

    public IEnumerable<T> Evaluate<T>(IEnumerable<T> query, Specification<T> specification)
    {
        if (specification.Context.Skip != null && specification.Context.Skip != 0)
        {
            query = query.Skip(specification.Context.Skip.Value);
        }

        if (specification.Context.Take != null)
        {
            query = query.Take(specification.Context.Take.Value);
        }

        return query;
    }
}
