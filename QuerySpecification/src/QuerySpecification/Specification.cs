using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace PozitronDev.QuerySpecification
{
    public abstract class Specification<T, TResult> : Specification<T>, ISpecification<T, TResult> where T : class
    {
        protected new virtual ISpecificationBuilder<T, TResult> Query { get; }

        protected Specification()
            : this(TransientEvaluator.Default)
        {
        }

        protected Specification(ITransientSpecificationEvaluator transientEvaluator) 
            : base(transientEvaluator)
        {
            this.Query = new SpecificationBuilder<T, TResult>(this);
        }

        public new virtual List<TResult> Evaluate(List<T> entities)
        {
            return evaluator.Evaluate(entities, this);
        }

        public Expression<Func<T, TResult>>? Selector { get; internal set; }

        public new Func<List<TResult>, List<TResult>>? InMemory { get; internal set; } = null;
    }

    public abstract class Specification<T> : ISpecification<T> where T : class
    {
        protected readonly ITransientSpecificationEvaluator evaluator;
        protected virtual ISpecificationBuilder<T> Query { get; }

        protected Specification() 
            : this(TransientEvaluator.Default)
        {
        }
        protected Specification(ITransientSpecificationEvaluator transientEvaluator)
        {
            this.evaluator = transientEvaluator;
            this.Query = new SpecificationBuilder<T>(this);
        }

        public virtual List<T> Evaluate(List<T> entities)
        {
            return evaluator.Evaluate(entities, this);
        }

        public IEnumerable<Expression<Func<T, bool>>> WhereExpressions { get; } = new List<Expression<Func<T, bool>>>();

        public IEnumerable<(Expression<Func<T, object>> KeySelector, OrderTypeEnum OrderType)> OrderExpressions { get; } =
            new List<(Expression<Func<T, object>> KeySelector, OrderTypeEnum OrderType)>();

        public IEnumerable<IncludeExpressionInfo> IncludeExpressions { get; } = new List<IncludeExpressionInfo>();

        public IEnumerable<string> IncludeStrings { get; } = new List<string>();

        public IEnumerable<(Expression<Func<T, string>> Selector, string SearchTerm, int SearchGroup)> SearchCriterias { get; } = 
            new List<(Expression<Func<T, string>> Selector, string SearchTerm, int SearchGroup)>();


        public int? Take { get; internal set; } = null;

        public int? Skip { get; internal set; } = null;

        public bool IsPagingEnabled { get; internal set; } = false;

        
        public Func<List<T>, List<T>>? InMemory { get; internal set; } = null;

        public string? CacheKey { get; internal set; }
        public bool CacheEnabled { get; internal set; }

        public bool AsNoTracking { get; internal set; } = false;
    }
}
