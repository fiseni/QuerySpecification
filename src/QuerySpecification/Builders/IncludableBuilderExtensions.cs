namespace Pozitron.QuerySpecification;

public static class IncludableBuilderExtensions
{
    public static IIncludableSpecificationBuilder<TEntity, TResult, TProperty> ThenInclude<TEntity, TResult, TPreviousProperty, TProperty>(
        this IIncludableSpecificationBuilder<TEntity, TResult, TPreviousProperty> previousBuilder,
        Expression<Func<TPreviousProperty, TProperty>> thenIncludeExpression)
        where TEntity : class
        => ThenInclude(previousBuilder, thenIncludeExpression, true);

    public static IIncludableSpecificationBuilder<TEntity, TResult, TProperty> ThenInclude<TEntity, TResult, TPreviousProperty, TProperty>(
        this IIncludableSpecificationBuilder<TEntity, TResult, TPreviousProperty> previousBuilder,
        Expression<Func<TPreviousProperty, TProperty>> thenIncludeExpression,
        bool condition)
        where TEntity : class
    {
        if (condition && !Specification<TEntity, TResult>._isChainDiscarded)
        {
            var state = new SpecState
            {
                Type = StateType.Include,
                Bag = (int)IncludeTypeEnum.ThenInclude,
                Reference = thenIncludeExpression
            };
            previousBuilder.Specification.Add(state);
        }
        else
        {
            Specification<TEntity, TResult>._isChainDiscarded = true;
        }

        var includeBuilder = new IncludableSpecificationBuilder<TEntity, TResult, TProperty>(previousBuilder.Specification);
        return includeBuilder;
    }

    public static IIncludableSpecificationBuilder<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>(
        this IIncludableSpecificationBuilder<TEntity, TPreviousProperty> previousBuilder,
        Expression<Func<TPreviousProperty, TProperty>> thenIncludeExpression)
        where TEntity : class
        => ThenInclude(previousBuilder, thenIncludeExpression, true);

    public static IIncludableSpecificationBuilder<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>(
        this IIncludableSpecificationBuilder<TEntity, TPreviousProperty> previousBuilder,
        Expression<Func<TPreviousProperty, TProperty>> thenIncludeExpression,
        bool condition)
        where TEntity : class
    {
        if (condition && !Specification<TEntity>._isChainDiscarded)
        {
            var state = new SpecState
            {
                Type = StateType.Include,
                Bag = (int)IncludeTypeEnum.ThenInclude,
                Reference = thenIncludeExpression
            };
            previousBuilder.Specification.Add(state);
        }
        else
        {
            Specification<TEntity>._isChainDiscarded = true;
        }

        var includeBuilder = new IncludableSpecificationBuilder<TEntity, TProperty>(previousBuilder.Specification);
        return includeBuilder;
    }

    public static IIncludableSpecificationBuilder<TEntity, TResult, TProperty> ThenInclude<TEntity, TResult, TPreviousProperty, TProperty>(
        this IIncludableSpecificationBuilder<TEntity, TResult, IEnumerable<TPreviousProperty>> previousBuilder,
        Expression<Func<TPreviousProperty, TProperty>> thenIncludeExpression)
        where TEntity : class
        => ThenInclude(previousBuilder, thenIncludeExpression, true);

    public static IIncludableSpecificationBuilder<TEntity, TResult, TProperty> ThenInclude<TEntity, TResult, TPreviousProperty, TProperty>(
        this IIncludableSpecificationBuilder<TEntity, TResult, IEnumerable<TPreviousProperty>> previousBuilder,
        Expression<Func<TPreviousProperty, TProperty>> thenIncludeExpression,
        bool condition)
        where TEntity : class
    {
        if (condition && !Specification<TEntity, TResult>._isChainDiscarded)
        {
            var state = new SpecState
            {
                Type = StateType.Include,
                Bag = (int)IncludeTypeEnum.ThenInclude,
                Reference = thenIncludeExpression
            };
            previousBuilder.Specification.Add(state);
        }
        else
        {
            Specification<TEntity, TResult>._isChainDiscarded = true;
        }

        var includeBuilder = new IncludableSpecificationBuilder<TEntity, TResult, TProperty>(previousBuilder.Specification);
        return includeBuilder;
    }

    public static IIncludableSpecificationBuilder<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>(
        this IIncludableSpecificationBuilder<TEntity, IEnumerable<TPreviousProperty>> previousBuilder,
        Expression<Func<TPreviousProperty, TProperty>> thenIncludeExpression)
        where TEntity : class
        => ThenInclude(previousBuilder, thenIncludeExpression, true);

    public static IIncludableSpecificationBuilder<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>(
        this IIncludableSpecificationBuilder<TEntity, IEnumerable<TPreviousProperty>> previousBuilder,
        Expression<Func<TPreviousProperty, TProperty>> thenIncludeExpression,
        bool condition)
        where TEntity : class
    {
        if (condition && !Specification<TEntity>._isChainDiscarded)
        {
            var state = new SpecState
            {
                Type = StateType.Include,
                Bag = (int)IncludeTypeEnum.ThenInclude,
                Reference = thenIncludeExpression
            };
            previousBuilder.Specification.Add(state);
        }
        else
        {
            Specification<TEntity>._isChainDiscarded = true;
        }

        var includeBuilder = new IncludableSpecificationBuilder<TEntity, TProperty>(previousBuilder.Specification);
        return includeBuilder;
    }
}
