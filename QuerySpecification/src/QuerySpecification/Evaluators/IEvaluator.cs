using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PozitronDev.QuerySpecification
{
    public interface IEvaluator<T> where T : class
    {
        bool IsCriteriaEvaluator { get; }

        IQueryable<T> GetQuery(IQueryable<T> query, ISpecification<T> specification);
    }
}
