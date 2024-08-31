namespace Pozitron.QuerySpecification;

public interface IReadRepositoryBase<T> where T : class
{
    Task<T?> GetByIdAsync<TId>(TId id, CancellationToken cancellationToken = default) where TId : notnull;

    [Obsolete("Use FirstOrDefaultAsync<T> or SingleOrDefaultAsync<T> instead. The SingleOrDefaultAsync<T> can be applied only to SingleResultSpecification<T> specifications.")]
    Task<T?> GetBySpecAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);

    [Obsolete("Use FirstOrDefaultAsync<T> or SingleOrDefaultAsync<T> instead. The SingleOrDefaultAsync<T> can be applied only to SingleResultSpecification<T> specifications.")]
    Task<TResult?> GetBySpecAsync<TResult>(ISpecification<T, TResult> specification, CancellationToken cancellationToken = default);

    Task<T?> FirstOrDefaultAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);

    Task<TResult?> FirstOrDefaultAsync<TResult>(ISpecification<T, TResult> specification, CancellationToken cancellationToken = default);

    Task<T?> SingleOrDefaultAsync(ISingleResultSpecification<T> specification, CancellationToken cancellationToken = default);

    Task<TResult?> SingleOrDefaultAsync<TResult>(ISingleResultSpecification<T, TResult> specification, CancellationToken cancellationToken = default);

    Task<List<T>> ListAsync(CancellationToken cancellationToken = default);

    Task<List<T>> ListAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);

    Task<List<TResult>> ListAsync<TResult>(ISpecification<T, TResult> specification, CancellationToken cancellationToken = default);

    Task<int> CountAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);

    Task<int> CountAsync(CancellationToken cancellationToken = default);

    Task<bool> AnyAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);

    Task<bool> AnyAsync(CancellationToken cancellationToken = default);


#if NET6_0_OR_GREATER
    IAsyncEnumerable<T> AsAsyncEnumerable(ISpecification<T> specification);
#endif
}
