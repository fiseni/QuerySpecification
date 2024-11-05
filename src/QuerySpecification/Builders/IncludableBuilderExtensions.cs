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
        if (condition && !Specification<TEntity, TResult>.IsChainDiscarded)
        {
            previousBuilder.Specification.AddInternal(ItemType.Include, thenIncludeExpression, (int)IncludeType.ThenInclude);
        }
        else
        {
            Specification<TEntity, TResult>.IsChainDiscarded = true;
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
        if (condition && !Specification<TEntity>.IsChainDiscarded)
        {
            previousBuilder.Specification.AddInternal(ItemType.Include, thenIncludeExpression, (int)IncludeType.ThenInclude);
        }
        else
        {
            Specification<TEntity>.IsChainDiscarded = true;
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
        if (condition && !Specification<TEntity, TResult>.IsChainDiscarded)
        {
            previousBuilder.Specification.AddInternal(ItemType.Include, thenIncludeExpression, (int)IncludeType.ThenInclude);
        }
        else
        {
            Specification<TEntity, TResult>.IsChainDiscarded = true;
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
        if (condition && !Specification<TEntity>.IsChainDiscarded)
        {
            previousBuilder.Specification.AddInternal(ItemType.Include, thenIncludeExpression, (int)IncludeType.ThenInclude);
        }
        else
        {
            Specification<TEntity>.IsChainDiscarded = true;
        }

        var includeBuilder = new IncludableSpecificationBuilder<TEntity, TProperty>(previousBuilder.Specification);
        return includeBuilder;
    }
}
