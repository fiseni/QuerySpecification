using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PozitronDev.QuerySpecification.EntityFrameworkCore3
{
    public class SearchEvaluator<T> : IEvaluator<T> where T : class
    {
        private SearchEvaluator() { }
        public static SearchEvaluator<T> Instance { get; } = new SearchEvaluator<T>();

        public bool IsCriteriaEvaluator { get; } = true;

        public IQueryable<T> GetQuery(IQueryable<T> query, ISpecification<T> specification)
        {
            foreach (var searchCriteria in specification.SearchCriterias.GroupBy(x => x.SearchGroup))
            {
                var criterias = searchCriteria.Select(x => (x.Selector, x.SearchTerm));
                query = query.Search(criterias);
            }

            return query;
        }
    }
}
