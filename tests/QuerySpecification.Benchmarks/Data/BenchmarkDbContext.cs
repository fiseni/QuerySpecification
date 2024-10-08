﻿namespace QuerySpecification.Benchmarks;

public class BenchmarkDbContext : DbContext
{
    public DbSet<Store> Stores { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=QuerySpecificationBenchmark;ConnectRetryCount=0");

    public static async Task SeedAsync()
    {
        using var context = new BenchmarkDbContext();
        var created = await context.Database.EnsureCreatedAsync();

        if (!created) return;

        var store = new Store
        {
            Name = "Store 1",
            Company = new()
            {
                Name = "Company 1",
                Country = new()
                {
                    Name = "Country 1"
                }
            },
            Products =
            [
                new()
                {
                    Name = "Product 1"
                }
            ]
        };

        context.Add(store);
        await context.SaveChangesAsync();
    }
}
