namespace Pozitron.QuerySpecification;

public static class IQueryableExtensions
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

    public static Task<PagedResult<TSource>> ToPagedResultAsync<TSource>(
      this IQueryable<TSource> source,
      PagingFilter filter,
      CancellationToken cancellationToken = default)
      where TSource : class
        => ToPagedResultAsync(source, filter, PaginationSettings.Default, cancellationToken);

    public static async Task<PagedResult<TSource>> ToPagedResultAsync<TSource>(
      this IQueryable<TSource> source,
      PagingFilter filter,
      PaginationSettings paginationSettings,
      CancellationToken cancellationToken = default)
      where TSource : class
    {
        var count = await source.CountAsync(cancellationToken);
        var pagination = new Pagination(paginationSettings, count, filter);

        var query = source
            .Skip(pagination.Skip)
            .Take(pagination.Take);

        var data = await query.ToListAsync(cancellationToken);

        return new PagedResult<TSource>(data, pagination);
    }
}
