namespace Pozitron.QuerySpecification.EntityFrameworkCore;

public class SpecificationEvaluator
{
    public static SpecificationEvaluator Default { get; } = new SpecificationEvaluator();

    public static SpecificationEvaluator Cached { get; } = new SpecificationEvaluator(true);

    protected List<IEvaluator> Evaluators { get; }

    public SpecificationEvaluator(bool cacheEnabled = false)
    {
        Evaluators =
        [
            WhereEvaluator.Instance,
            SearchEvaluator.Instance,
            cacheEnabled ? IncludeEvaluator.Cached : IncludeEvaluator.Default,
            OrderEvaluator.Instance,
            PaginationEvaluator.Instance,
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

    public virtual IQueryable<TResult> GetQuery<T, TResult>(IQueryable<T> query, Specification<T, TResult> specification) where T : class
    {
        ArgumentNullException.ThrowIfNull(specification);
        if (specification.Context.Selector is null && specification.Context.SelectorMany is null) throw new SelectorNotFoundException();
        if (specification.Context.Selector is not null && specification.Context.SelectorMany is not null) throw new ConcurrentSelectorsException();

        query = GetQuery(query, (Specification<T>)specification);

        return specification.Context.Selector is not null
          ? query.Select(specification.Context.Selector)
          : query.SelectMany(specification.Context.SelectorMany!);
    }

    public virtual IQueryable<T> GetQuery<T>(IQueryable<T> query, Specification<T> specification, bool evaluateCriteriaOnly = false) where T : class
    {
        ArgumentNullException.ThrowIfNull(specification);

        var evaluators = evaluateCriteriaOnly ? Evaluators.Where(x => x.IsCriteriaEvaluator) : Evaluators;

        foreach (var evaluator in evaluators)
        {
            query = evaluator.GetQuery(query, specification);
        }

        return query;
    }
}
