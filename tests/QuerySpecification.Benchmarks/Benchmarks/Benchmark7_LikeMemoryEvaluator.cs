namespace QuerySpecification.Benchmarks;

[MemoryDiagnoser]
public class Benchmark7_LikeMemoryEvaluator
{
    private List<Customer> _source = default!;
    private CustomerSpec _specification = default!;
    private LikeMemoryEvaluatorOriginal<Customer> _evaluatorOriginal = default!;
    private LikeMemoryEvaluatorV10<Customer> _evaluatorV10 = default!;
    private LikeMemoryEvaluator _evaluatorV11 = default!;

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
        _evaluatorV11 = LikeMemoryEvaluator.Instance;

        // We'll help out the old implementations by even providing ready-to-use list (usually that happens in the evaluators)
        var likeExpressionsCompiled = _specification.LikeExpressionsCompiled.ToList();
        _evaluatorOriginal = new LikeMemoryEvaluatorOriginal<Customer>(likeExpressionsCompiled);
        _evaluatorV10 = new LikeMemoryEvaluatorV10<Customer>(likeExpressionsCompiled);
    }

    [Benchmark(Baseline = true)]
    public int EvaluateOriginal()
    {
        var evaluator = _evaluatorOriginal;
        var result = evaluator.Evaluate(_source, _specification);
        return result.Count();
    }

    [Benchmark]
    public int Evaluate_v10()
    {
        var evaluator = _evaluatorV10;
        var result = evaluator.Evaluate(_source, _specification);
        return result.Count();
    }

    [Benchmark]
    public int Evaluate_v11()
    {
        var evaluator = _evaluatorV11;
        var result = evaluator.Evaluate(_source, _specification);
        return result.Count();
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

    private sealed class LikeMemoryEvaluatorOriginal<T>(List<LikeExpressionCompiled<T>> likeExpressionsCompiled)
    {
        public IEnumerable<T> Evaluate(IEnumerable<T> source, Specification<T> specification)
        {
            foreach (var likeGroup in likeExpressionsCompiled.GroupBy(x => x.Group))
            {
                source = source.Where(x => likeGroup.Any(c => c.KeySelector(x)?.Like(c.Pattern) ?? false));
            }

            return source;
        }
    }

    private sealed class LikeMemoryEvaluatorV10<T>(List<LikeExpressionCompiled<T>> likeExpressionsCompiled)
    {
        public IEnumerable<T> Evaluate(IEnumerable<T> source, Specification<T> specification)
        {
            var groups = likeExpressionsCompiled.GroupBy(x => x.Group).ToList();

            foreach (var item in source)
            {
                var match = true;
                foreach (var group in groups)
                {
                    var matchOrGroup = false;
                    foreach (var like in group)
                    {
                        if (like.KeySelector(item)?.Like(like.Pattern) ?? false)
                        {
                            matchOrGroup = true;
                            break;
                        }
                    }

                    if ((match = match && matchOrGroup) is false)
                    {
                        break;
                    }
                }

                if (match)
                {
                    yield return item;
                }
            }
        }
    }
}

