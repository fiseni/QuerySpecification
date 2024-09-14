using BenchmarkDotNet.Attributes;
using Microsoft.EntityFrameworkCore;
using Pozitron.QuerySpecification;

namespace Pozitron.QuerySpecification.EntityFrameworkCore.Benchmarks;

// Benchmarks excluding roundtrip to the database, just evaluating the query string.
[MemoryDiagnoser]
public class QueryStringBenchmark
{
    [Benchmark(Baseline = true)]
    public string EFIncludeExpression()
    {
        var id = 1;
        using var context = new BenchmarkDbContext();

        var queryString = context
            .Stores
            .Where(x => x.Id == id)
            .Include(x => x.Products)
            .Include(x => x.Company).ThenInclude(x => x.Country)
            .ToQueryString();

        return queryString;
    }

    [Benchmark]
    public string SpecIncludeExpression()
    {
        var id = 1;
        using var context = new BenchmarkDbContext();

        var queryString = context
            .Stores
            .WithSpecification(new StoreIncludeProductsSpec(id))
            .ToQueryString();

        return queryString;
    }

    [Benchmark]
    public string EFIncludeString()
    {
        var id = 1;
        using var context = new BenchmarkDbContext();

        var queryString = context
            .Stores
            .Where(x => x.Id == id)
            .Include(nameof(Store.Products))
            .Include($"{nameof(Store.Company)}.{nameof(Company.Country)}")
            .ToQueryString();

        return queryString;
    }

    [Benchmark]
    public string SpecIncludeString()
    {
        var id = 1;
        using var context = new BenchmarkDbContext();

        var queryString = context
            .Stores
            .WithSpecification(new StoreIncludeProductsAsStringSpec(id))
            .ToQueryString();

        return queryString;
    }

    private sealed class StoreIncludeProductsSpec : Specification<Store>
    {
        public StoreIncludeProductsSpec(int id)
        {
            Query
                .Where(x => x.Id == id)
                .Include(x => x.Products)
                .Include(x => x.Company).ThenInclude(x => x.Country);
        }
    }

    private sealed class StoreIncludeProductsAsStringSpec : Specification<Store>
    {
        public StoreIncludeProductsAsStringSpec(int id)
        {
            Query
                .Where(x => x.Id == id)
                .Include(nameof(Store.Products))
                .Include($"{nameof(Store.Company)}.{nameof(Company.Country)}");
        }
    }
}
