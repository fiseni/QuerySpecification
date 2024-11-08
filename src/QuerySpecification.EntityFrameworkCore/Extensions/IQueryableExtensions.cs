namespace Pozitron.QuerySpecification;

/// <summary>
/// Provides extension methods for <see cref="IQueryable{T}"/> to apply specifications and pagination.
/// </summary>
public static class IQueryableExtensions
{
    /// <summary>
    /// Applies the given specification to the queryable source.
    /// </summary>
    /// <typeparam name="TSource">The type of the entity.</typeparam>
    /// <param name="source">The queryable source.</param>
    /// <param name="specification">The specification to apply.</param>
    /// <param name="evaluator">The specification evaluator to use. If null, the default evaluator is used.</param>
    /// <returns>The queryable source with the specification applied.</returns>
    public static IQueryable<TSource> WithSpecification<TSource>(
      this IQueryable<TSource> source,
      Specification<TSource> specification,
      SpecificationEvaluator? evaluator = null)
      where TSource : class
    {
        evaluator ??= SpecificationEvaluator.Default;
        return evaluator.Evaluate(source, specification);
    }

    /// <summary>
    /// Applies the given specification to the queryable source and projects the result to a different type.
    /// </summary>
    /// <typeparam name="TSource">The type of the entity.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="source">The queryable source.</param>
    /// <param name="specification">The specification to apply.</param>
    /// <param name="evaluator">The specification evaluator to use. If null, the default evaluator is used.</param>
    /// <returns>The queryable source with the specification applied and projected to the result type.</returns>
    public static IQueryable<TResult> WithSpecification<TSource, TResult>(
      this IQueryable<TSource> source,
      Specification<TSource, TResult> specification,
      SpecificationEvaluator? evaluator = null)
      where TSource : class
    {
        evaluator ??= SpecificationEvaluator.Default;
        return evaluator.Evaluate(source, specification);
    }

    /// <summary>
    /// Converts the queryable source to a paged result asynchronously.
    /// </summary>
    /// <typeparam name="TSource">The type of the entity.</typeparam>
    /// <param name="source">The queryable source.</param>
    /// <param name="filter">The paging filter.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the paged result.</returns>
    public static Task<PagedResult<TSource>> ToPagedResultAsync<TSource>(
      this IQueryable<TSource> source,
      PagingFilter filter,
      CancellationToken cancellationToken = default)
      where TSource : class
      => ToPagedResultAsync(source, filter, PaginationSettings.Default, cancellationToken);

    /// <summary>
    /// Converts the queryable source to a paged result asynchronously with the specified pagination settings.
    /// </summary>
    /// <typeparam name="TSource">The type of the entity.</typeparam>
    /// <param name="source">The queryable source.</param>
    /// <param name="filter">The paging filter.</param>
    /// <param name="paginationSettings">The pagination settings.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the paged result.</returns>
    public static async Task<PagedResult<TSource>> ToPagedResultAsync<TSource>(
      this IQueryable<TSource> source,
      PagingFilter filter,
      PaginationSettings paginationSettings,
      CancellationToken cancellationToken = default)
      where TSource : class
    {
        var count = await source.CountAsync(cancellationToken);
        var pagination = new Pagination(paginationSettings, count, filter);

        source = source.ApplyPaging(pagination);

        var data = await source.ToListAsync(cancellationToken);

        return new PagedResult<TSource>(data, pagination);
    }
}
