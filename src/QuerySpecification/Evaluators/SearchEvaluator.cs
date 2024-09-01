namespace Pozitron.QuerySpecification;

public class SearchEvaluator : IInMemoryEvaluator
{
    private SearchEvaluator() { }
    public static SearchEvaluator Instance = new();

    public bool IsCriteriaEvaluator { get; } = true;

    public IEnumerable<T> Evaluate<T>(IEnumerable<T> query, Specification<T> specification)
    {
        foreach (var searchGroup in specification.SearchExpressions.GroupBy(x => x.SearchGroup))
        {
            query = query.Where(x => searchGroup.Any(c => c.SelectorFunc(x).Like(c.SearchTerm)));
        }

        return query;
    }
}
