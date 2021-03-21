using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PozitronDev.QuerySpecification.EntityFrameworkCore3
{
    public class AsSplitQueryEvaluator : IEvaluator
    {
        private AsSplitQueryEvaluator() { }
        public static AsSplitQueryEvaluator Instance { get; } = new AsSplitQueryEvaluator();

        public bool IsCriteriaEvaluator { get; } = true;

        public IQueryable<T> GetQuery<T>(IQueryable<T> query, ISpecification<T> specification) where T : class
        {
            if (specification.AsSplitQuery)
            {
                // No support in EF Core 3
                //query = query.AsSplitQuery();
            }

            return query;
        }
    }
}
