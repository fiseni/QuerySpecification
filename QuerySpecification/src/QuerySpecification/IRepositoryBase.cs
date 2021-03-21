using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PozitronDev.QuerySpecification
{
    public interface IRepositoryBase<T> : IReadRepositoryBase<T> where T : class
    {
        Task<T> AddAsync(T entity, bool saveChanges = true, CancellationToken cancellationToken = default);
        Task UpdateAsync(T entity, bool saveChanges = true, CancellationToken cancellationToken = default);
        Task DeleteAsync(T entity, bool saveChanges = true, CancellationToken cancellationToken = default);
        Task DeleteRangeAsync(IEnumerable<T> entities, bool saveChanges = true, CancellationToken cancellationToken = default);
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
