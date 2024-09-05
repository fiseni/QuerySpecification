using System.Linq.Expressions;

namespace Pozitron.QuerySpecification;

public static class SpecificationBuilderExtensions
{
    public static ISpecificationBuilder<T> Where<T>(
        this ISpecificationBuilder<T> specificationBuilder,
        Expression<Func<T, bool>> criteria)
        => Where(specificationBuilder, criteria, true);

    public static ISpecificationBuilder<T> Where<T>(
        this ISpecificationBuilder<T> specificationBuilder,
        Expression<Func<T, bool>> criteria,
        bool condition)
    {
        if (condition)
        {
            var info = new WhereExpressionInfo<T>(criteria);
            specificationBuilder.Specification.Add(info);
        }
        return specificationBuilder;
    }

    public static IOrderedSpecificationBuilder<T> OrderBy<T>(
        this ISpecificationBuilder<T> specificationBuilder,
        Expression<Func<T, object?>> keySelector)
        => OrderBy(specificationBuilder, keySelector, true);

    public static IOrderedSpecificationBuilder<T> OrderBy<T>(
        this ISpecificationBuilder<T> specificationBuilder,
        Expression<Func<T, object?>> keySelector,
        bool condition)
    {
        if (condition)
        {
            var info = new OrderExpressionInfo<T>(keySelector, OrderTypeEnum.OrderBy);
            specificationBuilder.Specification.Add(info);
        }

        var orderedSpecificationBuilder = new OrderedSpecificationBuilder<T>(specificationBuilder.Specification, !condition);
        return orderedSpecificationBuilder;
    }

    public static IOrderedSpecificationBuilder<T> OrderByDescending<T>(
        this ISpecificationBuilder<T> specificationBuilder,
        Expression<Func<T, object?>> keySelector)
        => OrderByDescending(specificationBuilder, keySelector, true);

    public static IOrderedSpecificationBuilder<T> OrderByDescending<T>(
        this ISpecificationBuilder<T> specificationBuilder,
        Expression<Func<T, object?>> keySelector,
        bool condition)
    {
        if (condition)
        {
            var info = new OrderExpressionInfo<T>(keySelector, OrderTypeEnum.OrderByDescending);
            specificationBuilder.Specification.Add(info);
        }

        var orderedSpecificationBuilder = new OrderedSpecificationBuilder<T>(specificationBuilder.Specification, !condition);
        return orderedSpecificationBuilder;
    }

    public static IIncludableSpecificationBuilder<T, TProperty> Include<T, TProperty>(
        this ISpecificationBuilder<T> specificationBuilder,
        Expression<Func<T, TProperty>> includeExpression) where T : class
        => Include(specificationBuilder, includeExpression, true);

    public static IIncludableSpecificationBuilder<T, TProperty> Include<T, TProperty>(
        this ISpecificationBuilder<T> specificationBuilder,
        Expression<Func<T, TProperty>> includeExpression,
        bool condition) where T : class
    {
        if (condition)
        {
            var info = new IncludeExpressionInfo(includeExpression, typeof(T), typeof(TProperty));
            specificationBuilder.Specification.Add(info);
        }

        var includeBuilder = new IncludableSpecificationBuilder<T, TProperty>(specificationBuilder.Specification, !condition);
        return includeBuilder;
    }

    public static ISpecificationBuilder<T> Include<T>(
        this ISpecificationBuilder<T> specificationBuilder,
        string includeString) where T : class
        => Include(specificationBuilder, includeString, true);

    public static ISpecificationBuilder<T> Include<T>(
        this ISpecificationBuilder<T> specificationBuilder,
        string includeString,
        bool condition) where T : class
    {
        if (condition)
        {
            specificationBuilder.Specification.Add(includeString);
        }

        return specificationBuilder;
    }

    public static ISpecificationBuilder<T> Search<T>(
        this ISpecificationBuilder<T> specificationBuilder,
        Expression<Func<T, string>> selector,
        string searchTerm,
        int searchGroup = 1) where T : class
        => Search(specificationBuilder, selector, searchTerm, true, searchGroup);

