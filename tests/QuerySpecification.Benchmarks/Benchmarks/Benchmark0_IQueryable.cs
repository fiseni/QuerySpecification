namespace QuerySpecification.Benchmarks;

[MemoryDiagnoser]
public class Benchmark0_IQueryable
{
    /* This benchmark measures building the IQueryable state. It's the pure overhead of using specifications.
     * Types:
     * 0 -> Empty
     * 1 -> Single Where clause
     * 2 -> Where and OrderBy
     * 3 -> Where, Order chain, Include chain, Flag (AsNoTracking)
     * 4 -> Where, Order chain, Include chain, Like, Skip, Take, Flag (AsNoTracking)
     */

    private DbSet<Store> _queryable = default!;

    [GlobalSetup]
    public void Setup()
    {
        _queryable = new BenchmarkDbContext().Stores;
    }

    [Params(0, 1, 2, 3, 4)]
    public int Type { get; set; }

    [Benchmark(Baseline = true)]
    public object EFCore()
    {
        if (Type == 0)
        {
            return _queryable;
        }
        else if (Type == 1)
        {
            return _queryable
                .Where(x => x.Id > 0);
        }
        else if (Type == 2)
        {
            return _queryable
                .Where(x => x.Id > 0)
                .OrderBy(x => x.Id);
        }
        else if (Type == 3)
        {
            return _queryable
                .Where(x => x.Id > 0)
                .OrderBy(x => x.Id)
                    .ThenBy(x => x.Name)
                .Include(x => x.Company)
                    .ThenInclude(x => x.Country)
                .AsNoTracking();
        }
        else
        {
            var nameTerm = "tore";
            return _queryable
                .Where(x => x.Id > 0)
                .OrderBy(x => x.Id)
                    .ThenBy(x => x.Name)
                .Include(x => x.Company)
                    .ThenInclude(x => x.Country)
                .Where(x => EF.Functions.Like(x.Name, $"%{nameTerm}%"))
                .Skip(1)
                .Take(1)
                .AsNoTracking();
        }
    }

    [Benchmark]
    public object Spec()
    {
        if (Type == 0)
        {
            var spec = new Specification<Store>();
            return _queryable
                .WithSpecification(spec);
        }
        else if (Type == 1)
        {
            var spec = new Specification<Store>();
            spec.Query
                .Where(x => x.Id > 0);

            return _queryable
                .WithSpecification(spec);
        }
        else if (Type == 2)
        {
            var spec = new Specification<Store>();
            spec.Query
                .Where(x => x.Id > 0)
                .OrderBy(x => x.Id);

            return _queryable
                .WithSpecification(spec);
        }
        else if (Type == 3)
        {
            var spec = new Specification<Store>(6);
            spec.Query
                .Where(x => x.Id > 0)
                .OrderBy(x => x.Id)
                    .ThenBy(x => x.Name)
                .Include(x => x.Company)
                    .ThenInclude(x => x.Country)
                .AsNoTracking();

            return _queryable
                .WithSpecification(spec);
        }
        else
        {
            var nameTerm = "tore";
            var spec = new Specification<Store>(7);
            spec.Query
                .Where(x => x.Id > 0)
                .OrderBy(x => x.Id)
                    .ThenBy(x => x.Name)
                .Include(x => x.Company)
                    .ThenInclude(x => x.Country)
                .Like(x => x.Name, $"%{nameTerm}%")
                .Skip(1)
                .Take(1)
                .AsNoTracking();

            return _queryable
                .WithSpecification(spec);
        }
    }
}
