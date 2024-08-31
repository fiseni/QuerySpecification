using Pozitron.QuerySpecification.Tests.Fixture.Entities;
using Xunit;

namespace Pozitron.QuerySpecification.EntityFrameworkCore.Tests.Fixture;

public class IntegrationTestBase : IClassFixture<SharedDatabaseFixture>
{
    protected TestDbContext dbContext;
    protected Repository<Company> companyRepository;
    protected Repository<Store> storeRepository;

    public IntegrationTestBase(SharedDatabaseFixture fixture)
    {
        dbContext = fixture.CreateContext();

        companyRepository = new Repository<Company>(dbContext);
        storeRepository = new Repository<Store>(dbContext);
    }
}
