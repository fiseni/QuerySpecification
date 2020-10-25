using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace PozitronDev.QuerySpecification
{
    public class SpecificationBuilder<T, TResult> : SpecificationBuilder<T>, ISpecificationBuilder<T, TResult>
    {
        private readonly Specification<T, TResult> specification;

        public SpecificationBuilder(Specification<T, TResult> specification) : base(specification)
        {
            this.specification = specification;
        }

        public ISpecificationBuilder<T, TResult> Select(Expression<Func<T, TResult>> selector)
        {
            specification.Selector = selector;
            return this;
        }

        public ISpecificationBuilder<T, TResult> InMemory(Func<List<TResult>, List<TResult>> predicate)
        {
            specification.InMemory = predicate;
            return this;
        }
    }

    public class SpecificationBuilder<T> : ISpecificationBuilder<T>
    {
        private readonly Specification<T> specification;
        private readonly IOrderedSpecificationBuilder<T> orderedSpecificationBuilder;

        public SpecificationBuilder(Specification<T> specification)
        {
            this.specification = specification;
            this.orderedSpecificationBuilder = new OrderedSpecificationBuilder<T>(specification);
        }

        public ISpecificationBuilder<T> Where(Expression<Func<T, bool>> criteria)
        {
            ((List<Expression<Func<T, bool>>>)specification.WhereExpressions).Add(criteria);
            return this;
        }

        public IOrderedSpecificationBuilder<T> OrderBy(Expression<Func<T, object?>> orderExpression)
        {
            ((List<(Expression<Func<T, object?>> OrderExpression, OrderTypeEnum OrderType)>)specification.OrderExpressions).Add((orderExpression, OrderTypeEnum.OrderBy));
            return orderedSpecificationBuilder;
        }

        public IOrderedSpecificationBuilder<T> OrderByDescending(Expression<Func<T, object?>> orderExpression)
        {
            ((List<(Expression<Func<T, object?>> OrderExpression, OrderTypeEnum OrderType)>)specification.OrderExpressions).Add((orderExpression, OrderTypeEnum.OrderByDescending));
            return orderedSpecificationBuilder;
        }

        public IIncludableSpecificationBuilder<T, TProperty> Include<TProperty>(Expression<Func<T, TProperty>> includeExpression)
        {
            var aggregator = new IncludeAggregator((includeExpression.Body as MemberExpression)?.Member?.Name);
            var includeBuilder = new IncludableSpecificationBuilder<T, TProperty>(aggregator);

            ((List<IIncludeAggregator>)specification.IncludeAggregators).Add(aggregator);
            return includeBuilder;
        }

        public ISpecificationBuilder<T> Include(string includeString)
        {
            ((List<string>)specification.IncludeStrings).Add(includeString);
            return this;
        }

        public ISpecificationBuilder<T> Search(Expression<Func<T, string>> selector, string searchTerm, int searchGroup = 1)
        {
            ((List<(Expression<Func<T, string>> Selector, string SearchTerm, int SearchGroup)>)specification.SearchCriterias).Add((selector, searchTerm, searchGroup));
            return this;
        }


        public ISpecificationBuilder<T> Take(int take)
        {
            if (specification.Take != null) throw new DuplicateTakeException();

            specification.Take = take;
            specification.IsPagingEnabled = true;
            return this;
        }

        public ISpecificationBuilder<T> Skip(int skip)
        {
            if (specification.Skip != null) throw new DuplicateSkipException();

            specification.Skip = skip;
            specification.IsPagingEnabled = true;
            return this;
        }

        public ISpecificationBuilder<T> Paginate(int skip, int take)
        {
            Skip(skip);
            Take(take);
            return this;
        }


        public ISpecificationBuilder<T> InMemory(Func<List<T>, List<T>> predicate)
        {
            specification.InMemory = predicate;
            return this;
        }
    }
}
