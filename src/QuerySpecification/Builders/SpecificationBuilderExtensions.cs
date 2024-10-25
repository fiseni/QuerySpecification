namespace Pozitron.QuerySpecification;

public static class SpecificationBuilderExtensions
{
    public static void Select<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder,
        Expression<Func<T, TResult>> selector)
    {
        builder.Specification.Add(selector);
    }

    public static void SelectMany<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder,
        Expression<Func<T, IEnumerable<TResult>>> selector)
    {
        builder.Specification.Add(selector);
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
            var state = new SpecState
            {
                Type = StateType.Where,
                Reference = criteria
            };
            builder.Specification.Add(state);
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
            var state = new SpecState
            {
                Type = StateType.Where,
                Reference = criteria
            };
            builder.Specification.Add(state);
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
            var state = new SpecState
            {
                Type = StateType.Order,
                Bag = (int)OrderTypeEnum.OrderBy,
                Reference = keySelector
            };
            builder.Specification.Add(state);
        }

        Specification<T,TResult>._isChainDiscarded = !condition;
        return (SpecificationBuilder<T, TResult>)builder;
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
            var state = new SpecState
            {
                Type = StateType.Order,
                Bag = (int)OrderTypeEnum.OrderBy,
                Reference = keySelector
            };
            builder.Specification.Add(state);
        }

        Specification<T>._isChainDiscarded = !condition;
        return (SpecificationBuilder<T>)builder;
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
            var state = new SpecState
            {
                Type = StateType.Order,
                Bag = (int)OrderTypeEnum.OrderByDescending,
                Reference = keySelector
            };
            builder.Specification.Add(state);
        }

        Specification<T,TResult>._isChainDiscarded = !condition;
        return (SpecificationBuilder<T, TResult>)builder;
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
            var state = new SpecState
            {
                Type = StateType.Order,
                Bag = (int)OrderTypeEnum.OrderByDescending,
                Reference = keySelector
            };
            builder.Specification.Add(state);
        }

        Specification<T>._isChainDiscarded = !condition;
        return (SpecificationBuilder<T>)builder;
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
            var state = new SpecState
            {
                Type = StateType.Include,
                Bag = (int)IncludeTypeEnum.Include,
                Reference = includeExpression
            };
            builder.Specification.Add(state);
        }

        Specification<T>._isChainDiscarded = !condition;
        var includeBuilder = new IncludableSpecificationBuilder<T, TResult, TProperty>(builder.Specification);
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
            var state = new SpecState
            {
                Type = StateType.Include,
                Bag = (int)IncludeTypeEnum.Include,
                Reference = includeExpression
            };
            builder.Specification.Add(state);
        }

        Specification<T>._isChainDiscarded = !condition;
        var includeBuilder = new IncludableSpecificationBuilder<T, TProperty>(builder.Specification);
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
            var state = new SpecState
            {
                Type = StateType.IncludeString,
                Reference = includeString
            };
            builder.Specification.Add(state);
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
            var state = new SpecState
            {
                Type = StateType.IncludeString,
                Reference = includeString
            };
            builder.Specification.Add(state);
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
            var state = new SpecState
            {
                Type = StateType.Like,
                Reference = expr
            };
            builder.Specification.Add(state);
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
            var state = new SpecState
            {
                Type = StateType.Like,
                Reference = expr
            };
            builder.Specification.Add(state);
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
            builder.Specification.SetEfFlag(EfFlag.IgnoreQueryFilters);
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
            builder.Specification.SetEfFlag(EfFlag.IgnoreQueryFilters);
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
            builder.Specification.SetEfFlag(EfFlag.AsSplitQuery);
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
            builder.Specification.SetEfFlag(EfFlag.AsSplitQuery);
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
            builder.Specification.RemoveEfFlag(EfFlag.AsNoTrackingWithIdentityResolution);
            builder.Specification.SetEfFlag(EfFlag.AsNoTracking);
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
            builder.Specification.RemoveEfFlag(EfFlag.AsNoTrackingWithIdentityResolution);
            builder.Specification.SetEfFlag(EfFlag.AsNoTracking);
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
            builder.Specification.RemoveEfFlag(EfFlag.AsNoTracking);
            builder.Specification.SetEfFlag(EfFlag.AsNoTrackingWithIdentityResolution);
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
            builder.Specification.RemoveEfFlag(EfFlag.AsNoTracking);
            builder.Specification.SetEfFlag(EfFlag.AsNoTrackingWithIdentityResolution);
        }
        return builder;
    }
}
