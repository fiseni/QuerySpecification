using System.Linq.Expressions;

namespace Pozitron.QuerySpecification;

public class SpecificationContext<T, TResult> : SpecificationContext<T>
{
    public Expression<Func<T, TResult>>? Selector { get; internal set; }

    public Expression<Func<T, IEnumerable<TResult>>>? SelectorMany { get; internal set; }
    public new Func<IEnumerable<TResult>, IEnumerable<TResult>>? PostProcessingAction { get; internal set; } = null;
}

public class SpecificationContext<T>
{

    public IEnumerable<WhereExpressionInfo<T>> WhereExpressions { get; } = [];

    public IEnumerable<OrderExpressionInfo<T>> OrderExpressions { get; } = [];

    public IEnumerable<IncludeExpressionInfo> IncludeExpressions { get; } = [];

    public IEnumerable<string> IncludeStrings { get; } = [];

    public IEnumerable<SearchExpressionInfo<T>> SearchCriterias { get; } = [];
    public IDictionary<string, object> Items { get; set; } = new Dictionary<string, object>();
    public Func<IEnumerable<T>, IEnumerable<T>>? PostProcessingAction { get; internal set; } = null;

    public int? Take { get; internal set; } = null;

    public int? Skip { get; internal set; } = null;


    public string? CacheKey { get; internal set; }

    public bool CacheEnabled { get; internal set; }

    public bool AsTracking { get; internal set; } = false;

    public bool AsNoTracking { get; internal set; } = false;

    public bool AsSplitQuery { get; internal set; } = false;

    public bool AsNoTrackingWithIdentityResolution { get; internal set; } = false;

    public bool IgnoreQueryFilters { get; internal set; } = false;
}
