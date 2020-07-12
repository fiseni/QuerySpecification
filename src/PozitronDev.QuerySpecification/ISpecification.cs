using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace PozitronDev.QuerySpecification
{
    public interface ISpecification<T>
    {
        IEnumerable<Expression<Func<T, bool>>> WhereExpressions { get; }

        int Take { get; }
        int Skip { get; }
        bool IsPagingEnabled { get; }
    }
}
