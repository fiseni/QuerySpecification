using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace PozitronDev.QuerySpecification
{
    public interface ISpecification<T, TResult> : ISpecification<T>
    {
        Expression<Func<T, TResult>>? Selector { get; }
        new Func<IEnumerable<TResult>, IEnumerable<TResult>>? InMemory { get; }
        new IEnumerable<TResult> Evaluate(IEnumerable<T> entities);
    }

    public interface ISpecification<T>
    {
        IEnumerable<Expression<Func<T, bool>>> WhereExpressions { get; }
        IEnumerable<(Expression<Func<T, object>> KeySelector, OrderTypeEnum OrderType)> OrderExpressions { get; }
        IEnumerable<IncludeExpressionInfo> IncludeExpressions { get; }
        IEnumerable<string> IncludeStrings { get; }
        IEnumerable<(Expression<Func<T, string>> Selector, string SearchTerm, int SearchGroup)> SearchCriterias { get; }

        int? Take { get; }
        int? Skip { get; }
        [Obsolete]
        bool IsPagingEnabled { get; }

        Func<IEnumerable<T>, IEnumerable<T>>? InMemory { get; }

        bool CacheEnabled { get; }
        string? CacheKey { get; }

        bool AsNoTracking { get; }
        bool AsSplitQuery { get; }

        IEnumerable<T> Evaluate(IEnumerable<T> entities);
    }
}
