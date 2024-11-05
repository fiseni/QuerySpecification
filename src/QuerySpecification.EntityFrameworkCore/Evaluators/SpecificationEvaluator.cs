namespace Pozitron.QuerySpecification;

public class SpecificationEvaluator
{
    public static SpecificationEvaluator Default = new();

    protected List<IEvaluator> Evaluators { get; }

    public SpecificationEvaluator()
    {
        Evaluators =
        [
            WhereEvaluator.Instance,
            LikeEvaluator.Instance,
            IncludeStringEvaluator.Instance,
            IncludeEvaluator.Instance,
            OrderEvaluator.Instance,
            AsNoTrackingEvaluator.Instance,
            AsNoTrackingWithIdentityResolutionEvaluator.Instance,
            IgnoreQueryFiltersEvaluator.Instance,
            AsSplitQueryEvaluator.Instance
        ];
    }

    public SpecificationEvaluator(IEnumerable<IEvaluator> evaluators)
    {
        Evaluators = evaluators.ToList();
    }

    public virtual IQueryable<TResult> Evaluate<T, TResult>(
        IQueryable<T> source,
        Specification<T, TResult> specification,
        bool ignorePaging = false) where T : class
    {
        ArgumentNullException.ThrowIfNull(specification);

        var selector = specification.Selector;
        var selectorMany = specification.SelectorMany;

        if (selector is null && selectorMany is null)
        {
            throw new SelectorNotFoundException();
        }

        source = Evaluate(source, (Specification<T>)specification, true);

        var resultQuery = selector is not null 
            ? source.Select(selector)
            : source.SelectMany(selectorMany!);

        return ignorePaging
            ? resultQuery
            : resultQuery.ApplyPaging(specification);
    }

    public virtual IQueryable<T> Evaluate<T>(
        IQueryable<T> source,
        Specification<T> specification,
        bool ignorePaging = false) where T : class
    {
        ArgumentNullException.ThrowIfNull(specification);
        if (specification.IsEmpty) return source;

        foreach (var evaluator in Evaluators)
        {
            source = evaluator.Evaluate(source, specification);
        }

        return ignorePaging
            ? source
            : source.ApplyPaging(specification);
    }
}
