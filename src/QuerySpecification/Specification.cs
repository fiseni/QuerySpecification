using System.Linq.Expressions;

namespace Pozitron.QuerySpecification;

public class Specification<T, TResult> : Specification<T>
{
    public Specification()
    {
        Query = new SpecificationBuilder<T, TResult>(this);
    }

    public new ISpecificationBuilder<T, TResult> Query { get; }

    public Expression<Func<T, TResult>>? Selector { get; internal set; }
    public Expression<Func<T, IEnumerable<TResult>>>? SelectorMany { get; internal set; }

    public new virtual IEnumerable<TResult> Evaluate(IEnumerable<T> entities)
    {
        var evaluator = SpecificationInMemoryEvaluator.Default;
        return evaluator.Evaluate(entities, this);
    }
}

public class Specification<T>
{
    private List<WhereExpression<T>>? _whereExpressions;
    private List<LikeExpression<T>>? _likeExpressions;
    private List<OrderExpression<T>>? _orderExpressions;
    private List<IncludeExpression>? _includeExpressions;
    private List<string>? _includeStrings;
    private Dictionary<string, object>? _items;

    public Specification()
    {
        Query = new SpecificationBuilder<T>(this);
    }

    public ISpecificationBuilder<T> Query { get; }

    public int Take { get; internal set; } = -1;
    public int Skip { get; internal set; } = -1;

    // Based on the alignment of 8 bytes (on x64) we can store 8 flags here
    // So, we have space for 4 more flags for free.
    public bool IgnoreQueryFilters { get; internal set; } = false;
    public bool AsSplitQuery { get; internal set; } = false;
    public bool AsNoTracking { get; internal set; } = false;
    public bool AsNoTrackingWithIdentityResolution { get; internal set; } = false;

    internal void Add(WhereExpression<T> whereExpression) => (_whereExpressions ??= []).Add(whereExpression);
    internal void Add(LikeExpression<T> likeExpression) => (_likeExpressions ??= []).Add(likeExpression);
    internal void Add(OrderExpression<T> orderExpression) => (_orderExpressions ??= []).Add(orderExpression);
    internal void Add(IncludeExpression includeExpression) => (_includeExpressions ??= []).Add(includeExpression);
    internal void Add(string includeString) => (_includeStrings ??= []).Add(includeString);


    // Specs are not intended to be thread-safe, so we don't need to worry about thread-safety here.
    public Dictionary<string, object> Items => _items ??= [];
    public IEnumerable<WhereExpression<T>> WhereExpressions => _whereExpressions ?? [];
    public IEnumerable<LikeExpression<T>> LikeExpressions => _likeExpressions ?? [];
    public IEnumerable<OrderExpression<T>> OrderExpressions => _orderExpressions ?? [];
    public IEnumerable<IncludeExpression> IncludeExpressions => _includeExpressions ?? [];
    public IEnumerable<string> IncludeStrings => _includeStrings ?? [];

    public virtual IEnumerable<T> Evaluate(IEnumerable<T> entities)
    {
        var evaluator = SpecificationInMemoryEvaluator.Default;
        return evaluator.Evaluate(entities, this);
    }

    public virtual bool IsSatisfiedBy(T entity)
    {
        var validator = SpecificationValidator.Default;
        return validator.IsValid(entity, this);
    }
}
