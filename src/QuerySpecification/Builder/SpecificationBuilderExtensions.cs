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
            ((List<WhereExpressionInfo<T>>)specificationBuilder.Context.WhereExpressions).Add(new WhereExpressionInfo<T>(criteria));
        }

        return specificationBuilder;
    }

    public static IOrderedSpecificationBuilder<T> OrderBy<T>(
        this ISpecificationBuilder<T> specificationBuilder,
        Expression<Func<T, object?>> orderExpression)
        => OrderBy(specificationBuilder, orderExpression, true);

    public static IOrderedSpecificationBuilder<T> OrderBy<T>(
        this ISpecificationBuilder<T> specificationBuilder,
        Expression<Func<T, object?>> orderExpression,
        bool condition)
    {
        if (condition)
        {
            ((List<OrderExpressionInfo<T>>)specificationBuilder.Context.OrderExpressions).Add(new OrderExpressionInfo<T>(orderExpression, OrderTypeEnum.OrderBy));
        }

        var orderedSpecificationBuilder = new OrderedSpecificationBuilder<T>(specificationBuilder.Context, !condition);

        return orderedSpecificationBuilder;
    }

    public static IOrderedSpecificationBuilder<T> OrderByDescending<T>(
        this ISpecificationBuilder<T> specificationBuilder,
        Expression<Func<T, object?>> orderExpression)
        => OrderByDescending(specificationBuilder, orderExpression, true);

    public static IOrderedSpecificationBuilder<T> OrderByDescending<T>(
        this ISpecificationBuilder<T> specificationBuilder,
        Expression<Func<T, object?>> orderExpression,
        bool condition)
    {
        if (condition)
        {
            ((List<OrderExpressionInfo<T>>)specificationBuilder.Context.OrderExpressions).Add(new OrderExpressionInfo<T>(orderExpression, OrderTypeEnum.OrderByDescending));
        }

        var orderedSpecificationBuilder = new OrderedSpecificationBuilder<T>(specificationBuilder.Context, !condition);

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

            ((List<IncludeExpressionInfo>)specificationBuilder.Context.IncludeExpressions).Add(info);
        }

        var includeBuilder = new IncludableSpecificationBuilder<T, TProperty>(specificationBuilder.Context, !condition);

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
            ((List<string>)specificationBuilder.Context.IncludeStrings).Add(includeString);
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
            ((List<SearchExpressionInfo<T>>)specificationBuilder.Context.SearchCriterias).Add(new SearchExpressionInfo<T>(selector, searchTerm, searchGroup));
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
            if (specificationBuilder.Context.Take != null) throw new DuplicateTakeException();

            specificationBuilder.Context.Take = take;
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
            if (specificationBuilder.Context.Skip != null) throw new DuplicateSkipException();

            specificationBuilder.Context.Skip = skip;
        }

        return specificationBuilder;
    }

    public static ISpecificationBuilder<T, TResult> Select<T, TResult>(
        this ISpecificationBuilder<T, TResult> specificationBuilder,
        Expression<Func<T, TResult>> selector)
    {
        specificationBuilder.Context.Selector = selector;

        return specificationBuilder;
    }

    public static ISpecificationBuilder<T, TResult> SelectMany<T, TResult>(
        this ISpecificationBuilder<T, TResult> specificationBuilder,
        Expression<Func<T, IEnumerable<TResult>>> selector)
    {
        specificationBuilder.Context.SelectorMany = selector;

        return specificationBuilder;
    }

    public static ISpecificationBuilder<T> PostProcessingAction<T>(
        this ISpecificationBuilder<T> specificationBuilder,
        Func<IEnumerable<T>, IEnumerable<T>> predicate)
    {
        specificationBuilder.Context.PostProcessingAction = predicate;

        return specificationBuilder;
    }

    public static ISpecificationBuilder<T, TResult> PostProcessingAction<T, TResult>(
        this ISpecificationBuilder<T, TResult> specificationBuilder,
        Func<IEnumerable<TResult>, IEnumerable<TResult>> predicate)
    {
        specificationBuilder.Context.PostProcessingAction = predicate;

        return specificationBuilder;
    }

    public static ISpecificationBuilder<T> AsTracking<T>(
        this ISpecificationBuilder<T> specificationBuilder) where T : class
        => AsTracking(specificationBuilder, true);

    public static ISpecificationBuilder<T> AsTracking<T>(
        this ISpecificationBuilder<T> specificationBuilder,
        bool condition) where T : class
    {
        if (condition)
        {
            specificationBuilder.Context.AsNoTracking = false;
            specificationBuilder.Context.AsNoTrackingWithIdentityResolution = false;
            specificationBuilder.Context.AsTracking = true;
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
            specificationBuilder.Context.AsTracking = false;
            specificationBuilder.Context.AsNoTrackingWithIdentityResolution = false;
            specificationBuilder.Context.AsNoTracking = true;
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
            specificationBuilder.Context.AsSplitQuery = true;
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
            specificationBuilder.Context.AsTracking = false;
            specificationBuilder.Context.AsNoTracking = false;
            specificationBuilder.Context.AsNoTrackingWithIdentityResolution = true;
        }

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
            specificationBuilder.Context.IgnoreQueryFilters = true;
        }

        return specificationBuilder;
    }
}
