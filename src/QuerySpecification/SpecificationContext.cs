using System.Linq.Expressions;

namespace Pozitron.QuerySpecification;

public class SpecificationContext<T, TResult> : SpecificationContext<T>
{
    public Expression<Func<T, TResult>>? Selector { get; internal set; }
    public Expression<Func<T, IEnumerable<TResult>>>? SelectorMany { get; internal set; }
}

public class SpecificationContext<T>
{
    internal List<WhereExpressionInfo<T>>? _whereExpressions;
    internal List<OrderExpressionInfo<T>>? _orderExpressions;
    internal List<IncludeExpressionInfo>? _includeExpressions;
    internal List<string>? _includeStrings;
    internal List<SearchExpressionInfo<T>>? _searchExpressions;

    public IEnumerable<WhereExpressionInfo<T>> WhereExpressions => _whereExpressions ?? Enumerable.Empty<WhereExpressionInfo<T>>();
    public IEnumerable<SearchExpressionInfo<T>> SearchExpressions => _searchExpressions ?? Enumerable.Empty<SearchExpressionInfo<T>>();
    public IEnumerable<OrderExpressionInfo<T>> OrderExpressions => _orderExpressions ?? Enumerable.Empty<OrderExpressionInfo<T>>();
    public IEnumerable<IncludeExpressionInfo> IncludeExpressions => _includeExpressions ?? Enumerable.Empty<IncludeExpressionInfo>();
    public IEnumerable<string> IncludeStrings => _includeStrings ?? Enumerable.Empty<string>();

    public Dictionary<string, object>? Items { get; set; } = null;

    public int? Take { get; internal set; } = null;
    public int? Skip { get; internal set; } = null;
    public bool IgnoreQueryFilters { get; internal set; } = false;
    public bool AsSplitQuery { get; internal set; } = false;
    public bool AsNoTracking { get; internal set; } = false;
    public bool AsNoTrackingWithIdentityResolution { get; internal set; } = false;
}
