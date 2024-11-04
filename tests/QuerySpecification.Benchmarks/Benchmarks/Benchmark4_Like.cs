namespace QuerySpecification.Benchmarks;

[MemoryDiagnoser]
public class Benchmark4_Like
{
    private DbSet<Store> _queryable = default!;

    [GlobalSetup]
    public void Setup()
    {
        _queryable = new BenchmarkDbContext().Stores;
    }

    [Params(0, 1, 2, 6)]
    public int Count { get; set; }

    [Benchmark(Baseline = true)]
    public object EFCore()
    {
        var nameSearchTerm = "%tore%";
        if (Count == 0)
        {
            return _queryable;
        }
        else if (Count == 1)
        {
            return _queryable
                .Where(x => EF.Functions.Like(x.Name, nameSearchTerm));
        }
        else if (Count == 2)
        {
            return _queryable
                .Where(x => EF.Functions.Like(x.Name, nameSearchTerm))
                .Where(x => EF.Functions.Like(x.Name, nameSearchTerm));
        }
        else
        {
            return _queryable
                .Where(x => EF.Functions.Like(x.Name, nameSearchTerm) || EF.Functions.Like(x.Name, nameSearchTerm))
                .Where(x => EF.Functions.Like(x.Name, nameSearchTerm) || EF.Functions.Like(x.Name, nameSearchTerm))
                .Where(x => EF.Functions.Like(x.Name, nameSearchTerm) || EF.Functions.Like(x.Name, nameSearchTerm));
        }
    }


    [Benchmark]
    public object Spec()
    {
        var nameSearchTerm = "%tore%";
        if (Count == 0)
        {
            var spec = new Specification<Store>();
            return _queryable.WithSpecification(spec);
        }
        else if (Count == 1)
        {
            var spec = new Specification<Store>();
            spec.Query
                .Like(x => x.Name, nameSearchTerm);
            return _queryable.WithSpecification(spec);
        }
        else if (Count == 2)
        {
            var spec = new Specification<Store>();
            spec.Query
                .Like(x => x.Name, nameSearchTerm, 2)
                .Like(x => x.Name, nameSearchTerm, 1);
            return _queryable.WithSpecification(spec);
        }
        else
        {
            var spec = new Specification<Store>(6);
            spec.Query
                .Like(x => x.Name, nameSearchTerm, 2)
                .Like(x => x.Name, nameSearchTerm, 3)
                .Like(x => x.Name, nameSearchTerm, 1)
                .Like(x => x.Name, nameSearchTerm, 3)
                .Like(x => x.Name, nameSearchTerm, 2)
                .Like(x => x.Name, nameSearchTerm, 1);
            return _queryable.WithSpecification(spec);
        }
    }
}
