using Microsoft.EntityFrameworkCore;

namespace Pozitron.QuerySpecification.EntityFrameworkCore;

public class EFRepositoryFactory<TRepository, TConcreteRepository, TContext> : IRepositoryFactory<TRepository>
  where TConcreteRepository : TRepository
  where TContext : DbContext
{
    private readonly IDbContextFactory<TContext> _dbContextFactory;

    public EFRepositoryFactory(IDbContextFactory<TContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public TRepository CreateRepository()
    {
        var args = new object[] { _dbContextFactory.CreateDbContext() };
        return (TRepository)Activator.CreateInstance(typeof(TConcreteRepository), args)!;
    }
}
