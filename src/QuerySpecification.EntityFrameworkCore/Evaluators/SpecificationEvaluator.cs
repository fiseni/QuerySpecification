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

    public virtual IQueryable<TResult> GetQuery<T, TResult>(
        IQueryable<T> query,
        Specification<T, TResult> specification,
        bool ignorePaging = false) where T : class
    {
        ArgumentNullException.ThrowIfNull(specification);
        if (specification.Selector is null && specification.SelectorMany is null) throw new SelectorNotFoundException();
        if (specification.Selector is not null && specification.SelectorMany is not null) throw new ConcurrentSelectorsException();

        query = GetQuery(query, (Specification<T>)specification, true);

        var resultQuery = specification.Selector is not null
          ? query.Select(specification.Selector)
          : query.SelectMany(specification.SelectorMany!);

        return ignorePaging
            ? resultQuery
            : resultQuery.ApplyPaging(specification);
    }

    public virtual IQueryable<T> GetQuery<T>(
        IQueryable<T> query,
        Specification<T> specification,
        bool ignorePaging = false) where T : class
    {
        ArgumentNullException.ThrowIfNull(specification);

        foreach (var evaluator in Evaluators)
        {
            query = evaluator.GetQuery(query, specification);
        }

        return ignorePaging
            ? query
            : query.ApplyPaging(specification);
    }
}
