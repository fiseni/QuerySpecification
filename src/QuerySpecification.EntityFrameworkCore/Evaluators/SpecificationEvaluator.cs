namespace Pozitron.QuerySpecification;

/// <summary>
/// Evaluates specifications by applying a series of evaluators.
/// </summary>
public class SpecificationEvaluator
{
    /// <summary>
    /// Gets the default instance of the <see cref="SpecificationEvaluator"/> class.
    /// </summary>
    public static SpecificationEvaluator Default = new();

    /// <summary>
    /// Gets the list of evaluators.
    /// </summary>
    protected List<IEvaluator> Evaluators { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SpecificationEvaluator"/> class.
    /// </summary>
    public SpecificationEvaluator()
    {
        Evaluators = TypeDiscovery.IsAutoDiscoveryEnabled
            ? TypeDiscovery.GetEvaluators()
            :
            [
                WhereEvaluator.Instance,
                LikeEvaluator.Instance,
                IncludeStringEvaluator.Instance,
                IncludeEvaluator.Instance,
                OrderEvaluator.Instance,
                QueryTagEvaluator.Instance,
                IgnoreAutoIncludesEvaluator.Instance,
                IgnoreQueryFiltersEvaluator.Instance,
                AsSplitQueryEvaluator.Instance,
                AsNoTrackingEvaluator.Instance,
                AsNoTrackingWithIdentityResolutionEvaluator.Instance,
                AsTrackingEvaluator.Instance,
            ];
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SpecificationEvaluator"/> class with the specified evaluators.
    /// </summary>
    /// <param name="evaluators">The evaluators to use.</param>
    public SpecificationEvaluator(IEnumerable<IEvaluator> evaluators)
    {
        Evaluators = evaluators.ToList();
    }

    /// <summary>
    /// Evaluates the given specification on the provided queryable source and returns the result.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="source">The queryable source.</param>
    /// <param name="specification">The specification to evaluate.</param>
    /// <param name="ignorePaging">Whether to ignore paging settings (Take/Skip) defined in the specification.</param>
    /// <returns>The evaluated queryable result.</returns>
    public virtual IQueryable<TResult> Evaluate<T, TResult>(
        IQueryable<T> source,
        Specification<T, TResult> specification,
        bool ignorePaging = false) where T : class
    {
        ArgumentNullException.ThrowIfNull(specification);

        var selector = specification.Selector;
        var selectorMany = specification.SelectorMany;

        if (selector is null && selectorMany is null)
        {
            throw new SelectorNotFoundException();
        }

        source = Evaluate(source, (Specification<T>)specification, true);

        var resultQuery = selector is not null
            ? source.Select(selector)
            : source.SelectMany(selectorMany!);

        return ignorePaging
            ? resultQuery
            : resultQuery.ApplyPaging(specification);
    }

    /// <summary>
    /// Evaluates the given specification on the provided queryable source and returns the result.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="source">The queryable source.</param>
    /// <param name="specification">The specification to evaluate.</param>
    /// <param name="ignorePaging">Whether to ignore paging settings (Take/Skip) defined in the specification.</param>
    /// <returns>The evaluated queryable result.</returns>
    public virtual IQueryable<T> Evaluate<T>(
        IQueryable<T> source,
        Specification<T> specification,
        bool ignorePaging = false) where T : class
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
