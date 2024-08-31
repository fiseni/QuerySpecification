﻿using Microsoft.EntityFrameworkCore;

namespace Pozitron.QuerySpecification.EntityFrameworkCore;

public static class DbSetExtensions
{
    public static async Task<List<TSource>> ToListAsync<TSource>(
      this DbSet<TSource> source,
      Specification<TSource> specification,
      CancellationToken cancellationToken = default)
      where TSource : class
    {
        var result = await SpecificationEvaluator.Default.GetQuery(source, specification).ToListAsync(cancellationToken);

        return specification.PostProcessingAction == null
            ? result
            : specification.PostProcessingAction(result).ToList();
    }

    public static async Task<IEnumerable<TSource>> ToEnumerableAsync<TSource>(
      this DbSet<TSource> source,
      Specification<TSource> specification,
      CancellationToken cancellationToken = default)
      where TSource : class
    {
        var result = await SpecificationEvaluator.Default.GetQuery(source, specification).ToListAsync(cancellationToken);

        return specification.PostProcessingAction == null
            ? result
            : specification.PostProcessingAction(result);
    }

    public static IQueryable<TSource> WithSpecification<TSource>(
      this IQueryable<TSource> source,
      Specification<TSource> specification,
      ISpecificationEvaluator? evaluator = null)
      where TSource : class
    {
        evaluator ??= SpecificationEvaluator.Default;
        return evaluator.GetQuery(source, specification);
    }

    public static IQueryable<TResult> WithSpecification<TSource, TResult>(
      this IQueryable<TSource> source,
      Specification<TSource, TResult> specification,
      ISpecificationEvaluator? evaluator = null)
      where TSource : class
    {
        evaluator ??= SpecificationEvaluator.Default;
        return evaluator.GetQuery(source, specification);
    }
}
