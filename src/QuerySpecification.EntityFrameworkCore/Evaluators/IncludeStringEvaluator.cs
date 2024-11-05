namespace Pozitron.QuerySpecification;

public sealed class IncludeStringEvaluator : IEvaluator
{
    private IncludeStringEvaluator() { }
    public static IncludeStringEvaluator Instance = new();

    public IQueryable<T> Evaluate<T>(IQueryable<T> source, Specification<T> specification) where T : class
    {
        foreach (var item in specification.Items)
        {
            if (item.Type == ItemType.IncludeString && item.Reference is string includeString)
            {
                source = source.Include(includeString);
            }
        }

        return source;
    }
}
