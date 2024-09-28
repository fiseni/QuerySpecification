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

    public virtual IQueryable<TResult> Evaluate<T, TResult>(
        IQueryable<T> source,
        Specification<T, TResult> specification,
        bool ignorePaging = false) where T : class
    {
        ArgumentNullException.ThrowIfNull(specification);
        if (specification.Selector is null && specification.SelectorMany is null) throw new SelectorNotFoundException();
        if (specification.Selector is not null && specification.SelectorMany is not null) throw new ConcurrentSelectorsException();

        source = Evaluate(source, (Specification<T>)specification, true);

        var resultQuery = specification.Selector is not null
          ? source.Select(specification.Selector)
          : source.SelectMany(specification.SelectorMany!);

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

        foreach (var evaluator in Evaluators)
        {
            source = evaluator.Evaluate(source, specification);
        }

        return ignorePaging
            ? source
            : source.ApplyPaging(specification);
    }
}
