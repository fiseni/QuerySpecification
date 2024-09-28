namespace Pozitron.QuerySpecification;

public abstract class RepositoryBase<T> : IRepositoryBase<T>, IReadRepositoryBase<T> where T : class
{
    private readonly DbContext _dbContext;
    private readonly SpecificationEvaluator _evaluator;
    private readonly PaginationSettings _paginationSettings;

    protected RepositoryBase(DbContext dbContext)
        : this(dbContext, SpecificationEvaluator.Default, PaginationSettings.Default)
    {
    }
    protected RepositoryBase(DbContext dbContext, SpecificationEvaluator specificationEvaluator)
        : this(dbContext, specificationEvaluator, PaginationSettings.Default)
    {
    }
    protected RepositoryBase(DbContext dbContext, PaginationSettings paginationSettings)
        : this(dbContext, SpecificationEvaluator.Default, paginationSettings)
    {
    }
    protected RepositoryBase(DbContext dbContext, SpecificationEvaluator specificationEvaluator, PaginationSettings paginationSettings)
    {
        _dbContext = dbContext;
        _evaluator = specificationEvaluator;
        _paginationSettings = paginationSettings;
    }

    protected abstract IQueryable<TResult> Map<TResult>(IQueryable<T> source);

    public virtual async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        _dbContext.Set<T>().Add(entity);

        await SaveChangesAsync(cancellationToken);

        return entity;
    }
    public virtual async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        _dbContext.Set<T>().AddRange(entities);

        await SaveChangesAsync(cancellationToken);

        return entities;
    }
    public virtual async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        //dbContext.Set<T>().Update(entity);

        await SaveChangesAsync(cancellationToken);
    }
    public virtual async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        _dbContext.Set<T>().Remove(entity);

        await SaveChangesAsync(cancellationToken);
    }
    public virtual async Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        _dbContext.Set<T>().RemoveRange(entities);

        await SaveChangesAsync(cancellationToken);
    }
    public virtual async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public virtual async ValueTask<T?> FindAsync<TId>(TId id, CancellationToken cancellationToken = default) where TId : notnull
    {
        return await _dbContext.Set<T>().FindAsync([id], cancellationToken: cancellationToken);
    }
    public virtual async Task<T> FirstAsync(Specification<T> specification, CancellationToken cancellationToken = default)
    {
        var result = await GenerateQuery(specification).FirstOrDefaultAsync(cancellationToken);
        return result ?? throw new EntityNotFoundException(typeof(T).Name);
    }
    public virtual async Task<TResult> FirstAsync<TResult>(Specification<T, TResult> specification, CancellationToken cancellationToken = default)
    {
        var result = await GenerateQuery(specification).FirstOrDefaultAsync(cancellationToken);
        return result ?? throw new EntityNotFoundException(typeof(T).Name);
    }
    public virtual async Task<T?> FirstOrDefaultAsync(Specification<T> specification, CancellationToken cancellationToken = default)
    {
        return await GenerateQuery(specification).FirstOrDefaultAsync(cancellationToken);
    }
    public virtual async Task<TResult?> FirstOrDefaultAsync<TResult>(Specification<T, TResult> specification, CancellationToken cancellationToken = default)
    {
        return await GenerateQuery(specification).FirstOrDefaultAsync(cancellationToken);
    }
    public virtual async Task<T?> SingleOrDefaultAsync(Specification<T> specification, CancellationToken cancellationToken = default)
    {
        return await GenerateQuery(specification).SingleOrDefaultAsync(cancellationToken);
    }
    public virtual async Task<TResult?> SingleOrDefaultAsync<TResult>(Specification<T, TResult> specification, CancellationToken cancellationToken = default)
    {
        return await GenerateQuery(specification).SingleOrDefaultAsync(cancellationToken);
    }
    public virtual async Task<List<T>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<T>().ToListAsync(cancellationToken);
    }
    public virtual async Task<List<T>> ListAsync(Specification<T> specification, CancellationToken cancellationToken = default)
    {
        return await GenerateQuery(specification).ToListAsync(cancellationToken);
    }
    public virtual async Task<List<TResult>> ListAsync<TResult>(Specification<T, TResult> specification, CancellationToken cancellationToken = default)
    {
        return await GenerateQuery(specification).ToListAsync(cancellationToken);
    }
    public virtual async Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<T>().CountAsync(cancellationToken);
    }
    public virtual async Task<int> CountAsync(Specification<T> specification, CancellationToken cancellationToken = default)
    {
        return await GenerateQuery(specification, true).CountAsync(cancellationToken);
    }
    public virtual async Task<int> CountAsync<TResult>(Specification<T, TResult> specification, CancellationToken cancellationToken = default)
    {
        return await GenerateQuery(specification, true).CountAsync(cancellationToken);
    }
    public virtual async Task<bool> AnyAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<T>().AnyAsync(cancellationToken);
    }
    public virtual async Task<bool> AnyAsync(Specification<T> specification, CancellationToken cancellationToken = default)
    {
        return await GenerateQuery(specification, true).AnyAsync(cancellationToken);
    }
    public virtual async Task<bool> AnyAsync<TResult>(Specification<T, TResult> specification, CancellationToken cancellationToken = default)
    {
        return await GenerateQuery(specification, true).AnyAsync(cancellationToken);
    }

    public virtual IAsyncEnumerable<T> AsAsyncEnumerable(Specification<T> specification)
    {
        return GenerateQuery(specification).AsAsyncEnumerable();
    }
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

    protected virtual IQueryable<T> GenerateQuery(Specification<T> specification, bool ignorePaging = false)
    {
        var query = _evaluator.Evaluate(_dbContext.Set<T>(), specification, ignorePaging);
        return query;
    }
    protected virtual IQueryable<TResult> GenerateQuery<TResult>(Specification<T, TResult> specification, bool ignorePaging = false)
    {
        var query = _evaluator.Evaluate(_dbContext.Set<T>(), specification, ignorePaging);
        return query;
    }
}
