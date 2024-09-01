namespace Pozitron.QuerySpecification;

public class SearchValidator : IValidator
{
    private SearchValidator() { }
    public static SearchValidator Instance { get; } = new SearchValidator();

    public bool IsValid<T>(T entity, Specification<T> specification)
    {
        foreach (var searchGroup in specification.SearchExpressions.GroupBy(x => x.SearchGroup))
        {
            if (searchGroup.Any(c => c.SelectorFunc(entity).Like(c.SearchTerm)) == false) return false;
        }

        return true;
    }
}
