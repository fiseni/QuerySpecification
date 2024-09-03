using BenchmarkDotNet.Attributes;
using Microsoft.EntityFrameworkCore;
using Pozitron.QuerySpecification;

namespace Pozitron.QuerySpecification.EntityFrameworkCore.Benchmarks;

// Benchmarks including roundtrip to the database.
[MemoryDiagnoser]
public class DbQueryBenchmark
{
    [GlobalSetup]
    public async Task Setup()
    {
        // Initialize caches.
        await BenchmarkDbContext.InitializeAsync();
    }

    [Benchmark(Baseline = true)]
    public async Task<Store> EFIncludeExpression()
    {
        var id = 1;
        using var context = new BenchmarkDbContext();

        var result = await context
            .Stores
            .Where(x => x.Id == id)
            .Include(x => x.Products)
            .Include(x => x.Company).ThenInclude(x => x.Country)
            .FirstAsync();

        return result;
    }

    [Benchmark]
    public async Task<Store> SpecIncludeExpression()
    {
        var id = 1;
        using var context = new BenchmarkDbContext();

        var result = await context
            .Stores
            .WithSpecification(new StoreIncludeProductsSpec(id))
            .FirstAsync();

        return result;
    }
}
