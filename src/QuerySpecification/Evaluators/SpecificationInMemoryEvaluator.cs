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
            OrderEvaluator.Instance,
            LikeMemoryEvaluator.Instance,
        ];
    }
    public SpecificationInMemoryEvaluator(IEnumerable<IInMemoryEvaluator> evaluators)
    {
        Evaluators = evaluators.ToList();
    }

    public virtual IEnumerable<TResult> Evaluate<T, TResult>(
        IEnumerable<T> source,
        Specification<T, TResult> specification,
        bool ignorePaging = false)
    {
        ArgumentNullException.ThrowIfNull(specification);

        var selector = specification.Selector;
        var selectorMany = specification.SelectorMany;

        if (selector is null && selectorMany is null)
        {
            throw new SelectorNotFoundException();
        }

        source = Evaluate(source, (Specification<T>)specification, true);

        var result = selector is not null
            ? source.Select(selector.Compile())
            : source.SelectMany(selectorMany!.Compile());

        return ignorePaging
            ? result
            : result.ApplyPaging(specification);
    }

    public virtual IEnumerable<T> Evaluate<T>(
        IEnumerable<T> source,
        Specification<T> specification,
        bool ignorePaging = false)
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
