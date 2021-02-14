using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PozitronDev.QuerySpecification
{
    // This class is not used as base by the evaluator in plugin package anymore, since we have to ensure proper order of evaluation.
    // For an example Search should be evaluated before pagination.
    // This base class remains just for legacy reasons and for unit tests.
    public class SpecificationEvaluatorBase<T> : ISpecificationEvaluator<T> where T : class
    {
        private readonly List<IEvaluator<T>> evaluators = new List<IEvaluator<T>>();

        public SpecificationEvaluatorBase(IEnumerable<IEvaluator<T>> evaluators)
        {
            this.evaluators.AddRange(evaluators);
        }
        public SpecificationEvaluatorBase()
        {
            this.evaluators.AddRange(new IEvaluator<T>[]
            {
                WhereEvaluator<T>.Instance,
                OrderEvaluator<T>.Instance,
                PaginationEvaluator<T>.Instance
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
