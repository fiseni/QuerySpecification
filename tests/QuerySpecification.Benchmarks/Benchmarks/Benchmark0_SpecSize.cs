using System.Linq.Expressions;

namespace QuerySpecification.Benchmarks;

[MemoryDiagnoser]
public class Benchmark0_SpecSize
{
    /* This benchmark is just used to measure the Specification sizes and detect eventual regressions.
     * We measure with provided expressions, so it measures pure spec overhead.
     * Types:
     * 0 -> Empty
     * 1 -> Single Where clause
     * 2 -> Where and OrderBy
     * 3 -> Where, Order chain, Include chain, Flag (AsNoTracking)
     * 4 -> Where, Order chain, Include chain, Like, Skip, Take, Flag (AsNoTracking)
     * 
     * Here is the comparison between version 10 and version 11.

        | Method     | Type | Mean      | Error     | StdDev    | Ratio | RatioSD | Gen0   | Gen1   | Allocated | Alloc Ratio |
        |----------- |----- |----------:|----------:|----------:|------:|--------:|-------:|-------:|----------:|------------:|
        | Version_10 | 0    |  8.065 ns | 0.1938 ns | 0.1904 ns |  1.00 |    0.03 | 0.0134 |      - |     112 B |        1.00 |
        | Version_11 | 0    |  3.263 ns | 0.0249 ns | 0.0208 ns |  0.40 |    0.01 | 0.0029 |      - |      24 B |        0.21 |
        |            |      |           |           |           |       |         |        |        |           |             |
        | Version_10 | 1    | 19.677 ns | 0.1520 ns | 0.1270 ns |  1.00 |    0.01 | 0.0258 |      - |     216 B |        1.00 |
        | Version_11 | 1    |  9.618 ns | 0.0537 ns | 0.0449 ns |  0.49 |    0.00 | 0.0124 |      - |     104 B |        0.48 |
        |            |      |           |           |           |       |         |        |        |           |             |
        | Version_10 | 2    | 36.692 ns | 0.7407 ns | 0.7274 ns |  1.00 |    0.03 | 0.0430 |      - |     360 B |        1.00 |
        | Version_11 | 2    | 14.613 ns | 0.0780 ns | 0.0730 ns |  0.40 |    0.01 | 0.0124 |      - |     104 B |        0.29 |
        |            |      |           |           |           |       |         |        |        |           |             |
        | Version_10 | 3    | 64.766 ns | 1.2484 ns | 1.1678 ns |  1.00 |    0.03 | 0.0774 | 0.0001 |     648 B |        1.00 |
        | Version_11 | 3    | 42.890 ns | 0.1451 ns | 0.1133 ns |  0.66 |    0.01 | 0.0258 |      - |     216 B |        0.33 |
        |            |      |           |           |           |       |         |        |        |           |             |
        | Version_10 | 4    | 73.354 ns | 0.5833 ns | 0.5171 ns |  1.00 |    0.01 | 0.0918 | 0.0002 |     768 B |        1.00 |
        | Version_11 | 4    | 71.566 ns | 0.5052 ns | 0.4478 ns |  0.98 |    0.01 | 0.0343 |      - |     288 B |        0.38 |
     */

    public static class Expressions
    {
        public static Expression<Func<Store, bool>> Criteria { get; } = x => x.Id > 0;
        public static Expression<Func<Store, object?>> OrderBy { get; } = x => x.Id;
        public static Expression<Func<Store, object?>> OrderThenBy { get; } = x => x.Name;
        public static Expression<Func<Store, Company>> IncludeStoreCompany { get; } = x => x.Company;
        public static Expression<Func<Company, Country>> IncludeCompanyCountry { get; } = x => x.Country;
        public static Expression<Func<Store, string?>> Like { get; } = x => x.Name;
    }

    [Params(0, 1, 2, 3, 4)]
    public int Type { get; set; }

    [Benchmark]
    public object Spec()
    {
        if (Type == 0)
        {
            var spec = new Specification<Store>();
            return spec;
        }
        else if (Type == 1)
        {
            var spec = new Specification<Store>();
            spec.Query
                .Where(Expressions.Criteria);

            return spec;
        }
        else if (Type == 2)
        {
            var spec = new Specification<Store>();
            spec.Query
                .Where(Expressions.Criteria)
                .OrderBy(Expressions.OrderBy);

            return spec;
        }
        else if (Type == 3)
        {
            var spec = new Specification<Store>(6);
            spec.Query
                .Where(Expressions.Criteria)
                .OrderBy(Expressions.OrderBy)
                    .ThenBy(Expressions.OrderThenBy)
                .Include(Expressions.IncludeStoreCompany)
                    .ThenInclude(Expressions.IncludeCompanyCountry)
                .AsNoTracking();

            return spec;
        }
        else
        {
            var spec = new Specification<Store>(7);
            spec.Query
                .Where(Expressions.Criteria)
                .OrderBy(Expressions.OrderBy)
                    .ThenBy(Expressions.OrderThenBy)
                .Include(Expressions.IncludeStoreCompany)
                    .ThenInclude(Expressions.IncludeCompanyCountry)
                .Like(Expressions.Like, "%tore%")
                .Skip(1)
                .Take(1)
                .AsNoTracking();

            return spec;
        }
    }
}
