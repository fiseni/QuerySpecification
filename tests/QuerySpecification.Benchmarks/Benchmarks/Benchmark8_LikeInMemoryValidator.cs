namespace QuerySpecification.Benchmarks;

[MemoryDiagnoser]
public class Benchmark8_LikeInMemoryValidator
{
    private List<Customer> _source = default!;
    private CustomerSpec _specification = default!;
    private LikeValidatorOriginal<Customer> _validatorOriginal = default!;
    private LikeValidatorV10<Customer> _validatorV10 = default!;
    private LikeValidator _validatorV11 = default!;

    [GlobalSetup]
    public void Setup()
    {
        _source =
        [
            new(1, "axxa", "axya"),
            new(2, "aaaa", "aaaa"),
            new(3, "axxa", "axza"),
            new(4, "aaaa", null),
            new(5, "axxa", null),
            .. Enumerable.Range(6, 1000).Select(x => new Customer(x, "axxa", "axya"))
        ];

        _specification = new CustomerSpec();
        _validatorV11 = LikeValidator.Instance;

        // We'll help out the old implementations by even providing ready-to-use list (usually that happens in the validators)
        var likeExpressionsCompiled = _specification.LikeExpressionsCompiled.ToList();
        _validatorOriginal = new LikeValidatorOriginal<Customer>(likeExpressionsCompiled);
        _validatorV10 = new LikeValidatorV10<Customer>(likeExpressionsCompiled);
    }

    [Benchmark(Baseline = true)]
    public bool ValidateOriginal()
    {
        var validator = _validatorOriginal;

        var result = false;
        foreach (var item in _source)
        {
            result = validator.IsValid(item, _specification);
        }
        return result;
    }

    [Benchmark]
    public bool Validate_v10()
    {
        var validator = _validatorV10;

        var result = false;
        foreach (var item in _source)
        {
            result = validator.IsValid(item, _specification);
        }
        return result;
    }

    [Benchmark]
    public bool Validate_v11()
    {
        var validator = _validatorV11;

        var result = false;
        foreach (var item in _source)
        {
            result = validator.IsValid(item, _specification);
        }
        return result;
    }

    private record Customer(int Id, string FirstName, string? LastName);
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

    private sealed class LikeValidatorOriginal<T>(List<LikeExpressionCompiled<T>> likeExpressionsCompiled)
    {
        public bool IsValid(T entity, Specification<T> specification)
        {
            foreach (var likeGroup in likeExpressionsCompiled.GroupBy(x => x.Group))
            {
                if (likeGroup.Any(c => c.KeySelector(entity)?.Like(c.Pattern) ?? false) == false) return false;
            }

            return true;
        }
    }

    private sealed class LikeValidatorV10<T>(List<LikeExpressionCompiled<T>> likeExpressionsCompiled)
    {
        public bool IsValid(T entity, Specification<T> specification)
        {
            var groups = likeExpressionsCompiled.GroupBy(x => x.Group);

            foreach (var group in groups)
            {
                var match = false;
                foreach (var like in group)
                {
                    if (like.KeySelector(entity)?.Like(like.Pattern) ?? false)
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
}

