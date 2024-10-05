namespace Pozitron.QuerySpecification;

public interface IProjectionRepository<T> where T : class
{
    Task<TResult> ProjectToFirstAsync<TResult>(Specification<T> specification, CancellationToken cancellationToken = default);
    Task<TResult?> ProjectToFirstOrDefaultAsync<TResult>(Specification<T> specification, CancellationToken cancellationToken = default);
    Task<List<TResult>> ProjectToListAsync<TResult>(Specification<T> specification, CancellationToken cancellationToken = default);
    Task<PagedResult<TResult>> ProjectToListAsync<TResult>(Specification<T> specification, PagingFilter filter, CancellationToken cancellationToken = default);
}
