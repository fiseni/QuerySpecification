using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PozitronDev.QuerySpecification.EntityFrameworkCore3
{
    public class AsNoTrackingEvaluator<T> : IEvaluator<T> where T : class
    {
        private AsNoTrackingEvaluator() { }
        public static AsNoTrackingEvaluator<T> Instance { get; } = new AsNoTrackingEvaluator<T>();

        public bool IsCriteriaEvaluator { get; } = true;

        public IQueryable<T> GetQuery(IQueryable<T> query, ISpecification<T> specification)
        {
            if (specification.AsNoTracking)
            {
                query = query.AsNoTracking();
            }

            return query;
        }
    }
}
