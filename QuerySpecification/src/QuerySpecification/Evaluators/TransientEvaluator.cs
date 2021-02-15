using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace PozitronDev.QuerySpecification
{
    public class TransientEvaluator : ITransientSpecificationEvaluator
    {
        // Will use singleton for default configuration. Yet, it can be instantiated if necessary, with default or provided evaluators.
        public static TransientEvaluator Default { get; } = new TransientEvaluator();

        private readonly List<IEvaluator> evaluators = new List<IEvaluator>();

        public TransientEvaluator()
        {
            this.evaluators.AddRange(new IEvaluator[]
            {
                WhereEvaluator.Instance,
                OrderEvaluator.Instance,
                PaginationEvaluator.Instance
            });
        }
        public TransientEvaluator(IEnumerable<IEvaluator> evaluators)
        {
            this.evaluators.AddRange(evaluators);
        }

        public virtual List<TResult> Evaluate<T, TResult>(IEnumerable<T> source, ISpecification<T, TResult> specification) where T : class
        {
            var baseQuery = Evaluate(source, (ISpecification<T>)specification).AsQueryable();

            var resultQuery = baseQuery.Select(specification.Selector).ToList();

            return specification.InMemory == null
                ? resultQuery
                : specification.InMemory(resultQuery);
        }

        public virtual List<T> Evaluate<T>(IEnumerable<T> source, ISpecification<T> specification) where T : class
        {
            var queryable = source.AsQueryable();

            foreach (var evaluator in evaluators)
            {
                queryable = evaluator.GetQuery(queryable, specification);
            }

            return specification.InMemory == null
                ? queryable.ToList()
                : specification.InMemory(queryable.ToList());
        }
    }
}
