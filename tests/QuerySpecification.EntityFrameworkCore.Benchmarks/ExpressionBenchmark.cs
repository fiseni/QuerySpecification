using BenchmarkDotNet.Attributes;
using Microsoft.EntityFrameworkCore;
using Pozitron.QuerySpecification;

namespace Pozitron.QuerySpecification.EntityFrameworkCore.Benchmarks;

// Benchmarks measuring only building the IQueryable.
[MemoryDiagnoser]
public class ExpressionBenchmark
{
    private IQueryable<Store> _queryable = default!;

    [GlobalSetup]
    public async Task Setup()
    {
        // Initialize caches.
        await BenchmarkDbContext.InitializeAsync();

        _queryable = new BenchmarkDbContext().Stores.AsQueryable();
    }

    [Benchmark(Baseline = true)]
    public object EFIncludeExpression()
    {
        var id = 1;

        var result = _queryable
            .Where(x => x.Id == id)
            .Include(x => x.Products)
            .Include(x => x.Company).ThenInclude(x => x.Country);

        return result;
    }

    [Benchmark]
    public object SpecIncludeExpression()
    {
        var id = 1;

        var result = _queryable
            .WithSpecification(new StoreIncludeProductsSpec(id));

        return result;
    }
}
