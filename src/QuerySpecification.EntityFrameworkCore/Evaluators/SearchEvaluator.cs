namespace Pozitron.QuerySpecification.EntityFrameworkCore;

public class SearchEvaluator : IEvaluator
{
    private SearchEvaluator() { }
    public static SearchEvaluator Instance = new();

    public IQueryable<T> GetQuery<T>(IQueryable<T> query, Specification<T> specification) where T : class
    {
        foreach (var searchCriteria in specification.SearchExpressions.GroupBy(x => x.SearchGroup))
        {
            query = query.Search(searchCriteria);
        }

        return query;
    }
}
