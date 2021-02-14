using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PozitronDev.QuerySpecification.EntityFrameworkCore3
{
    public class IncludeEvaluator<T> : IEvaluator<T> where T : class
    {
        private IncludeEvaluator() { }
        public static IncludeEvaluator<T> Instance { get; } = new IncludeEvaluator<T>();

        public bool IsCriteriaEvaluator { get; } = false;

        public IQueryable<T> GetQuery(IQueryable<T> query, ISpecification<T> specification)
        {
            foreach (var includeString in specification.IncludeStrings)
            {
                query = query.Include(includeString);
            }

            foreach (var includeInfo in specification.IncludeExpressions)
            {
                if (includeInfo.Type == IncludeTypeEnum.Include)
                {
                    query = query.Include(includeInfo);
                }
                else if (includeInfo.Type == IncludeTypeEnum.ThenInclude)
                {
                    query = query.ThenInclude(includeInfo);
                }
            }

            return query;
        }
    }
}
