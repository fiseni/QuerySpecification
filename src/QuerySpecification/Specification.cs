using System.Linq.Expressions;

namespace Pozitron.QuerySpecification;

public class Specification<T, TResult> : Specification<T>
{
    public new ISpecificationBuilder<T, TResult> Query { get; }

    protected Specification()
        : this(SpecificationInMemoryEvaluator.Default)
    {
    }

    protected Specification(SpecificationInMemoryEvaluator specificationInMemoryEvaluator)
        : base(specificationInMemoryEvaluator)
    {
        Query = new SpecificationBuilder<T, TResult>(this);
    }

    public new virtual IEnumerable<TResult> Evaluate(IEnumerable<T> entities, bool evaluateCriteriaOnly = false)
    {
        return Evaluator.Evaluate(entities, this, evaluateCriteriaOnly);
    }

    public Expression<Func<T, TResult>>? Selector { get; internal set; }
    public Expression<Func<T, IEnumerable<TResult>>>? SelectorMany { get; internal set; }
}

public class Specification<T>
{
    protected SpecificationInMemoryEvaluator Evaluator { get; }
    public ISpecificationBuilder<T> Query { get; }

    protected Specification()
        : this(SpecificationInMemoryEvaluator.Default)
    {
    }

    protected Specification(SpecificationInMemoryEvaluator specificationInMemoryEvaluator)
    {
        Evaluator = specificationInMemoryEvaluator;
        Query = new SpecificationBuilder<T>(this);
    }

    public virtual IEnumerable<T> Evaluate(IEnumerable<T> entities, bool evaluateCriteriaOnly = false)
    {
        return Evaluator.Evaluate(entities, this, evaluateCriteriaOnly);
    }

    public virtual bool IsSatisfiedBy(T entity)
    {
        return SpecificationValidator.Default.IsValid(entity, this);
    }

    internal List<WhereExpressionInfo<T>>? _whereExpressions = null;
    internal List<SearchExpressionInfo<T>>? _searchExpressions = null;
    internal List<OrderExpressionInfo<T>>? _orderExpressions = null;
    internal List<IncludeExpressionInfo>? _includeExpressions = null;
    internal List<string>? _includeStrings = null;
    internal Dictionary<string, object>? _items = null;

    public IEnumerable<WhereExpressionInfo<T>> WhereExpressions => _whereExpressions ?? Enumerable.Empty<WhereExpressionInfo<T>>();
    public IEnumerable<SearchExpressionInfo<T>> SearchExpressions => _searchExpressions ?? Enumerable.Empty<SearchExpressionInfo<T>>();
    public IEnumerable<OrderExpressionInfo<T>> OrderExpressions => _orderExpressions ?? Enumerable.Empty<OrderExpressionInfo<T>>();
    public IEnumerable<IncludeExpressionInfo> IncludeExpressions => _includeExpressions ?? Enumerable.Empty<IncludeExpressionInfo>();
    public IEnumerable<string> IncludeStrings => _includeStrings ?? Enumerable.Empty<string>();
    public Dictionary<string, object> Items => _items ??= [];

    public int Take { get; internal set; } = -1;
    public int Skip { get; internal set; } = -1;
    
    // Based on the alignment of 8 bytes we can store 8 flags
    // So, we have space for 4 more flags for free.
    public bool IgnoreQueryFilters { get; internal set; } = false;
    public bool AsSplitQuery { get; internal set; } = false;
    public bool AsNoTracking { get; internal set; } = false;
    public bool AsNoTrackingWithIdentityResolution { get; internal set; } = false;
}
