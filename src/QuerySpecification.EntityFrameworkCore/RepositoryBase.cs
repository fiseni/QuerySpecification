using Microsoft.EntityFrameworkCore;

namespace Pozitron.QuerySpecification;

public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
    private readonly DbContext dbContext;
    private readonly ISpecificationEvaluator specificationEvaluator;

    public RepositoryBase(DbContext dbContext)
        : this(dbContext, SpecificationEvaluator.Default)
    {
    }

    public RepositoryBase(DbContext dbContext, ISpecificationEvaluator specificationEvaluator)
    {
        this.dbContext = dbContext;
        this.specificationEvaluator = specificationEvaluator;
    }

    public virtual async Task<T> AddAsync(T entity, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        dbContext.Set<T>().Add(entity);

        if (saveChanges)
        {
            await SaveChangesAsync(cancellationToken);
        }

        return entity;
    }
    public virtual async Task UpdateAsync(T entity, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        dbContext.Entry(entity).State = EntityState.Modified;

        if (saveChanges)
        {
            await SaveChangesAsync(cancellationToken);
        }
    }
    public virtual async Task DeleteAsync(T entity, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        dbContext.Set<T>().Remove(entity);

        if (saveChanges)
        {
            await SaveChangesAsync(cancellationToken);
        }
    }
    public virtual async Task DeleteRangeAsync(IEnumerable<T> entities, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        dbContext.Set<T>().RemoveRange(entities);

        if (saveChanges)
        {
            await SaveChangesAsync(cancellationToken);
        }
    }
    public virtual async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Set<T>().FindAsync(new object[] { id }, cancellationToken: cancellationToken);
    }
    public virtual async Task<T?> GetByIdAsync<TId>(TId id, CancellationToken cancellationToken = default) where TId : notnull
    {
        return await dbContext.Set<T>().FindAsync(new object[] { id }, cancellationToken: cancellationToken);
    }
    public virtual async Task<T?> GetBySpecAsync<Spec>(Spec specification, CancellationToken cancellationToken = default) where Spec : ISpecification<T>, ISingleResultSpecification
    {
        return await ApplySpecification(specification).FirstOrDefaultAsync(cancellationToken);
    }
    public virtual async Task<TResult> GetBySpecAsync<TResult>(ISpecification<T, TResult> specification, CancellationToken cancellationToken = default)
    {
        return await ApplySpecification(specification).FirstOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<List<T>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Set<T>().ToListAsync(cancellationToken);
    }
    public virtual async Task<List<T>> ListAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
    {
        var queryResult = await ApplySpecification(specification).ToListAsync(cancellationToken);

        return specification.PostProcessingAction == null ? queryResult : specification.PostProcessingAction(queryResult).ToList();
    }
    public virtual async Task<List<TResult>> ListAsync<TResult>(ISpecification<T, TResult> specification, CancellationToken cancellationToken = default)
    {
        var queryResult = await ApplySpecification(specification).ToListAsync(cancellationToken);

        return specification.PostProcessingAction == null ? queryResult : specification.PostProcessingAction(queryResult).ToList();
    }

    public virtual async Task<int> CountAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
    {
        return await ApplySpecification(specification, true).CountAsync(cancellationToken);
    }

    protected virtual IQueryable<T> ApplySpecification(ISpecification<T> specification, bool evaluateCriteriaOnly = false)
    {
        return specificationEvaluator.GetQuery(dbContext.Set<T>().AsQueryable(), specification, evaluateCriteriaOnly);
    }
    protected virtual IQueryable<TResult> ApplySpecification<TResult>(ISpecification<T, TResult> specification)
    {
        if (specification is null) throw new ArgumentNullException("Specification is required");
        if (specification.Selector is null) throw new SelectorNotFoundException();

        return specificationEvaluator.GetQuery(dbContext.Set<T>().AsQueryable(), specification);
    }
}