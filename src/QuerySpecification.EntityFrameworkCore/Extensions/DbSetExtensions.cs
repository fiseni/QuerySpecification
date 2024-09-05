﻿using Microsoft.EntityFrameworkCore;

namespace Pozitron.QuerySpecification.EntityFrameworkCore;

public static class DbSetExtensions
{
    public static IQueryable<TSource> WithSpecification<TSource>(
      this IQueryable<TSource> source,
      Specification<TSource> specification,
      SpecificationEvaluator? evaluator = null)
      where TSource : class
    {
        evaluator ??= SpecificationEvaluator.Default;
        return evaluator.GetQuery(source, specification);
    }

    public static IQueryable<TResult> WithSpecification<TSource, TResult>(
      this IQueryable<TSource> source,
      Specification<TSource, TResult> specification,
      SpecificationEvaluator? evaluator = null)
      where TSource : class
    {
        evaluator ??= SpecificationEvaluator.Default;
        return evaluator.GetQuery(source, specification);
    }
}