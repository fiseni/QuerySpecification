﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PozitronDev.QuerySpecification.EntityFrameworkCore3
{
    public class SpecificationEvaluator<T> : SpecificationEvaluatorBase<T> where T : class
    {
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

            foreach (var searchCriteria in specification.SearchCriterias.GroupBy(x => x.SearchGroup))
            {
                var criterias = searchCriteria.Select(x => (x.Selector, x.SearchTerm));
                query = query.Search(criterias);
            }

            return query;
        }
    }
}
