using System.Reflection;

namespace QuerySpecification.Benchmarks;

// Benchmarks measuring only building the IQueryable.
[MemoryDiagnoser]
public class LikeInMemoryBenchmark
{
    public record Customer(int Id, string FirstName, string? LastName);
    private class CustomerSpec : Specification<Customer>
    {
        public CustomerSpec()
        {
            Query
            .Like(x => x.FirstName, "%xx%", 1)
            .Like(x => x.LastName, "%xy%", 2)
            .Like(x => x.LastName, "%xz%", 2);
        }
    }

    private CustomerSpec _specification = default!;
    private List<Customer> _source = default!;

    [GlobalSetup]
    public void Setup()
    {
        _specification = new CustomerSpec();
        _source =
        [
            new(1, "axxa", "axya"),
            new(2, "aaaa", "aaaa"),
            new(3, "axxa", "axza"),
            new(4, "aaaa", null),
            new(5, "axxa", null),
            .. Enumerable.Range(6, 1000).Select(x => new Customer(x, "axxa", "axya"))
        ];
    }

    [Benchmark(Baseline = true)]
    public List<Customer> EvaluateOption1()
    {
        var source = _source.AsEnumerable();

        foreach (var likeGroup in _specification.LikeExpressions.GroupBy(x => x.Group))
        {
            source = source.Where(x => likeGroup.Any(c => c.KeySelectorFunc(x)?.Like(c.Pattern) ?? false));
        }

        return source.ToList();
    }

    [Benchmark]
    public List<Customer> EvaluateOption2()
    {
        var source = _source.AsEnumerable();

        // Precompute the predicates for each group
        var groupPredicates = _specification
            .LikeExpressions
            .GroupBy(x => x.Group)
            .Select(group => new Func<Customer, bool>(x => group.Any(c => c.KeySelectorFunc(x)?.Like(c.Pattern) ?? false)))
            .ToList();

        // Apply all predicates to filter the source
        var result = source.Where(x => groupPredicates.All(predicate => predicate(x)));

        return result.ToList();
    }
}

public static class LikeExtensions
{
    private static readonly MethodInfo _likeMethod = typeof(LikeMemoryEvaluator).Assembly
        .GetType("Pozitron.QuerySpecification.LikeExtension")!
        .GetMethod("Like", BindingFlags.Public | BindingFlags.Static)!;

    // I don't want to expose the internal types to Benchmark project.
    // There is overhead here with reflection, but it affects all benchmarks equally.
    public static bool Like(this string input, string pattern)
    {
        bool result = (bool)_likeMethod!.Invoke(null, [input, pattern])!;
        return result;
    }
}
