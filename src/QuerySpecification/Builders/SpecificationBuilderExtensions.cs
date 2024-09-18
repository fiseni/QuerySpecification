using System.Linq.Expressions;

namespace Pozitron.QuerySpecification;

public static class SpecificationBuilderExtensions
{
    public static void Select<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder,
        Expression<Func<T, TResult>> selector)
    {
        builder.Specification.Selector = selector;
    }

    public static void SelectMany<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder,
        Expression<Func<T, IEnumerable<TResult>>> selector)
    {
        builder.Specification.SelectorMany = selector;
    }

    public static ISpecificationBuilder<T, TResult> Where<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder,
        Expression<Func<T, bool>> criteria)
        => Where(builder, criteria, true);

    public static ISpecificationBuilder<T, TResult> Where<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder,
        Expression<Func<T, bool>> criteria,
        bool condition)
    {
        if (condition)
        {
            var expr = new WhereExpression<T>(criteria);
            builder.Specification.Add(expr);
        }
        return builder;
    }

    public static ISpecificationBuilder<T> Where<T>(
        this ISpecificationBuilder<T> builder,
        Expression<Func<T, bool>> criteria)
        => Where(builder, criteria, true);

    public static ISpecificationBuilder<T> Where<T>(
        this ISpecificationBuilder<T> builder,
        Expression<Func<T, bool>> criteria,
        bool condition)
    {
        if (condition)
        {
            var expr = new WhereExpression<T>(criteria);
            builder.Specification.Add(expr);
        }
        return builder;
    }

    public static IOrderedSpecificationBuilder<T, TResult> OrderBy<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder,
        Expression<Func<T, object?>> keySelector)
        => OrderBy(builder, keySelector, true);

    public static IOrderedSpecificationBuilder<T, TResult> OrderBy<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder,
        Expression<Func<T, object?>> keySelector,
        bool condition)
    {
        if (condition)
        {
            var expr = new OrderExpression<T>(keySelector, OrderTypeEnum.OrderBy);
            builder.Specification.Add(expr);
        }

        var orderedSpecificationBuilder = new OrderedSpecificationBuilder<T, TResult>(builder.Specification, !condition);
        return orderedSpecificationBuilder;
    }

    public static IOrderedSpecificationBuilder<T> OrderBy<T>(
        this ISpecificationBuilder<T> builder,
        Expression<Func<T, object?>> keySelector)
        => OrderBy(builder, keySelector, true);

    public static IOrderedSpecificationBuilder<T> OrderBy<T>(
        this ISpecificationBuilder<T> builder,
        Expression<Func<T, object?>> keySelector,
        bool condition)
    {
        if (condition)
        {
            var expr = new OrderExpression<T>(keySelector, OrderTypeEnum.OrderBy);
            builder.Specification.Add(expr);
        }

        var orderedSpecificationBuilder = new OrderedSpecificationBuilder<T>(builder.Specification, !condition);
        return orderedSpecificationBuilder;
    }

    public static IOrderedSpecificationBuilder<T, TResult> OrderByDescending<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder,
        Expression<Func<T, object?>> keySelector)
        => OrderByDescending(builder, keySelector, true);

    public static IOrderedSpecificationBuilder<T, TResult> OrderByDescending<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder,
        Expression<Func<T, object?>> keySelector,
        bool condition)
    {
        if (condition)
        {
            var expr = new OrderExpression<T>(keySelector, OrderTypeEnum.OrderByDescending);
            builder.Specification.Add(expr);
        }

        var orderedSpecificationBuilder = new OrderedSpecificationBuilder<T, TResult>(builder.Specification, !condition);
        return orderedSpecificationBuilder;
    }

    public static IOrderedSpecificationBuilder<T> OrderByDescending<T>(
        this ISpecificationBuilder<T> builder,
        Expression<Func<T, object?>> keySelector)
        => OrderByDescending(builder, keySelector, true);

    public static IOrderedSpecificationBuilder<T> OrderByDescending<T>(
        this ISpecificationBuilder<T> builder,
        Expression<Func<T, object?>> keySelector,
        bool condition)
    {
        if (condition)
        {
            var expr = new OrderExpression<T>(keySelector, OrderTypeEnum.OrderByDescending);
            builder.Specification.Add(expr);
        }

        var orderedSpecificationBuilder = new OrderedSpecificationBuilder<T>(builder.Specification, !condition);
        return orderedSpecificationBuilder;
    }

    public static IIncludableSpecificationBuilder<T, TResult, TProperty> Include<T, TResult, TProperty>(
        this ISpecificationBuilder<T, TResult> builder,
        Expression<Func<T, TProperty>> includeExpression) where T : class
        => Include(builder, includeExpression, true);

    public static IIncludableSpecificationBuilder<T, TResult, TProperty> Include<T, TResult, TProperty>(
        this ISpecificationBuilder<T, TResult> builder,
        Expression<Func<T, TProperty>> includeExpression,
        bool condition) where T : class
    {
        if (condition)
        {
            var expr = new IncludeExpression(includeExpression, typeof(T), typeof(TProperty));
            builder.Specification.Add(expr);
        }

        var includeBuilder = new IncludableSpecificationBuilder<T, TResult, TProperty>(builder.Specification, !condition);
        return includeBuilder;
    }

    public static IIncludableSpecificationBuilder<T, TProperty> Include<T, TProperty>(
        this ISpecificationBuilder<T> builder,
        Expression<Func<T, TProperty>> includeExpression) where T : class
        => Include(builder, includeExpression, true);

    public static IIncludableSpecificationBuilder<T, TProperty> Include<T, TProperty>(
        this ISpecificationBuilder<T> builder,
        Expression<Func<T, TProperty>> includeExpression,
        bool condition) where T : class
    {
        if (condition)
        {
            var expr = new IncludeExpression(includeExpression, typeof(T), typeof(TProperty));
            builder.Specification.Add(expr);
        }

        var includeBuilder = new IncludableSpecificationBuilder<T, TProperty>(builder.Specification, !condition);
        return includeBuilder;
    }

    public static ISpecificationBuilder<T, TResult> Include<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder,
        string includeString) where T : class
        => Include(builder, includeString, true);

    public static ISpecificationBuilder<T, TResult> Include<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder,
        string includeString,
        bool condition) where T : class
    {
        if (condition)
        {
            builder.Specification.Add(includeString);
        }
        return builder;
    }
    public static ISpecificationBuilder<T> Include<T>(
        this ISpecificationBuilder<T> builder,
        string includeString) where T : class
        => Include(builder, includeString, true);

    public static ISpecificationBuilder<T> Include<T>(
        this ISpecificationBuilder<T> builder,
        string includeString,
        bool condition) where T : class
    {
        if (condition)
        {
            builder.Specification.Add(includeString);
        }
        return builder;
    }

    public static ISpecificationBuilder<T, TResult> Like<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder,
        Expression<Func<T, string?>> keySelector,
        string pattern,
        int group = 1) where T : class
        => Like(builder, keySelector, pattern, true, group);

    public static ISpecificationBuilder<T, TResult> Like<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder,
        Expression<Func<T, string?>> keySelector,
        string pattern,
        bool condition,
        int group = 1) where T : class
    {
        if (condition)
        {
            var expr = new LikeExpression<T>(keySelector, pattern, group);
            builder.Specification.Add(expr);
        }
        return builder;
    }

    public static ISpecificationBuilder<T> Like<T>(
        this ISpecificationBuilder<T> builder,
        Expression<Func<T, string?>> keySelector,
        string pattern,
        int group = 1) where T : class
        => Like(builder, keySelector, pattern, true, group);

    public static ISpecificationBuilder<T> Like<T>(
        this ISpecificationBuilder<T> builder,
        Expression<Func<T, string?>> keySelector,
        string pattern,
        bool condition,
        int group = 1) where T : class
    {
        if (condition)
        {
            var expr = new LikeExpression<T>(keySelector, pattern, group);
            builder.Specification.Add(expr);
        }
        return builder;
    }

    public static ISpecificationBuilder<T, TResult> Take<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder,
        int take)
        => Take(builder, take, true);

    public static ISpecificationBuilder<T, TResult> Take<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder,
        int take,
        bool condition)
    {
        if (condition)
        {
            builder.Specification.Take = take;
        }
        return builder;
    }

    public static ISpecificationBuilder<T> Take<T>(
        this ISpecificationBuilder<T> builder,
        int take)
        => Take(builder, take, true);

    public static ISpecificationBuilder<T> Take<T>(
        this ISpecificationBuilder<T> builder,
        int take,
        bool condition)
    {
        if (condition)
        {
            builder.Specification.Take = take;
        }
        return builder;
    }

    public static ISpecificationBuilder<T, TResult> Skip<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder,
        int skip)
        => Skip(builder, skip, true);

    public static ISpecificationBuilder<T, TResult> Skip<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder,
        int skip,
        bool condition)
    {
        if (condition)
        {
            builder.Specification.Skip = skip;
        }
        return builder;
    }

    public static ISpecificationBuilder<T> Skip<T>(
        this ISpecificationBuilder<T> builder,
        int skip)
        => Skip(builder, skip, true);

    public static ISpecificationBuilder<T> Skip<T>(
        this ISpecificationBuilder<T> builder,
        int skip,
        bool condition)
    {
        if (condition)
        {
            builder.Specification.Skip = skip;
        }
        return builder;
    }

    public static ISpecificationBuilder<T, TResult> IgnoreQueryFilters<T, TResult>(
    this ISpecificationBuilder<T, TResult> builder) where T : class
        => IgnoreQueryFilters(builder, true);

    public static ISpecificationBuilder<T, TResult> IgnoreQueryFilters<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder,
        bool condition) where T : class
    {
        if (condition)
        {
            builder.Specification.IgnoreQueryFilters = true;
        }
        return builder;
    }

    public static ISpecificationBuilder<T> IgnoreQueryFilters<T>(
    this ISpecificationBuilder<T> builder) where T : class
    => IgnoreQueryFilters(builder, true);

    public static ISpecificationBuilder<T> IgnoreQueryFilters<T>(
        this ISpecificationBuilder<T> builder,
        bool condition) where T : class
    {
        if (condition)
        {
            builder.Specification.IgnoreQueryFilters = true;
        }
        return builder;
    }

    public static ISpecificationBuilder<T, TResult> AsSplitQuery<T, TResult>(
    this ISpecificationBuilder<T, TResult> builder) where T : class
        => AsSplitQuery(builder, true);

    public static ISpecificationBuilder<T, TResult> AsSplitQuery<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder,
        bool condition) where T : class
    {
        if (condition)
        {
            builder.Specification.AsSplitQuery = true;
        }
        return builder;
    }

    public static ISpecificationBuilder<T> AsSplitQuery<T>(
    this ISpecificationBuilder<T> builder) where T : class
    => AsSplitQuery(builder, true);

    public static ISpecificationBuilder<T> AsSplitQuery<T>(
        this ISpecificationBuilder<T> builder,
        bool condition) where T : class
    {
        if (condition)
        {
            builder.Specification.AsSplitQuery = true;
        }
        return builder;
    }

    public static ISpecificationBuilder<T, TResult> AsNoTracking<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder) where T : class
        => AsNoTracking(builder, true);

    public static ISpecificationBuilder<T, TResult> AsNoTracking<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder,
        bool condition) where T : class
    {
        if (condition)
        {
            builder.Specification.AsNoTrackingWithIdentityResolution = false;
            builder.Specification.AsNoTracking = true;
        }
        return builder;
    }

    public static ISpecificationBuilder<T> AsNoTracking<T>(
        this ISpecificationBuilder<T> builder) where T : class
        => AsNoTracking(builder, true);

    public static ISpecificationBuilder<T> AsNoTracking<T>(
        this ISpecificationBuilder<T> builder,
        bool condition) where T : class
    {
        if (condition)
        {
            builder.Specification.AsNoTrackingWithIdentityResolution = false;
            builder.Specification.AsNoTracking = true;
        }
        return builder;
    }

    public static ISpecificationBuilder<T, TResult> AsNoTrackingWithIdentityResolution<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder) where T : class
        => AsNoTrackingWithIdentityResolution(builder, true);

    public static ISpecificationBuilder<T, TResult> AsNoTrackingWithIdentityResolution<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder,
        bool condition) where T : class
    {
        if (condition)
        {
            builder.Specification.AsNoTracking = false;
            builder.Specification.AsNoTrackingWithIdentityResolution = true;
        }
        return builder;
    }

    public static ISpecificationBuilder<T> AsNoTrackingWithIdentityResolution<T>(
        this ISpecificationBuilder<T> builder) where T : class
        => AsNoTrackingWithIdentityResolution(builder, true);

    public static ISpecificationBuilder<T> AsNoTrackingWithIdentityResolution<T>(
        this ISpecificationBuilder<T> builder,
        bool condition) where T : class
    {
        if (condition)
        {
            builder.Specification.AsNoTracking = false;
            builder.Specification.AsNoTrackingWithIdentityResolution = true;
        }
        return builder;
    }
}
