﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace PozitronDev.QuerySpecification.Builder
{
    public interface ISpecificationBuilder<TSource>
    {
        ISpecificationBuilder<TSource> Where(Expression<Func<TSource, bool>> criteria);
        ISpecificationBuilder<TSource> Paginate(int skip, int take);
        IOrderedSpecificationBuilder<TSource> OrderBy(Expression<Func<TSource, object>> orderExpression);
        IOrderedSpecificationBuilder<TSource> OrderByDescending(Expression<Func<TSource, object>> orderExpression);
        ISpecificationBuilder<TSource> Include(string includeString);
        IIncludableSpecificationBuilder<TSource, TProperty> Include<TProperty>(Expression<Func<TSource, TProperty>> includeExpression);
    }
}
