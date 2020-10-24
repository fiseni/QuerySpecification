using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PozitronDev.QuerySpecification
{
    public interface IRepositoryBase<T> where T : class
    {
        Task<T> AddAsync(T entity, bool saveChanges = true);
        Task UpdateAsync(T entity, bool saveChanges = true);
        Task DeleteAsync(T entity, bool saveChanges = true);
        Task DeleteRangeAsync(IEnumerable<T> entities, bool saveChanges = true);
        Task SaveChangesAsync();

        Task<T?> GetByIdAsync(int id);
        Task<T?> GetByIdAsync<TId>(TId id);
        Task<T?> GetBySpecAsync(ISpecification<T> specification);
        Task<TResult> GetBySpecAsync<TResult>(ISpecification<T, TResult> specification);

        Task<List<T>> ListAsync();
        Task<List<T>> ListAsync(ISpecification<T> specification);
        Task<List<TResult>> ListAsync<TResult>(ISpecification<T, TResult> specification);

        Task<int> CountAsync(ISpecification<T> specification);
    }
}
