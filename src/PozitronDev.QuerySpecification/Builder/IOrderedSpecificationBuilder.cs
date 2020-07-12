using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace PozitronDev.QuerySpecification.Builder
{
    public interface IOrderedSpecificationBuilder<TSource>
    {
        IOrderedSpecificationBuilder<TSource> ThenBy(Expression<Func<TSource, object>> orderExpression);
        IOrderedSpecificationBuilder<TSource> ThenByDescending(Expression<Func<TSource, object>> orderExpression);
    }
}
