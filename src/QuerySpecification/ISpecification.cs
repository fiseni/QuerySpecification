using System.Linq.Expressions;

namespace Pozitron.QuerySpecification;

public interface ISpecification<T, TResult> : ISpecification<T>
{
    new ISpecificationBuilder<T, TResult> Query { get; }

    Expression<Func<T, TResult>>? Selector { get; }

    Expression<Func<T, IEnumerable<TResult>>>? SelectorMany { get; }

    new Func<IEnumerable<TResult>, IEnumerable<TResult>>? PostProcessingAction { get; }

    new IEnumerable<TResult> Evaluate(IEnumerable<T> entities);
}

public interface ISpecification<T>
{
    ISpecificationBuilder<T> Query { get; }

    IDictionary<string, object> Items { get; set; }

    IEnumerable<WhereExpressionInfo<T>> WhereExpressions { get; }

    IEnumerable<OrderExpressionInfo<T>> OrderExpressions { get; }

    IEnumerable<IncludeExpressionInfo> IncludeExpressions { get; }

    IEnumerable<string> IncludeStrings { get; }

    IEnumerable<SearchExpressionInfo<T>> SearchCriterias { get; }

    int? Take { get; }

    int? Skip { get; }

    Func<IEnumerable<T>, IEnumerable<T>>? PostProcessingAction { get; }

    bool CacheEnabled { get; }

    string? CacheKey { get; }

    bool AsTracking { get; }

    bool AsNoTracking { get; }

    bool AsSplitQuery { get; }

    bool AsNoTrackingWithIdentityResolution { get; }

    bool IgnoreQueryFilters { get; }

    IEnumerable<T> Evaluate(IEnumerable<T> entities);

    bool IsSatisfiedBy(T entity);
}
