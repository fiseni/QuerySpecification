using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace PozitronDev.QuerySpecification
{
    public class TransientSpecificationEvaluator : ITransientSpecificationEvaluator
    {
        // Will use singleton for default configuration. Yet, it can be instantiated if necessary, with default or provided evaluators.
        public static TransientSpecificationEvaluator Default { get; } = new TransientSpecificationEvaluator();

        private readonly List<ITransientEvaluator> evaluators = new List<ITransientEvaluator>();

        public TransientSpecificationEvaluator()
        {
            this.evaluators.AddRange(new ITransientEvaluator[]
            {
                WhereEvaluator.Instance,
                OrderEvaluator.Instance,
                PaginationEvaluator.Instance
            });
        }
        public TransientSpecificationEvaluator(IEnumerable<ITransientEvaluator> evaluators)
        {
            this.evaluators.AddRange(evaluators);
        }

        public virtual IEnumerable<TResult> Evaluate<T, TResult>(IEnumerable<T> source, ISpecification<T, TResult> specification)
        {
            _ = specification.Selector ?? throw new SelectorNotFoundException();

            var baseQuery = Evaluate(source, (ISpecification<T>)specification);

            var resultQuery = baseQuery.Select(specification.Selector.Compile());

            return specification.InMemory == null
                ? resultQuery
                : specification.InMemory(resultQuery);
        }

        public virtual IEnumerable<T> Evaluate<T>(IEnumerable<T> source, ISpecification<T> specification)
        {
            foreach (var evaluator in evaluators)
            {
                source = evaluator.Evaluate(source, specification);
            }

            return specification.InMemory == null
                ? source
                : specification.InMemory(source);
        }
    }
}
