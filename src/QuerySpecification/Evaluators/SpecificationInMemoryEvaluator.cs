namespace Pozitron.QuerySpecification;

/// <summary>
/// Evaluates specifications in memory.
/// </summary>
public class SpecificationInMemoryEvaluator
{
    /// <summary>
    /// Gets the default instance of the <see cref="SpecificationInMemoryEvaluator"/> class.
    /// </summary>
    public static SpecificationInMemoryEvaluator Default = new();

    /// <summary>
    /// Gets the list of in-memory evaluators.
    /// </summary>
    protected List<IInMemoryEvaluator> Evaluators { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SpecificationInMemoryEvaluator"/> class.
    /// </summary>
    public SpecificationInMemoryEvaluator()
    {
        Evaluators =
        [
            WhereEvaluator.Instance,
            OrderEvaluator.Instance,
            LikeMemoryEvaluator.Instance,
        ];
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SpecificationInMemoryEvaluator"/> class with the specified evaluators.
    /// </summary>
    /// <param name="evaluators">The in-memory evaluators to use.</param>
    public SpecificationInMemoryEvaluator(IEnumerable<IInMemoryEvaluator> evaluators)
    {
        Evaluators = evaluators.ToList();
    }

    /// <summary>
    /// Evaluates the given specification on the provided enumerable source and returns the result.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="source">The enumerable source.</param>
    /// <param name="specification">The specification to evaluate.</param>
    /// <param name="ignorePaging">Whether to ignore paging settings (Take/Skip) defined in the specification.</param>
    /// <returns>The evaluated enumerable result.</returns>
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

    /// <summary>
    /// Evaluates the given specification on the provided enumerable source and returns the result.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="source">The enumerable source.</param>
    /// <param name="specification">The specification to evaluate.</param>
    /// <param name="ignorePaging">Whether to ignore paging settings (Take/Skip) defined in the specification.</param>
    /// <returns>The evaluated enumerable result.</returns>
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
