namespace Pozitron.QuerySpecification;

/// <summary>
/// Represents a repository for projecting entities to different result types.
/// </summary>
/// <typeparam name="T">The type of the entity.</typeparam>
public interface IProjectionRepository<T> where T : class
{
    /// <summary>
    /// Projects the first entity that matches the specification to a result. It throws an exception if no entity is found.
    /// It ignores the selector in the specification, and projects the entity to the result type using the Map method.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="specification">The specification to evaluate.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the projected result.</returns>
    /// <exception cref="EntityNotFoundException"></exception>
    Task<TResult> ProjectToFirstAsync<TResult>(Specification<T> specification, CancellationToken cancellationToken = default);

    /// <summary>
    /// Projects the first entity that matches the specification to a result or null if no entity is found.
    /// It ignores the selector in the specification, and projects the entity to the result type using the Map method.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="specification">The specification to evaluate.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the projected result or null if no entity is found.</returns>
    Task<TResult?> ProjectToFirstOrDefaultAsync<TResult>(Specification<T> specification, CancellationToken cancellationToken = default);

    /// <summary>
    /// Projects the entities that match the specification to a list of results.
    /// It ignores the selector in the specification. It projects the entities to the result type using the Map method.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="specification">The specification to evaluate.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the list of projected results.</returns>
    Task<List<TResult>> ProjectToListAsync<TResult>(Specification<T> specification, CancellationToken cancellationToken = default);

    /// <summary>
    /// Projects the entities that match the specification to a paged list of results.
    /// It ignores the selector in the specification, and projects the entities to the result type using the Map method.
    /// It ignores the paging filter in the specification, and applies pagination based on the provided paging filter.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="specification">The specification to evaluate.</param>
    /// <param name="filter">The paging filter.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the paged list of projected results.</returns>
    Task<PagedResult<TResult>> ProjectToListAsync<TResult>(Specification<T> specification, PagingFilter filter, CancellationToken cancellationToken = default);
}
