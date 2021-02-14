using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PozitronDev.QuerySpecification
{
    public class PaginationEvaluator<T> : IEvaluator<T> where T : class
    {
        private PaginationEvaluator() { }
        public static PaginationEvaluator<T> Instance { get; } = new PaginationEvaluator<T>();

        public bool IsCriteriaEvaluator { get; } =  false;

        public IQueryable<T> GetQuery(IQueryable<T> query, ISpecification<T> specification)
        {
            // If skip is 0, avoid adding to the IQueryable. It will generate more optimized SQL that way.
            if (specification.Skip != null && specification.Skip != 0)
            {
                query = query.Skip(specification.Skip.Value);
            }

            if (specification.Take != null)
            {
                query = query.Take(specification.Take.Value);
            }

            return query;
        }
    }
}
