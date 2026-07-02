namespace Pozitron.QuerySpecification;

/// <summary>
/// Evaluates specifications by applying a series of evaluators.
/// </summary>
public class SpecificationEvaluator
{
    /// <summary>
    /// Gets the default instance of the <see cref="SpecificationEvaluator"/> class.
    /// </summary>
    public static readonly SpecificationEvaluator Default = new();

    /// <summary>
    /// Gets the list of evaluators.
    /// </summary>
    protected List<IEvaluator> Evaluators { get; }

    // Snapshot of Evaluators with each evaluator's kind bit precomputed, so Evaluate avoids the per-call
    // interface checks. Built lazily and rebuilt if the list count changes (e.g. a derived type added evaluators).
    private KindedEvaluator[]? _kindedEvaluators;

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
                // A single evaluator that reads the flags bag once and applies all flag operations,
                // replacing the six individual flag evaluators (still used by the auto-discovery path).
                FlagsEvaluator.Instance,
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

        // A single scan; the spec holds at most one Select item (Select and SelectMany overwrite each other).
        var selectItem = specification.FirstOrDefault<object>(ItemType.Select);
        var selector = selectItem as Expression<Func<T, TResult>>;
        var selectorMany = selector is null ? selectItem as Expression<Func<T, IEnumerable<TResult>>> : null;

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

        // One pass to find which kinds are present, then skip evaluators whose kind is absent (O(1) bit test)
        // instead of having each evaluator scan the items only to find nothing.
        var presentKinds = specification.PresentKindsMask();

        foreach (var (evaluator, kindMask) in GetKindedEvaluators())
        {
            if (kindMask != 0 && (presentKinds & kindMask) == 0)
            {
                continue;
            }
            source = evaluator.Evaluate(source, specification);
        }

        return ignorePaging
            ? source
            : source.ApplyPaging(specification);
    }

    private KindedEvaluator[] GetKindedEvaluators()
    {
        var cache = _kindedEvaluators;
        var evaluators = Evaluators;
        if (cache is not null && cache.Length == evaluators.Count) return cache;

        cache = new KindedEvaluator[evaluators.Count];
        for (var i = 0; i < cache.Length; i++)
        {
            var evaluator = evaluators[i];
            cache[i] = new(evaluator, evaluator is ISpecItemKind kind ? 1 << -kind.ItemType : 0);
        }
        _kindedEvaluators = cache;
        return cache;
    }

    private readonly record struct KindedEvaluator(IEvaluator Evaluator, int KindMask);
}
