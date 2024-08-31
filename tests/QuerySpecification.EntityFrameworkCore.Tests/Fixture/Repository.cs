namespace Pozitron.QuerySpecification.EntityFrameworkCore.Tests.Fixture;

public class Repository<T> : RepositoryBase<T> where T : class
{
    protected readonly TestDbContext dbContext;

    public Repository(TestDbContext testDbContext) : base(testDbContext)
    {
        dbContext = testDbContext;
    }
}
