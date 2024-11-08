namespace Pozitron.QuerySpecification;

/// <summary>
/// Represents a repository with mapping capabilities for projecting entities to different result types.
/// </summary>
/// <typeparam name="T">The type of the entity.</typeparam>
public abstract class RepositoryWithMapper<T> : RepositoryBase<T>, IProjectionRepository<T> where T : class
{
    private readonly PaginationSettings _paginationSettings = PaginationSettings.Default;

    /// <summary>
    /// Initializes a new instance of the <see cref="RepositoryWithMapper{T}"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    protected RepositoryWithMapper(DbContext dbContext)
        : base(dbContext)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RepositoryWithMapper{T}"/> class with the specified specification evaluator.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    /// <param name="specificationEvaluator">The specification evaluator.</param>
    protected RepositoryWithMapper(DbContext dbContext, SpecificationEvaluator specificationEvaluator)
        : base(dbContext, specificationEvaluator)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RepositoryWithMapper{T}"/> class with the specified pagination settings.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    /// <param name="paginationSettings">The pagination settings.</param>
    protected RepositoryWithMapper(DbContext dbContext, PaginationSettings paginationSettings)
        : base(dbContext)
    {
        _paginationSettings = paginationSettings;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RepositoryWithMapper{T}"/> class with the specified specification evaluator and pagination settings.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    /// <param name="specificationEvaluator">The specification evaluator.</param>
    /// <param name="paginationSettings">The pagination settings.</param>
    protected RepositoryWithMapper(DbContext dbContext, SpecificationEvaluator specificationEvaluator, PaginationSettings paginationSettings)
        : base(dbContext, specificationEvaluator)
    {
        _paginationSettings = paginationSettings;
    }

    /// <summary>
    /// Maps the source query to the result type.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="source">The source query.</param>
    /// <returns>The mapped query.</returns>
    protected abstract IQueryable<TResult> Map<TResult>(IQueryable<T> source);

    /// <inheritdoc/>
    public virtual async Task<TResult> ProjectToFirstAsync<TResult>(Specification<T> specification, CancellationToken cancellationToken = default)
    {
        var query = GenerateQuery(specification).AsNoTracking();

        var projectedQuery = Map<TResult>(query);

        var result = await projectedQuery.FirstOrDefaultAsync(cancellationToken);

        return result ?? throw new EntityNotFoundException(typeof(T).Name);
    }

    /// <inheritdoc/>
    public virtual async Task<TResult?> ProjectToFirstOrDefaultAsync<TResult>(Specification<T> specification, CancellationToken cancellationToken = default)
    {
        var query = GenerateQuery(specification).AsNoTracking();

        var projectedQuery = Map<TResult>(query);

        return await projectedQuery.FirstOrDefaultAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task<List<TResult>> ProjectToListAsync<TResult>(Specification<T> specification, CancellationToken cancellationToken = default)
    {
        var query = GenerateQuery(specification).AsNoTracking();

        var projectedQuery = Map<TResult>(query);

        return await projectedQuery.ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
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
