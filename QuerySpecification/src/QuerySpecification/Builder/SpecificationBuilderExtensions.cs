﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace PozitronDev.QuerySpecification
{
    public static class SpecificationBuilderExtensions
    {
        public static ISpecificationBuilder<T> Where<T>(
            this ISpecificationBuilder<T> specificationBuilder,
            Expression<Func<T, bool>> criteria) where T : class
        {
            ((List<Expression<Func<T, bool>>>)specificationBuilder.Specification.WhereExpressions).Add(criteria);

            return specificationBuilder;
        }

        public static IOrderedSpecificationBuilder<T> OrderBy<T>(
            this ISpecificationBuilder<T> specificationBuilder,
            Expression<Func<T, object?>> orderExpression) where T : class
        {
            ((List<(Expression<Func<T, object?>> OrderExpression, OrderTypeEnum OrderType)>)specificationBuilder.Specification.OrderExpressions)
                .Add((orderExpression, OrderTypeEnum.OrderBy));

            var orderedSpecificationBuilder = new OrderedSpecificationBuilder<T>(specificationBuilder.Specification);

            return orderedSpecificationBuilder;
        }

        public static IOrderedSpecificationBuilder<T> OrderByDescending<T>(
            this ISpecificationBuilder<T> specificationBuilder,
            Expression<Func<T, object?>> orderExpression) where T : class
        {
            ((List<(Expression<Func<T, object?>> OrderExpression, OrderTypeEnum OrderType)>)specificationBuilder.Specification.OrderExpressions)
                .Add((orderExpression, OrderTypeEnum.OrderByDescending));

            var orderedSpecificationBuilder = new OrderedSpecificationBuilder<T>(specificationBuilder.Specification);

            return orderedSpecificationBuilder;
        }

        public static IIncludableSpecificationBuilder<T, TProperty> Include<T, TProperty>(
            this ISpecificationBuilder<T> specificationBuilder,
            Expression<Func<T, TProperty>> includeExpression) where T : class
        {
            var info = new IncludeExpressionInfo(includeExpression, typeof(T), typeof(TProperty));
            
            ((List<IncludeExpressionInfo>)specificationBuilder.Specification.IncludeExpressions).Add(info);

            var includeBuilder = new IncludableSpecificationBuilder<T, TProperty>(specificationBuilder.Specification);

            return includeBuilder;
        }

        public static ISpecificationBuilder<T> Include<T>(
            this ISpecificationBuilder<T> specificationBuilder,
            string includeString) where T : class
        {
            ((List<string>)specificationBuilder.Specification.IncludeStrings).Add(includeString);
            return specificationBuilder;
        }


        public static ISpecificationBuilder<T> Search<T>(
            this ISpecificationBuilder<T> specificationBuilder,
            Expression<Func<T, string>> selector, 
            string searchTerm, 
            int searchGroup = 1) where T : class
        {
            ((List<(Expression<Func<T, string>> Selector, string SearchTerm, int SearchGroup)>)specificationBuilder.Specification.SearchCriterias)
                .Add((selector, searchTerm, searchGroup));
            
            return specificationBuilder;
        }

        public static ISpecificationBuilder<T> Take<T>(
            this ISpecificationBuilder<T> specificationBuilder,
            int take) where T : class
        {
            if (specificationBuilder.Specification.Take != null) throw new DuplicateTakeException();

            specificationBuilder.Specification.Take = take;
            specificationBuilder.Specification.IsPagingEnabled = true;
            return specificationBuilder;
        }

        public static ISpecificationBuilder<T> Skip<T>(
            this ISpecificationBuilder<T> specificationBuilder,
            int skip) where T : class
        {
            if (specificationBuilder.Specification.Skip != null) throw new DuplicateSkipException();

            specificationBuilder.Specification.Skip = skip;
            specificationBuilder.Specification.IsPagingEnabled = true;
            return specificationBuilder;
        }

        public static ISpecificationBuilder<T> Paginate<T>(
            this ISpecificationBuilder<T> specificationBuilder,
            int skip, 
            int take) where T : class
        {
            specificationBuilder.Skip(skip);
            specificationBuilder.Take(take);
            
            return specificationBuilder;
        }

        public static ISpecificationBuilder<T> InMemory<T>(
            this ISpecificationBuilder<T> specificationBuilder,
            Func<List<T>, List<T>> predicate) where T : class
        {
            specificationBuilder.Specification.InMemory = predicate;
            
            return specificationBuilder;
        }

        public static ISpecificationBuilder<T, TResult> Select<T, TResult>(
            this ISpecificationBuilder<T, TResult> specificationBuilder,
            Expression<Func<T, TResult>> selector) where T : class
        {
            specificationBuilder.Specification.Selector = selector;

            return specificationBuilder;
        }

        public static ISpecificationBuilder<T, TResult> InMemory<T, TResult>(
            this ISpecificationBuilder<T, TResult> specificationBuilder,
            Func<List<TResult>, List<TResult>> predicate) where T : class
        {
            specificationBuilder.Specification.InMemory = predicate;
            
            return specificationBuilder;
        }

        public static ISpecificationBuilder<T> EnableCache<T>(
            this ISpecificationBuilder<T> specificationBuilder,
            string specificationName, params object[] args) where T : class
        {
            _ = specificationName ?? throw new ArgumentNullException(nameof(specificationName));

            specificationBuilder.Specification.CacheKey = $"{specificationName}-{string.Join("-", args)}";

            specificationBuilder.Specification.CacheEnabled = true;

            return specificationBuilder;
        }

        public static ISpecificationBuilder<T> AsNoTracking<T>(
            this ISpecificationBuilder<T> specificationBuilder) where T : class
        {
            specificationBuilder.Specification.AsNoTracking = true;

            return specificationBuilder;
        }
    }
}
