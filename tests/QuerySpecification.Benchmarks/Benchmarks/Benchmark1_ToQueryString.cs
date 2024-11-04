namespace QuerySpecification.Benchmarks;

[MemoryDiagnoser]
public class Benchmark1_ToQueryString
{
    /* This benchmark measures building the final SQL query.
     * Types:
     * 0 -> Empty
     * 1 -> Single Where clause
     * 2 -> Where and OrderBy
     * 3 -> Where, Order chain, Include chain, Flag (AsNoTracking)
     * 4 -> Where, Order chain, Include chain, Like, Skip, Take, Flag (AsNoTracking)
     */

    [Params(0, 1, 2, 3, 4)]
    public int Type { get; set; }

    [Benchmark(Baseline = true)]
    public string EFCore()
    {
        using var context = new BenchmarkDbContext();

        if (Type == 0)
        {
            return context.Stores
                .ToQueryString();
        }
        else if (Type == 1)
        {
            return context.Stores
                .Where(x => x.Id > 0)
                .ToQueryString();
        }
        else if (Type == 2)
        {
            return context.Stores
                .Where(x => x.Id > 0)
                .OrderBy(x => x.Id)
                .ToQueryString();
        }
        else if (Type == 3)
        {
            return context.Stores
                .Where(x => x.Id > 0)
                .OrderBy(x => x.Id)
                    .ThenBy(x => x.Name)
                .Include(x => x.Company)
                    .ThenInclude(x => x.Country)
                .AsNoTracking()
                .ToQueryString();
        }
        else
        {
            var nameTerm = "tore";
            return context.Stores
                .Where(x => x.Id > 0)
                .OrderBy(x => x.Id)
                    .ThenBy(x => x.Name)
                .Include(x => x.Company)
                    .ThenInclude(x => x.Country)
                .Where(x => EF.Functions.Like(x.Name, $"%{nameTerm}%"))
                .Skip(1)
                .Take(1)
                .AsNoTracking()
                .ToQueryString();
        }
    }

    [Benchmark]
    public string Spec()
    {
        using var context = new BenchmarkDbContext();

        if (Type == 0)
        {
            var spec = new Specification<Store>();

            return context.Stores
                .WithSpecification(spec)
                .ToQueryString();
        }
        else if (Type == 1)
        {
            var spec = new Specification<Store>();
            spec.Query
                .Where(x => x.Id > 0);

            return context.Stores
                .WithSpecification(spec)
                .ToQueryString();
        }
        else if (Type == 2)
        {
            var spec = new Specification<Store>();
            spec.Query
                .Where(x => x.Id > 0)
                .OrderBy(x => x.Id);

            return context.Stores
                .WithSpecification(spec)
                .ToQueryString();
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

            return context.Stores
                .WithSpecification(spec)
                .ToQueryString();
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

            return context.Stores
                .WithSpecification(spec)
                .ToQueryString();
        }
    }
}
