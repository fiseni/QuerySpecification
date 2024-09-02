using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Pozitron.QuerySpecification.EntityFrameworkCore.Benchmarks;

public class BenchmarkDbContext : DbContext
{
    public DbSet<Country> Countries { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<Store> Stores { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Product> Products { get; set; }

    public BenchmarkDbContext()
    {
    }
    public BenchmarkDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=QuerySpecificationBenchmark;ConnectRetryCount=0");
        //optionsBuilder.LogTo(Console.WriteLine, LogLevel.Debug);
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Store>().HasOne(x => x.Address).WithOne(x => x.Store).HasForeignKey<Address>(x => x.StoreId);
    }

    public static async Task InitializeAsync()
    {
        await SeedAsync();

        using var context = new BenchmarkDbContext();

        // Initialize caches.

        var id = 1;
        _ = await context
            .Stores
            .Where(x => x.Id == id)
            .Include(x => x.Products)
            .Include(x => x.Company).ThenInclude(x => x.Country)
            .ToListAsync();

        _ = await context
            .Stores
            .WithSpecification(new StoreIncludeProductsSpec(id))
            .ToListAsync();
    }

    private static async Task SeedAsync()
    {
        using var context = new BenchmarkDbContext();
        var created = await context.Database.EnsureCreatedAsync();

        if (!created) return;

        var store = new Store
        {
            Name = "Store 1",
            Address = new Address
            {
                Street = "Street 1",
            },
            Company = new Company
            {
                Name = "Company 1",
                Country = new Country
                {
                    Name = "Country 1"
                }
            },
            Products = new List<Product>
            {
                new Product
                {
                    Name = "Product 1"
                }
            }
        };

        context.Add(store);
        await context.SaveChangesAsync();
    }
}
