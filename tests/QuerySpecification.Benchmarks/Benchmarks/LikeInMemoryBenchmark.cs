namespace QuerySpecification.Benchmarks;

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
    public int EvaluateOriginal()
    {
        var evaluator = LikeMemoryEvaluatorOriginal.Instance;
        var result = evaluator.Evaluate(_source, _specification);
        return result.Count();
    }

    [Benchmark]
    public int EvaluateCurrent()
    {
        var evaluator = LikeMemoryEvaluator.Instance;
        var result = evaluator.Evaluate(_source, _specification);
        return result.Count();
    }
}

public sealed class LikeMemoryEvaluatorOriginal : IInMemoryEvaluator
{
    private LikeMemoryEvaluatorOriginal() { }
    public static LikeMemoryEvaluatorOriginal Instance = new();

    public IEnumerable<T> Evaluate<T>(IEnumerable<T> source, Specification<T> specification)
    {
        foreach (var likeGroup in specification.LikeExpressions.GroupBy(x => x.Group))
        {
            source = source.Where(x => likeGroup.Any(c => c.KeySelectorFunc(x)?.Like(c.Pattern) ?? false));
        }

        return source;
    }
}
