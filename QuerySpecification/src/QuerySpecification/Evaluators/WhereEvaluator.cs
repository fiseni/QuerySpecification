using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PozitronDev.QuerySpecification
{
    public class WhereEvaluator<T> : IEvaluator<T> where T : class
    {
        private WhereEvaluator() { }
        public static WhereEvaluator<T> Instance { get; } = new WhereEvaluator<T>();

        public bool IsCriteriaEvaluator { get; } =  true;

        public IQueryable<T> GetQuery(IQueryable<T> query, ISpecification<T> specification)
        {
            foreach (var criteria in specification.WhereExpressions)
            {
                query = query.Where(criteria);
            }

            return query;
        }
    }
}
