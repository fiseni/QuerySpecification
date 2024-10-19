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
        ValidateSelectExpression(specification);

        source = Evaluate(source, (Specification<T>)specification, true);

        var resultQuery = specification.SelectExpression!.Selector is not null
          ? source.Select(specification.SelectExpression.Selector)
          : source.SelectMany(specification.SelectExpression.SelectorMany!);

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

    private static void ValidateSelectExpression<T, TResult>(Specification<T, TResult> spec)
    {
        if (spec.SelectExpression is null)
            throw new SelectorNotFoundException();

        if (spec.SelectExpression.Selector is null && spec.SelectExpression.SelectorMany is null)
            throw new SelectorNotFoundException();

        if (spec.SelectExpression.Selector is not null && spec.SelectExpression.SelectorMany is not null)
            throw new ConcurrentSelectorsException();
    }
}
