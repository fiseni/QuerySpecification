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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=QuerySpecificationBenchmark;ConnectRetryCount=0");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Store>().HasOne(x => x.Address).WithOne(x => x.Store).HasForeignKey<Address>(x => x.StoreId);
    }

    public static async Task SeedAsync()
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