    public static ISpecificationBuilder<T> Search<T>(
        this ISpecificationBuilder<T> specificationBuilder,
        Expression<Func<T, string>> selector,
        string searchTerm,
        bool condition,
        int searchGroup = 1) where T : class
    {
        if (condition)
        {
            var info = new SearchExpressionInfo<T>(selector, searchTerm, searchGroup);
            specificationBuilder.Specification.Add(info);
        }

        return specificationBuilder;
    }

    public static ISpecificationBuilder<T> Take<T>(
        this ISpecificationBuilder<T> specificationBuilder,
        int take)
        => Take(specificationBuilder, take, true);

    public static ISpecificationBuilder<T> Take<T>(
        this ISpecificationBuilder<T> specificationBuilder,
        int take,
        bool condition)
    {
        if (condition)
        {
            specificationBuilder.Specification.Take = take;
        }

        return specificationBuilder;
    }

    public static ISpecificationBuilder<T> Skip<T>(
        this ISpecificationBuilder<T> specificationBuilder,
        int skip)
        => Skip(specificationBuilder, skip, true);

    public static ISpecificationBuilder<T> Skip<T>(
        this ISpecificationBuilder<T> specificationBuilder,
        int skip,
        bool condition)
    {
        if (condition)
        {
            specificationBuilder.Specification.Skip = skip;
        }

        return specificationBuilder;
    }

    public static ISpecificationBuilder<T, TResult> Select<T, TResult>(
        this ISpecificationBuilder<T, TResult> specificationBuilder,
        Expression<Func<T, TResult>> selector)
    {
        specificationBuilder.Specification.Selector = selector;
        return specificationBuilder;
    }

    public static ISpecificationBuilder<T, TResult> SelectMany<T, TResult>(
        this ISpecificationBuilder<T, TResult> specificationBuilder,
        Expression<Func<T, IEnumerable<TResult>>> selector)
    {
        specificationBuilder.Specification.SelectorMany = selector;
        return specificationBuilder;
    }

    public static ISpecificationBuilder<T> IgnoreQueryFilters<T>(
    this ISpecificationBuilder<T> specificationBuilder) where T : class
    => IgnoreQueryFilters(specificationBuilder, true);

    public static ISpecificationBuilder<T> IgnoreQueryFilters<T>(
        this ISpecificationBuilder<T> specificationBuilder,
        bool condition) where T : class
    {
        if (condition)
        {
            specificationBuilder.Specification.IgnoreQueryFilters = true;
        }
        return specificationBuilder;
    }

    public static ISpecificationBuilder<T> AsSplitQuery<T>(
    this ISpecificationBuilder<T> specificationBuilder) where T : class
    => AsSplitQuery(specificationBuilder, true);

    public static ISpecificationBuilder<T> AsSplitQuery<T>(
        this ISpecificationBuilder<T> specificationBuilder,
        bool condition) where T : class
    {
        if (condition)
        {
            specificationBuilder.Specification.AsSplitQuery = true;
        }
        return specificationBuilder;
    }

    public static ISpecificationBuilder<T> AsNoTracking<T>(
        this ISpecificationBuilder<T> specificationBuilder) where T : class
        => AsNoTracking(specificationBuilder, true);

    public static ISpecificationBuilder<T> AsNoTracking<T>(
        this ISpecificationBuilder<T> specificationBuilder,
        bool condition) where T : class
    {
        if (condition)
        {
            specificationBuilder.Specification.AsNoTrackingWithIdentityResolution = false;
            specificationBuilder.Specification.AsNoTracking = true;
        }
        return specificationBuilder;
    }

    public static ISpecificationBuilder<T> AsNoTrackingWithIdentityResolution<T>(
        this ISpecificationBuilder<T> specificationBuilder) where T : class
        => AsNoTrackingWithIdentityResolution(specificationBuilder, true);

    public static ISpecificationBuilder<T> AsNoTrackingWithIdentityResolution<T>(
        this ISpecificationBuilder<T> specificationBuilder,
        bool condition) where T : class
    {
        if (condition)
        {
            specificationBuilder.Specification.AsNoTracking = false;
            specificationBuilder.Specification.AsNoTrackingWithIdentityResolution = true;
        }
        return specificationBuilder;
    }
}
