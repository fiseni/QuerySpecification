using MartinCostello.SqlLocalDb;
using Respawn;

namespace QuerySpecification.EntityFrameworkCore.Tests.Fixture;

public class TestFactory : IAsyncLifetime
{
    private string _connectionString = default!;
    private Respawner _respawner = default!;

    public DbContextOptions<TestDbContext> DbContextOptions { get; private set; } = default!;

    public Task ResetDatabase() => _respawner.ResetAsync(_connectionString);

    public async Task InitializeAsync()
    {
        using (var localDB = new SqlLocalDbApi())
        {
            _connectionString = localDB.IsLocalDBInstalled()
                ? $"Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=QuerySpecificationTestsDB;Integrated Security=SSPI;TrustServerCertificate=True;"
                : $"Data Source=databaseEF;Initial Catalog=QuerySpecificationTestsDB;PersistSecurityInfo=True;User ID=sa;Password=P@ssW0rd!;Encrypt=False;TrustServerCertificate=True;";
        }

        Console.WriteLine($"Connection string: {_connectionString}");

        DbContextOptions = new DbContextOptionsBuilder<TestDbContext>()
            .UseSqlServer(_connectionString)
            .EnableDetailedErrors()
            .EnableSensitiveDataLogging()
            .Options;

        using var dbContext = new TestDbContext(DbContextOptions);

        //await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.EnsureCreatedAsync();

        _respawner = await Respawner.CreateAsync(_connectionString, new RespawnerOptions
        {
            DbAdapter = DbAdapter.SqlServer,
            SchemasToInclude = ["dbo"],
        });

        await ResetDatabase();
    }

    public Task DisposeAsync() => Task.CompletedTask;

    //public async Task DisposeAsync()
    //{
    //    using var dbContext = new TestDbContext(DbContextOptions);
    //    await dbContext.Database.EnsureDeletedAsync();
    //}
}
