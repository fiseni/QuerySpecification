namespace Pozitron.QuerySpecification;

public class AsSplitQueryEvaluator : IEvaluator
{
    private AsSplitQueryEvaluator() { }
    public static AsSplitQueryEvaluator Instance = new();

    public IQueryable<T> Evaluate<T>(IQueryable<T> source, Specification<T> specification) where T : class
    {
        if (specification.AsSplitQuery)
        {
            source = source.AsSplitQuery();
        }

        return source;
    }
}
