namespace Pozitron.QuerySpecification;

public class InMemorySpecificationEvaluator
{
    // Will use singleton for default configuration. Yet, it can be instantiated if necessary, with default or provided evaluators.
    public static InMemorySpecificationEvaluator Default = new();

    protected List<IInMemoryEvaluator> Evaluators { get; }

    public InMemorySpecificationEvaluator()
    {
        Evaluators =
        [
            WhereEvaluator.Instance,
            SearchEvaluator.Instance,
            OrderEvaluator.Instance,
            PaginationEvaluator.Instance
        ];
    }
    public InMemorySpecificationEvaluator(IEnumerable<IInMemoryEvaluator> evaluators)
    {
        Evaluators = evaluators.ToList();
    }

    public virtual IEnumerable<TResult> Evaluate<T, TResult>(IEnumerable<T> source, Specification<T, TResult> specification)
    {
        if (specification.Selector is null && specification.SelectorMany is null) throw new SelectorNotFoundException();
        if (specification.Selector != null && specification.SelectorMany != null) throw new ConcurrentSelectorsException();

        var baseQuery = Evaluate(source, (Specification<T>)specification);

        var resultQuery = specification.Selector != null
          ? baseQuery.Select(specification.Selector.Compile())
          : baseQuery.SelectMany(specification.SelectorMany!.Compile());

        return resultQuery;
    }

    public virtual IEnumerable<T> Evaluate<T>(IEnumerable<T> source, Specification<T> specification)
    {
        foreach (var evaluator in Evaluators)
        {
            source = evaluator.Evaluate(source, specification);
        }

        return source;
    }
}
