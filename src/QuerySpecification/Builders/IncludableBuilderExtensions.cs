﻿namespace Pozitron.QuerySpecification;

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
        if (condition && !previousBuilder.IsChainDiscarded)
        {
            var expr = new IncludeExpression(thenIncludeExpression, typeof(TEntity), typeof(TProperty), typeof(TPreviousProperty));
            previousBuilder.Specification.Add(expr);
        }

        var includeBuilder = new IncludableSpecificationBuilder<TEntity, TResult, TProperty>(previousBuilder.Specification, !condition || previousBuilder.IsChainDiscarded);
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
        if (condition && !previousBuilder.IsChainDiscarded)
        {
            var expr = new IncludeExpression(thenIncludeExpression, typeof(TEntity), typeof(TProperty), typeof(TPreviousProperty));
            previousBuilder.Specification.Add(expr);
        }

        var includeBuilder = new IncludableSpecificationBuilder<TEntity, TProperty>(previousBuilder.Specification, !condition || previousBuilder.IsChainDiscarded);
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
        if (condition && !previousBuilder.IsChainDiscarded)
        {
            var expr = new IncludeExpression(thenIncludeExpression, typeof(TEntity), typeof(TProperty), typeof(IEnumerable<TPreviousProperty>));
            previousBuilder.Specification.Add(expr);
        }

        var includeBuilder = new IncludableSpecificationBuilder<TEntity, TResult, TProperty>(previousBuilder.Specification, !condition || previousBuilder.IsChainDiscarded);
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
        if (condition && !previousBuilder.IsChainDiscarded)
        {
            var expr = new IncludeExpression(thenIncludeExpression, typeof(TEntity), typeof(TProperty), typeof(IEnumerable<TPreviousProperty>));
            previousBuilder.Specification.Add(expr);
        }

        var includeBuilder = new IncludableSpecificationBuilder<TEntity, TProperty>(previousBuilder.Specification, !condition || previousBuilder.IsChainDiscarded);
        return includeBuilder;
    }
}
