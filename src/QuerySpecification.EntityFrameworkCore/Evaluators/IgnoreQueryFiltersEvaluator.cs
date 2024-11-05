namespace Pozitron.QuerySpecification;

public sealed class IgnoreQueryFiltersEvaluator : IEvaluator
{
    private IgnoreQueryFiltersEvaluator() { }
    public static IgnoreQueryFiltersEvaluator Instance = new();

    public IQueryable<T> Evaluate<T>(IQueryable<T> source, Specification<T> specification) where T : class
    {
        if (specification.IgnoreQueryFilters)
        {
            source = source.IgnoreQueryFilters();
        }

        return source;
    }
}
