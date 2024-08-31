namespace Pozitron.QuerySpecification;

public class SpecificationEvaluator : ISpecificationEvaluator
{
    // Will use singleton for default configuration. Yet, it can be instantiated if necessary, with default or provided evaluators.
    public static SpecificationEvaluator Default { get; } = new SpecificationEvaluator();

    private readonly List<IEvaluator> _evaluators = new List<IEvaluator>();

    public SpecificationEvaluator()
    {
        _evaluators.AddRange(new IEvaluator[]
        {
            WhereEvaluator.Instance,
            SearchEvaluator.Instance,
            IncludeEvaluator.Instance,
            OrderEvaluator.Instance,
            PaginationEvaluator.Instance,
            AsSplitQueryEvaluator.Instance,
            AsNoTrackingEvaluator.Instance,
            AsNoTrackingWithIdentityResolutionEvaluator.Instance
        });
    }
    public SpecificationEvaluator(IEnumerable<IEvaluator> evaluators)
    {
        _evaluators.AddRange(evaluators);
    }


    public virtual IQueryable<TResult> GetQuery<T, TResult>(IQueryable<T> query, ISpecification<T, TResult> specification) where T : class
    {
        query = GetQuery(query, (ISpecification<T>)specification);

        return query.Select(specification.Selector);
    }

    public virtual IQueryable<T> GetQuery<T>(IQueryable<T> query, ISpecification<T> specification, bool evaluateCriteriaOnly = false) where T : class
    {
        var evaluators = evaluateCriteriaOnly ? _evaluators.Where(x => x.IsCriteriaEvaluator) : _evaluators;

        foreach (var evaluator in evaluators)
        {
            query = evaluator.GetQuery(query, specification);
        }

        return query;
    }
}
