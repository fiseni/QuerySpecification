using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace PozitronDev.QuerySpecification
{
    public interface ISpecification<T, TResult> : ISpecification<T>
    {
        Expression<Func<T, TResult>>? Selector { get; }
        new Func<List<TResult>, List<TResult>>? InMemory { get; }
    }

    public interface ISpecification<T>
    {
        IEnumerable<Expression<Func<T, bool>>> WhereExpressions { get; }
        IEnumerable<string> IncludeStrings { get; }
        IEnumerable<IIncludeAggregator> IncludeAggregators { get; }
        IEnumerable<(Expression<Func<T, object>> KeySelector, OrderTypeEnum OrderType)> OrderExpressions { get; }
        IEnumerable<(string SearchTerm, int SearchType)> SearchCriterias { get; }

        int? Take { get; }
        int? Skip { get; }

        [Obsolete]
        bool IsPagingEnabled { get; }

        Func<List<T>, List<T>>? InMemory { get; }
    }
}
