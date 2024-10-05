namespace Pozitron.QuerySpecification;

public abstract class RepositoryWithMapper<T> : RepositoryBase<T>, IProjectionRepository<T> where T : class
{
    private readonly PaginationSettings _paginationSettings = PaginationSettings.Default;

    protected RepositoryWithMapper(DbContext dbContext)
        : base(dbContext)
    {
    }
    protected RepositoryWithMapper(DbContext dbContext, SpecificationEvaluator specificationEvaluator)
        : base(dbContext, specificationEvaluator)
    {
    }
    protected RepositoryWithMapper(DbContext dbContext, PaginationSettings paginationSettings)
        : base(dbContext)
    {
        _paginationSettings = paginationSettings;
    }
    protected RepositoryWithMapper(DbContext dbContext, SpecificationEvaluator specificationEvaluator, PaginationSettings paginationSettings)
        : base(dbContext, specificationEvaluator)
    {
        _paginationSettings = paginationSettings;
    }

    protected abstract IQueryable<TResult> Map<TResult>(IQueryable<T> source);

    public virtual async Task<TResult> ProjectToFirstAsync<TResult>(Specification<T> specification, CancellationToken cancellationToken = default)
    {
        var query = GenerateQuery(specification).AsNoTracking();

        var projectedQuery = Map<TResult>(query);

        var result = await projectedQuery.FirstOrDefaultAsync(cancellationToken);

        return result ?? throw new EntityNotFoundException(typeof(T).Name);
    }
    public virtual async Task<TResult?> ProjectToFirstOrDefaultAsync<TResult>(Specification<T> specification, CancellationToken cancellationToken = default)
    {
        var query = GenerateQuery(specification).AsNoTracking();

        var projectedQuery = Map<TResult>(query);

        return await projectedQuery.FirstOrDefaultAsync(cancellationToken);
    }
    public virtual async Task<List<TResult>> ProjectToListAsync<TResult>(Specification<T> specification, CancellationToken cancellationToken = default)
    {
        var query = GenerateQuery(specification).AsNoTracking();

        var projectedQuery = Map<TResult>(query);

        return await projectedQuery.ToListAsync(cancellationToken);
    }
    public virtual async Task<PagedResult<TResult>> ProjectToListAsync<TResult>(Specification<T> specification, PagingFilter filter, CancellationToken cancellationToken = default)
    {
        var query = GenerateQuery(specification, true).AsNoTracking();
        var projectedQuery = Map<TResult>(query);

        var count = await projectedQuery.CountAsync(cancellationToken);
        var pagination = new Pagination(_paginationSettings, count, filter);

        projectedQuery = projectedQuery.ApplyPaging(pagination);
        var data = await projectedQuery.ToListAsync(cancellationToken);

        return new PagedResult<TResult>(data, pagination);
    }
}
