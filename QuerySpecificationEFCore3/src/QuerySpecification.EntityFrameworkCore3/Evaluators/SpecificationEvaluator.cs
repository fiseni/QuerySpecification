using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PozitronDev.QuerySpecification.EntityFrameworkCore3
{
    public class SpecificationEvaluator<T> : ISpecificationEvaluator<T> where T : class
    {
        private readonly List<IEvaluator<T>> evaluators = new List<IEvaluator<T>>();

        public SpecificationEvaluator(IEnumerable<IEvaluator<T>> evaluators)
        {
            this.evaluators.AddRange(evaluators);
        }
        public SpecificationEvaluator()
        {
            this.evaluators.AddRange(new IEvaluator<T>[] 
            { 
                WhereEvaluator<T>.Instance,
                SearchEvaluator<T>.Instance,
                IncludeEvaluator<T>.Instance,
                OrderEvaluator<T>.Instance,
                PaginationEvaluator<T>.Instance,
                AsNoTrackingEvaluator<T>.Instance
            });
        }

        public virtual IQueryable<TResult> GetQuery<TResult>(IQueryable<T> query, ISpecification<T, TResult> specification, bool evaluateCriteriaOnly = false)
        {
            query = GetQuery(query, (ISpecification<T>)specification, evaluateCriteriaOnly);

            return query.Select(specification.Selector);
        }

        public virtual IQueryable<T> GetQuery(IQueryable<T> query, ISpecification<T> specification, bool evaluateCriteriaOnly = false)
        {
            var evaluators = evaluateCriteriaOnly ? this.evaluators.Where(x => x.IsCriteriaEvaluator) : this.evaluators;

            foreach (var evaluator in evaluators)
            {
                query = evaluator.GetQuery(query, specification);
            }

            return query;
        }
    }
}
