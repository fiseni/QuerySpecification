using BenchmarkDotNet.Attributes;
using Microsoft.EntityFrameworkCore;
using Pozitron.QuerySpecification;
using Pozitron.QuerySpecification.Tests.Fixture.Entities;

namespace Pozitron.QuerySpecification.EntityFrameworkCore.Benchmarks;

[MemoryDiagnoser]
public class SpecIncludeBenchmark
{
    private readonly int max = 1000000;
    private readonly SpecificationEvaluator evaluator = SpecificationEvaluator.Default;
    private readonly Specification<Store> specInclude = new StoreIncludeProductsSpec();
    private readonly Specification<Store> specIncludeString = new StoreIncludeProductsAsStringSpec();

    private readonly IQueryable<Store> Stores;

    public SpecIncludeBenchmark()
    {
        Stores = new BenchmarkDbContext().Stores.AsQueryable();
    }

    [Benchmark]
    public void EFIncludeExpression()
    {
        for (int i = 0; i < max; i++)
        {
            _ = Stores.Include(x => x.Products);
        }
    }

    [Benchmark]
    public void EFIncludeString()
    {
        for (int i = 0; i < max; i++)
        {
            _ = Stores.Include(nameof(Store.Products));
        }
    }

    [Benchmark]
    public void SpecIncludeExpression()
    {
        for (int i = 0; i < max; i++)
        {
            _ = evaluator.GetQuery(Stores, specInclude);
        }
    }

    [Benchmark]
    public void SpecIncludeString()
    {
        for (int i = 0; i < max; i++)
        {
            _ = evaluator.GetQuery(Stores, specIncludeString);
        }
    }
}
