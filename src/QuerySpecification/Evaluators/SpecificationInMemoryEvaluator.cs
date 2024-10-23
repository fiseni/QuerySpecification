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
            LikeMemoryEvaluator.Instance,
            OrderEvaluator.Instance,
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
        var selectExpression = specification.SelectExpression.Validate();

        source = Evaluate(source, (Specification<T>)specification, true);

        var result = selectExpression.Selector is not null
          ? source.Select(selectExpression.Selector.Compile())
          : source.SelectMany(selectExpression.SelectorMany!.Compile());

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
