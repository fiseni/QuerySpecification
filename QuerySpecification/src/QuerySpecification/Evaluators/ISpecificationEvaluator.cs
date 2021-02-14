using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PozitronDev.QuerySpecification
{
    public interface ISpecificationEvaluator<T> where T : class
    {
        IQueryable<TResult> GetQuery<TResult>(IQueryable<T> query, ISpecification<T, TResult> specification, bool evaluateCriteriaOnly = false);
        IQueryable<T> GetQuery(IQueryable<T> query, ISpecification<T> specification, bool evaluateCriteriaOnly = false);
    }
}
