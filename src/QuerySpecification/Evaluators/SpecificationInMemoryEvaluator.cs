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
        ValidateSelectExpression(specification);

        source = Evaluate(source, (Specification<T>)specification, true);

        var result = specification.SelectExpression!.Selector is not null
          ? source.Select(specification.SelectExpression.Selector.Compile())
          : source.SelectMany(specification.SelectExpression.SelectorMany!.Compile());

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
