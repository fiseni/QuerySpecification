using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PozitronDev.QuerySpecification.EntityFrameworkCore3
{
    public class SpecificationEvaluator<T> : SpecificationEvaluatorBase<T> where T : class
    {
        private readonly ISearchEvaluator<T>? searchEvaluator;

        public SpecificationEvaluator()
        {
            this.searchEvaluator = null;
        }

        public SpecificationEvaluator(ISearchEvaluator<T>? searchEvaluator)
        {
            this.searchEvaluator = searchEvaluator;
        }

        public override IQueryable<TResult> GetQuery<TResult>(IQueryable<T> inputQuery, ISpecification<T, TResult> specification)
        {
            var query = GetQuery(inputQuery, (ISpecification<T>)specification);

            var selectQuery = query.Select(specification.Selector);

            return selectQuery;
        }

        public override IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> specification)
        {
            var query = base.GetQuery(inputQuery, specification);

            query = specification.IncludeStrings.Aggregate(query,
                        (current, includeString) => current.Include(includeString));

            foreach (var includeAggregator in specification.IncludeAggregators)
            {
                var includeString = includeAggregator.IncludeString;
                if (!string.IsNullOrEmpty(includeString))
                {
                    query = query.Include(includeString);
                }
            }

            foreach (var searchCriteria in specification.SearchCriterias)
            {
                var searchExpression = searchEvaluator?.GetExpression(searchCriteria.SearchTerm, searchCriteria.SearchType);

                if (searchExpression != null)
                {
                    query = query.Where(searchExpression);
                }
            }

            return query;
        }
    }
}
