namespace Pozitron.QuerySpecification;

/// <summary>
/// Represents a base repository for accessing and modifying entities.
/// </summary>
/// <typeparam name="T">The type of the entity.</typeparam>
public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
    private readonly DbContext _dbContext;
    private readonly SpecificationEvaluator _evaluator;

    /// <summary>
    /// Initializes a new instance of the <see cref="RepositoryBase{T}"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    protected RepositoryBase(DbContext dbContext)
        : this(dbContext, SpecificationEvaluator.Default)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RepositoryBase{T}"/> class with the specified specification evaluator.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    /// <param name="specificationEvaluator">The specification evaluator.</param>
    protected RepositoryBase(DbContext dbContext, SpecificationEvaluator specificationEvaluator)
    {
        _dbContext = dbContext;
        _evaluator = specificationEvaluator;
    }

    /// <inheritdoc/>
    public virtual async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        _dbContext.Set<T>().Add(entity);

        await SaveChangesAsync(cancellationToken);

        return entity;
    }

    /// <inheritdoc/>
    public virtual async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        _dbContext.Set<T>().AddRange(entities);

        await SaveChangesAsync(cancellationToken);

        return entities;
    }

    /// <inheritdoc/>
    public virtual async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        //dbContext.Set<T>().Update(entity);

        await SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        _dbContext.Set<T>().Remove(entity);

        await SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        _dbContext.Set<T>().RemoveRange(entities);

        await SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async ValueTask<T?> FindAsync<TId>(TId id, CancellationToken cancellationToken = default) where TId : notnull
    {
        return await _dbContext.Set<T>().FindAsync([id], cancellationToken: cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task<T> FirstAsync(Specification<T> specification, CancellationToken cancellationToken = default)
    {
        var result = await GenerateQuery(specification).FirstOrDefaultAsync(cancellationToken);
        return result ?? throw new EntityNotFoundException(typeof(T).Name);
    }

    /// <inheritdoc/>
    public virtual async Task<TResult> FirstAsync<TResult>(Specification<T, TResult> specification, CancellationToken cancellationToken = default)
    {
        var result = await GenerateQuery(specification).FirstOrDefaultAsync(cancellationToken);
        return result ?? throw new EntityNotFoundException(typeof(T).Name);
    }

    /// <inheritdoc/>
    public virtual async Task<T?> FirstOrDefaultAsync(Specification<T> specification, CancellationToken cancellationToken = default)
    {
        return await GenerateQuery(specification).FirstOrDefaultAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task<TResult?> FirstOrDefaultAsync<TResult>(Specification<T, TResult> specification, CancellationToken cancellationToken = default)
    {
        return await GenerateQuery(specification).FirstOrDefaultAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task<T?> SingleOrDefaultAsync(Specification<T> specification, CancellationToken cancellationToken = default)
    {
        return await GenerateQuery(specification).SingleOrDefaultAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task<TResult?> SingleOrDefaultAsync<TResult>(Specification<T, TResult> specification, CancellationToken cancellationToken = default)
    {
        return await GenerateQuery(specification).SingleOrDefaultAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task<List<T>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<T>().ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task<List<T>> ListAsync(Specification<T> specification, CancellationToken cancellationToken = default)
    {
        return await GenerateQuery(specification).ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task<List<TResult>> ListAsync<TResult>(Specification<T, TResult> specification, CancellationToken cancellationToken = default)
    {
        return await GenerateQuery(specification).ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<T>().CountAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task<int> CountAsync(Specification<T> specification, CancellationToken cancellationToken = default)
    {
        return await GenerateQuery(specification, true).CountAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task<int> CountAsync<TResult>(Specification<T, TResult> specification, CancellationToken cancellationToken = default)
    {
        return await GenerateQuery(specification, true).CountAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task<bool> AnyAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<T>().AnyAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task<bool> AnyAsync(Specification<T> specification, CancellationToken cancellationToken = default)
    {
        return await GenerateQuery(specification, true).AnyAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task<bool> AnyAsync<TResult>(Specification<T, TResult> specification, CancellationToken cancellationToken = default)
    {
        return await GenerateQuery(specification, true).AnyAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public virtual IAsyncEnumerable<T> AsAsyncEnumerable(Specification<T> specification)
    {
        return GenerateQuery(specification).AsAsyncEnumerable();
    }

    /// <summary>
    /// Generates a query based on the specification.
    /// </summary>
    /// <param name="specification">The specification to evaluate.</param>
    /// <param name="ignorePaging">Whether to ignore paging settings (Take/Skip) defined in the specification.</param>
    /// <returns>The generated query.</returns>
    protected virtual IQueryable<T> GenerateQuery(Specification<T> specification, bool ignorePaging = false)
    {
        var query = _evaluator.Evaluate(_dbContext.Set<T>(), specification, ignorePaging);
        return query;
    }

    /// <summary>
    /// Generates a query based on the specification.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="specification">The specification to evaluate.</param>
    /// <param name="ignorePaging">Whether to ignore paging settings (Take/Skip) defined in the specification.</param>
    /// <returns>The generated query.</returns>
    protected virtual IQueryable<TResult> GenerateQuery<TResult>(Specification<T, TResult> specification, bool ignorePaging = false)
    {
        var query = _evaluator.Evaluate(_dbContext.Set<T>(), specification, ignorePaging);
        return query;
    }
}
