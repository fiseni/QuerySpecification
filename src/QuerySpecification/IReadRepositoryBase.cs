namespace Pozitron.QuerySpecification;

/// <summary>
/// Represents a read-only repository for accessing entities.
/// </summary>
/// <typeparam name="T">The type of the entity.</typeparam>
public interface IReadRepositoryBase<T> where T : class
{
    /// <summary>
    /// Finds an entity by its identifier.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier.</typeparam>
    /// <param name="id">The identifier of the entity.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the entity if found; otherwise, null.</returns>
    ValueTask<T?> FindAsync<TId>(TId id, CancellationToken cancellationToken = default) where TId : notnull;

    /// <summary>
    /// Gets the first entity that matches the specification. It throws an exception if no entity is found.
    /// </summary>
    /// <param name="specification">The specification to evaluate.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the first entity that matches the specification.</returns>
    /// <exception cref="EntityNotFoundException"></exception>
    Task<T> FirstAsync(Specification<T> specification, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the first entity that matches the specification and projects it to a result. It throws an exception if no entity is found.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="specification">The specification to evaluate.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the first entity that matches the specification and is projected to a result.</returns>
    /// <exception cref="EntityNotFoundException"></exception>
    Task<TResult> FirstAsync<TResult>(Specification<T, TResult> specification, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the first entity that matches the specification or null if no entity is found.
    /// </summary>
    /// <param name="specification">The specification to evaluate.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the first entity that matches the specification or null if no entity is found.</returns>
    Task<T?> FirstOrDefaultAsync(Specification<T> specification, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the first entity that matches the specification and projects it to a result or null if no entity is found.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="specification">The specification to evaluate.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the first entity that matches the specification and is projected to a result or null if no entity is found.</returns>
    Task<TResult?> FirstOrDefaultAsync<TResult>(Specification<T, TResult> specification, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the single entity that matches the specification or null if no entity is found. It throws an exception if there is not exactly one element in the sequence.
    /// </summary>
    /// <param name="specification">The specification to evaluate.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the single entity that matches the specification or null if no entity is found.</returns>
    Task<T?> SingleOrDefaultAsync(Specification<T> specification, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the single entity that matches the specification and projects it to a result or null if no entity is found. It throws an exception if there is not exactly one element in the sequence.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="specification">The specification to evaluate.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the single entity that matches the specification and is projected to a result or null if no entity is found.</returns>
    Task<TResult?> SingleOrDefaultAsync<TResult>(Specification<T, TResult> specification, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a list of all entities.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the list of all entities.</returns>
    Task<List<T>> ListAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a list of entities that match the specification.
    /// </summary>
    /// <param name="specification">The specification to evaluate.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the list of entities that match the specification.</returns>
    Task<List<T>> ListAsync(Specification<T> specification, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a list of entities that match the specification and projects them to a result.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="specification">The specification to evaluate.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the list of entities that match the specification and are projected to a result.</returns>
    Task<List<TResult>> ListAsync<TResult>(Specification<T, TResult> specification, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the count of all entities.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the count of all entities.</returns>
    Task<int> CountAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the count of entities that match the specification.
    /// It ignores the skip and take values of the specification.
    /// </summary>
    /// <param name="specification">The specification to evaluate.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the count of entities that match the specification.</returns>
    Task<int> CountAsync(Specification<T> specification, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the count of entities that match the specification and projects them to a result.
    /// It ignores the skip and take values of the specification.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="specification">The specification to evaluate.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the count of entities that match the specification and are projected to a result.</returns>
    Task<int> CountAsync<TResult>(Specification<T, TResult> specification, CancellationToken cancellationToken = default);

    /// <summary>
    /// Determines whether any entities exist.
    /// It ignores the skip and take values of the specification.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains true if any entities exist; otherwise, false.</returns>
    Task<bool> AnyAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Determines whether any entities match the specification.
    /// It ignores the skip and take values of the specification.
    /// </summary>
    /// <param name="specification">The specification to evaluate.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains true if any entities match the specification; otherwise, false.</returns>
    Task<bool> AnyAsync(Specification<T> specification, CancellationToken cancellationToken = default);

    /// <summary>
    /// Determines whether any entities match the specification and projects them to a result.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="specification">The specification to evaluate.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains true if any entities match the specification and are projected to a result; otherwise, false.</returns>
    Task<bool> AnyAsync<TResult>(Specification<T, TResult> specification, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns an asynchronous enumerable of entities that match the specification.
    /// </summary>
    /// <param name="specification">The specification to evaluate.</param>
    /// <returns>An asynchronous enumerable of entities that match the specification.</returns>
    IAsyncEnumerable<T> AsAsyncEnumerable(Specification<T> specification);
}
