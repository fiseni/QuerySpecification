using System.Linq.Expressions;

namespace Pozitron.QuerySpecification;

public class Specification<T, TResult> : Specification<T>
{
    public new ISpecificationBuilder<T, TResult> Query { get; }

    protected Specification() : base()
    {
        Query = new SpecificationBuilder<T, TResult>(this);
    }

    protected Specification(SpecificationInMemoryEvaluator specificationInMemoryEvaluator)
        : base(specificationInMemoryEvaluator)
    {
        Query = new SpecificationBuilder<T, TResult>(this);
    }

    public Expression<Func<T, TResult>>? Selector { get; internal set; }
    public Expression<Func<T, IEnumerable<TResult>>>? SelectorMany { get; internal set; }
}

public class Specification<T>
{
    private SpecificationInMemoryEvaluator _evaluator;
    public ISpecificationBuilder<T> Query { get; }

    protected Specification()
        : this(SpecificationInMemoryEvaluator.Default)
    {
    }

    protected Specification(SpecificationInMemoryEvaluator specificationInMemoryEvaluator)
    {
        _evaluator = specificationInMemoryEvaluator;
        Query = new SpecificationBuilder<T>(this);
    }

    public virtual IEnumerable<T> Evaluate(IEnumerable<T> entities, bool evaluateCriteriaOnly = false)
    {
        return _evaluator.Evaluate(entities, this, evaluateCriteriaOnly);
    }

    public virtual bool IsSatisfiedBy(T entity)
    {
        return SpecificationValidator.Default.IsValid(entity, this);
    }

    internal List<WhereExpressionInfo<T>>? _whereExpressions;
    internal List<SearchExpressionInfo<T>>? _searchExpressions;
    internal List<OrderExpressionInfo<T>>? _orderExpressions;
    internal List<IncludeExpressionInfo>? _includeExpressions;
    internal List<string>? _includeStrings;

    public IEnumerable<WhereExpressionInfo<T>> WhereExpressions => _whereExpressions ?? Enumerable.Empty<WhereExpressionInfo<T>>();
    public IEnumerable<SearchExpressionInfo<T>> SearchExpressions => _searchExpressions ?? Enumerable.Empty<SearchExpressionInfo<T>>();
    public IEnumerable<OrderExpressionInfo<T>> OrderExpressions => _orderExpressions ?? Enumerable.Empty<OrderExpressionInfo<T>>();
    public IEnumerable<IncludeExpressionInfo> IncludeExpressions => _includeExpressions ?? Enumerable.Empty<IncludeExpressionInfo>();
    public IEnumerable<string> IncludeStrings => _includeStrings ?? Enumerable.Empty<string>();

    public Dictionary<string, object>? Items { get; set; } = null;

    public int Take { get; internal set; } = -1;
    public int Skip { get; internal set; } = -1;
    public bool IgnoreQueryFilters { get; internal set; } = false;
    public bool AsSplitQuery { get; internal set; } = false;
    public bool AsNoTracking { get; internal set; } = false;
    public bool AsNoTrackingWithIdentityResolution { get; internal set; } = false;
}
