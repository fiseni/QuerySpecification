namespace Pozitron.QuerySpecification;

public class SpecificationInMemoryEvaluator
{
    public static SpecificationInMemoryEvaluator Default = new();

    protected List<IInMemoryEvaluator> Evaluators { get; }

    public SpecificationInMemoryEvaluator()
    {
        Evaluators =
        [
            WhereEvaluator.Instance,
            SearchEvaluator.Instance,
            OrderEvaluator.Instance,
            PaginationEvaluator.Instance
        ];
    }
    public SpecificationInMemoryEvaluator(IEnumerable<IInMemoryEvaluator> evaluators)
    {
        Evaluators = evaluators.ToList();
    }

    public virtual IEnumerable<TResult> Evaluate<T, TResult>(IEnumerable<T> source, Specification<T, TResult> specification)
    {
        ArgumentNullException.ThrowIfNull(specification);
        if (specification.Selector is null && specification.SelectorMany is null) throw new SelectorNotFoundException();
        if (specification.Selector is not null && specification.SelectorMany is not null) throw new ConcurrentSelectorsException();

        var baseQuery = Evaluate(source, (Specification<T>)specification);

        var resultQuery = specification.Selector is not null
          ? baseQuery.Select(specification.Selector.Compile())
          : baseQuery.SelectMany(specification.SelectorMany!.Compile());

        return resultQuery;
    }

    public virtual IEnumerable<T> Evaluate<T>(IEnumerable<T> source, Specification<T> specification)
    {
        ArgumentNullException.ThrowIfNull(specification);

        foreach (var evaluator in Evaluators)
        {
            source = evaluator.Evaluate(source, specification);
        }

        return source;
    }
}
