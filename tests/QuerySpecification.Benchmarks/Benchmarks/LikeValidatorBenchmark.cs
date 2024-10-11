namespace QuerySpecification.Benchmarks;

// Benchmarks measuring the in-memory Like evaluator implementations.
[MemoryDiagnoser]
public class LikeValidatorBenchmark
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
    private Customer _customer = default!;

    [GlobalSetup]
    public void Setup()
    {
        _specification = new CustomerSpec();
        _customer = new(1, "axxa", "axza");
    }

    [Benchmark(Baseline = true)]
    public bool ValidateOption1()
    {
        var entity = _customer;

        var groups = _specification.LikeExpressions.GroupBy(x => x.Group);

        foreach (var likeGroup in groups)
        {
            if (likeGroup.Any(c => c.KeySelectorFunc(entity)?.Like(c.Pattern) ?? false) == false) return false;
        }

        return true;
    }

    [Benchmark]
    public bool ValidateOption2()
    {
        var entity = _customer;

        var groups = _specification.LikeExpressions.GroupBy(x => x.Group).ToList();

        foreach (var group in groups)
        {
            var match = false;
            foreach (var like in group)
            {
                if (like.KeySelectorFunc(entity)?.Like(like.Pattern) ?? false)
                {
                    match = true;
                    break;
                }
            }

            if (match is false)
                return false;
        }

        return true;
    }

    [Benchmark]
    public bool ValidateOption3()
    {
        var entity = _customer;

        var groups = _specification.LikeExpressions.GroupBy(x => x.Group);

        foreach (var group in groups)
        {
            var match = false;
            foreach (var like in group)
            {
                if (like.KeySelectorFunc(entity)?.Like(like.Pattern) ?? false)
                {
                    match = true;
                    break;
                }
            }

            if (match is false)
                return false;
        }

        return true;
    }
}
