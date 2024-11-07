namespace Pozitron.QuerySpecification;

public static partial class SpecificationBuilderExtensions
{
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
            builder.Specification.AddInternal(ItemType.IncludeString, includeString);
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
            builder.Specification.AddInternal(ItemType.IncludeString, includeString);
        }
        return builder;
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
            builder.Specification.AddInternal(ItemType.Include, includeExpression, (int)IncludeType.Include);
        }

        Specification<T, TResult>.IsChainDiscarded = !condition;
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
            builder.Specification.AddInternal(ItemType.Include, includeExpression, (int)IncludeType.Include);
        }

        Specification<T>.IsChainDiscarded = !condition;
        var includeBuilder = new IncludableSpecificationBuilder<T, TProperty>(builder.Specification);
        return includeBuilder;
    }

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
