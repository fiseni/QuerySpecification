using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PozitronDev.QuerySpecification.EntityFrameworkCore3
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        private readonly DbContext dbContext;
        private readonly ISpecificationEvaluator<T> specificationEvaluator;

        public RepositoryBase(DbContext dbContext)
        {
            this.dbContext = dbContext;
            this.specificationEvaluator = new SpecificationEvaluator<T>();
        }

        public RepositoryBase(DbContext dbContext, ISpecificationEvaluator<T> specificationEvaluator)
        {
            this.dbContext = dbContext;
            this.specificationEvaluator = specificationEvaluator;
        }

        public virtual async Task<T> AddAsync(T entity, bool saveChanges = true)
        {
            dbContext.Set<T>().Add(entity);

            if (saveChanges)
            {
                await SaveChangesAsync();
            }

            return entity;
        }
        public virtual async Task UpdateAsync(T entity, bool saveChanges = true)
        {
            dbContext.Entry(entity).State = EntityState.Modified;

            if (saveChanges)
            {
                await SaveChangesAsync();
            }
        }
        public virtual async Task DeleteAsync(T entity, bool saveChanges = true)
        {
            dbContext.Set<T>().Remove(entity);

            if (saveChanges)
            {
                await SaveChangesAsync();
            }
        }
        public virtual async Task DeleteRangeAsync(IEnumerable<T> entities, bool saveChanges = true)
        {
            dbContext.Set<T>().RemoveRange(entities);

            if (saveChanges)
            {
                await SaveChangesAsync();
            }
        }
        public virtual async Task SaveChangesAsync()
        {
            await dbContext.SaveChangesAsync();
        }

        public virtual async Task<T?> GetByIdAsync(int id)
        {
            return await dbContext.Set<T>().FindAsync(id);
        }
        public virtual async Task<T?> GetByIdAsync<TId>(TId id)
        {
            return await dbContext.Set<T>().FindAsync(id);
        }
        public virtual async Task<T?> GetBySpecAsync(ISpecification<T> specification)
        {
            return await ApplySpecification(specification).FirstOrDefaultAsync();
        }
        public virtual async Task<TResult> GetBySpecAsync<TResult>(ISpecification<T, TResult> specification)
        {
            return await ApplySpecification(specification).FirstOrDefaultAsync();
        }

        public virtual async Task<List<T>> ListAsync()
        {
            return await dbContext.Set<T>().ToListAsync();
        }
        public virtual async Task<List<T>> ListAsync(ISpecification<T> specification)
        {
            var queryResult = await ApplySpecification(specification).ToListAsync();

            return specification.InMemory == null ? queryResult : specification.InMemory(queryResult);
        }
        public virtual async Task<List<TResult>> ListAsync<TResult>(ISpecification<T, TResult> specification)
        {
            var queryResult = await ApplySpecification(specification).ToListAsync();

            return specification.InMemory == null ? queryResult : specification.InMemory(queryResult);
        }

        public virtual async Task<int> CountAsync(ISpecification<T> specification)
        {
            return await ApplySpecification(specification, true).CountAsync();
        }

        protected IQueryable<T> ApplySpecification(ISpecification<T> specification, bool evaluateCriteriaOnly = false)
        {
            return specificationEvaluator.GetQuery(dbContext.Set<T>().AsQueryable(), specification, evaluateCriteriaOnly);
        }
        protected IQueryable<TResult> ApplySpecification<TResult>(ISpecification<T, TResult> specification, bool evaluateCriteriaOnly = false)
        {
            if (specification is null) throw new ArgumentNullException("Specification is required");
            if (specification.Selector is null) throw new SelectorNotFoundException();

            return specificationEvaluator.GetQuery(dbContext.Set<T>().AsQueryable(), specification, evaluateCriteriaOnly);
        }
    }
}