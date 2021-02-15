using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace PozitronDev.QuerySpecification
{
    public interface ISpecification<T, TResult> : ISpecification<T> where T : class
    {
        Expression<Func<T, TResult>>? Selector { get; }
        new Func<List<TResult>, List<TResult>>? InMemory { get; }
        new List<TResult> Evaluate(List<T> entities);
    }

    public interface ISpecification<T> where T : class
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

        Func<List<T>, List<T>>? InMemory { get; }

        bool CacheEnabled { get; }
        string? CacheKey { get; }

        bool AsNoTracking { get; }

        List<T> Evaluate(List<T> entities);
    }
}
