namespace QuerySpecification.Benchmarks;

// Benchmarks including roundtrip to the database.
[MemoryDiagnoser]
public class DbQueryBenchmark
{
    [GlobalSetup]
    public async Task Setup()
    {
        await BenchmarkDbContext.SeedAsync();
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
}
