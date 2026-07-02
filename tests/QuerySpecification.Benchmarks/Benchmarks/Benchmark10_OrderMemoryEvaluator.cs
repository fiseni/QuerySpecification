using System.Linq.Expressions;

namespace QuerySpecification.Benchmarks;

[MemoryDiagnoser]
public class Benchmark10_OrderMemoryEvaluator
{
    /* Measures in-memory ordering through the spec evaluator.
     * Order expressions are stored as Expression<Func<T, object?>>. Compiling them directly boxes every
     * value-type key during sorting and compares through Comparer<object>. The evaluator strips the boxing
     * Convert node and compiles a typed key selector instead (see SpecOrderCompiled), so sorting runs with
     * Comparer<TKey> and no per-element allocations.
     * BoxedBaseline represents the previous implementation; TypedLinq is the ideal hand-written LINQ.
     */

    private List<Customer> _source = default!;
    private CustomerSpec _spec = default!;
    private Func<Customer, object?> _boxedIdSelector = default!;
    private Func<Customer, object?> _boxedNameSelector = default!;

    [Params(100, 1000)]
    public int Rows { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _source = Enumerable.Range(1, Rows)
            .Select(x => new Customer(Rows - x, $"Name {x % 10}"))
            .ToList();

        _spec = new CustomerSpec();

        Expression<Func<Customer, object?>> boxedId = x => x.Id;
        Expression<Func<Customer, object?>> boxedName = x => x.Name;
        _boxedIdSelector = boxedId.Compile();
        _boxedNameSelector = boxedName.Compile();
    }

    [Benchmark(Baseline = true)]
    public int BoxedBaseline()
    {
        var result = 0;
        foreach (var item in _source.OrderBy(_boxedIdSelector).ThenByDescending(_boxedNameSelector))
        {
            result = item.Id;
        }
        return result;
    }

    [Benchmark]
    public int TypedLinq()
    {
        var result = 0;
        foreach (var item in _source.OrderBy(x => x.Id).ThenByDescending(x => x.Name))
        {
            result = item.Id;
        }
        return result;
    }

    [Benchmark]
    public int Evaluate_v11()
    {
        var result = 0;
        foreach (var item in _spec.Evaluate(_source))
        {
            result = item.Id;
        }
        return result;
    }

    private record Customer(int Id, string Name);

    private class CustomerSpec : Specification<Customer>
    {
        public CustomerSpec()
        {
            Query
                .OrderBy(x => x.Id)
                .ThenByDescending(x => x.Name);
        }
    }
}
