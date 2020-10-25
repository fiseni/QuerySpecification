using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace PozitronDev.QuerySpecification
{
    public interface ISpecificationBuilder<T, TResult> : ISpecificationBuilder<T>
    {
        ISpecificationBuilder<T, TResult> Select(Expression<Func<T, TResult>> selector);
        ISpecificationBuilder<T, TResult> InMemory(Func<List<TResult>, List<TResult>> predicate);
    }

    public interface ISpecificationBuilder<T>
    {
        ISpecificationBuilder<T> Where(Expression<Func<T, bool>> criteria);
        IOrderedSpecificationBuilder<T> OrderBy(Expression<Func<T, object?>> orderExpression);
        IOrderedSpecificationBuilder<T> OrderByDescending(Expression<Func<T, object?>> orderExpression);
        IIncludableSpecificationBuilder<T, TProperty> Include<TProperty>(Expression<Func<T, TProperty>> includeExpression);
        ISpecificationBuilder<T> Include(string includeString);
        ISpecificationBuilder<T> Search(Expression<Func<T, string>> selector, string searchTerm, int searchGroup = 1);
        
        ISpecificationBuilder<T> Take(int take);
        ISpecificationBuilder<T> Skip(int skip);
        [Obsolete]
        ISpecificationBuilder<T> Paginate(int skip, int take);
        
        ISpecificationBuilder<T> InMemory(Func<List<T>, List<T>> predicate);
    }
}
