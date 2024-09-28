using System.Runtime.CompilerServices;

namespace Tests.Evaluators;

public class SpecificationInMemoryEvaluatorTests
{
    private static readonly SpecificationInMemoryEvaluator _evaluator = SpecificationInMemoryEvaluator.Default;

    public record Customer(int Id, string FirstName, string LastName);
    public record CustomerWithMails(int Id, string FirstName, string LastName, List<string> Emails);

    [Fact]
    public void Evaluate_ThrowsArgumentNullException_GivenNullSpec()
    {
        Action sutAction = () => _evaluator.Evaluate([], (Specification<Customer>)null!);

        sutAction.Should().Throw<ArgumentNullException>().WithParameterName("specification");
    }

    [Fact]
    public void Evaluate_ThrowsArgumentNullException_GivenNullSpecificationWithSelector()
    {
        Action sutAction = () => _evaluator.Evaluate([], (Specification<Customer, string>)null!);

        sutAction.Should().Throw<ArgumentNullException>().WithParameterName("specification");
    }

    [Fact]
    public void Evaluate_ThrowsSelectorNotFoundException_GivenNoSelector()
    {
        var spec = new Specification<Customer, string>();

        Action sutAction = () => _evaluator.Evaluate([], spec);

        sutAction.Should().Throw<SelectorNotFoundException>();
    }

    [Fact]
    public void Evaluate_ThrowsConcurrentSelectorsException_GivenBothSelectAndSelectMany()
    {
        var spec = new Specification<CustomerWithMails, string>();
        spec.Query
            .Select(x => x.FirstName);
        spec.Query
            .SelectMany(x => x.Emails);

        Action sutAction = () => _evaluator.Evaluate([], spec);

        sutAction.Should().Throw<ConcurrentSelectorsException>();
    }

    [Fact]
    public void Evaluate_Filters_GivenSpec()
    {
        List<Customer> input =
        [
            new(1, "axxa", "axya"),
            new(2, "aaaa", "axya"),
            new(3, "aaaa", "axya"),
            new(4, "aaaa", "axya")
        ];

        List<Customer> expected =
        [
            new(3, "aaaa", "axya")
        ];

        var spec = new Specification<Customer>();
        spec.Query
            .Where(x => x.Id > 1)
            .Like(x => x.LastName, "%xy%")
            .OrderBy(x => x.Id)
            .Skip(1)
            .Take(1);

        AssertForEvaluate(spec, input, expected);
    }

    [Fact]
    public void Evaluate_Filters_GivenSpecWithSelect()
    {
        List<Customer> input =
        [
            new(1, "axxa", "axya"),
            new(2, "aaaa", "axya"),
            new(3, "vvvv", "axya"),
            new(4, "aaaa", "axya")
        ];

        List<string> expected = ["vvvv"];

        var spec = new Specification<Customer, string>();
        spec.Query
            .Where(x => x.Id > 1)
            .Like(x => x.LastName, "%xy%")
            .OrderBy(x => x.Id)
            .Skip(1)
            .Take(1)
            .Select(x => x.FirstName);

        AssertForEvaluate(spec, input, expected);
    }

    [Fact]
    public void Evaluate_Filters_GivenSpecWithSelectMany()
    {
        List<CustomerWithMails> input =
        [
            new(1, "axxa", "axya", []),
            new(2, "aaaa", "axya", []),
            new(3, "aaaa", "axya", ["zzz", "www"]),
            new(4, "aaaa", "axya", ["yyy"])
        ];

        List<string> expected = ["www", "yyy"];

        var spec = new Specification<CustomerWithMails, string>();
        spec.Query
            .Where(x => x.Id > 1)
            .Like(x => x.LastName, "%xy%")
            .OrderBy(x => x.Id)
            .Skip(1)
            .Take(2)
            .SelectMany(x => x.Emails);

        AssertForEvaluate(spec, input, expected);
    }

    [Fact]
    public void Evaluate_DoesNotFilter_GivenSpecAndIgnorePagination()
    {
        List<Customer> input =
        [
            new(1, "axxa", "axya"),
            new(2, "aaaa", "axya"),
            new(3, "aaaa", "axya"),
            new(4, "aaaa", "axya")
        ];

        var spec = new Specification<Customer>();
        spec.Query
            .OrderBy(x => x.Id)
            .Skip(1)
            .Take(1);

        AssertForEvaluate(spec, input, input, ignorePaging: true);
    }

    [Fact]
    public void Evaluate_DoesNotFilter_GivenSpecWithSelectAndIgnorePagination()
    {
        List<Customer> input =
        [
            new(1, "axxa", "axya"),
            new(2, "aaaa", "axya"),
            new(3, "vvvv", "axya"),
            new(4, "aaaa", "axya")
        ];

        List<string> expected = ["axxa", "aaaa", "vvvv", "aaaa"];

        var spec = new Specification<Customer, string>();
        spec.Query
            .OrderBy(x => x.Id)
            .Skip(1)
            .Take(1)
            .Select(x => x.FirstName);

        AssertForEvaluate(spec, input, expected, ignorePaging: true);
    }

    [Fact]
    public void Evaluate_DoesNotFilter_GivenSpecWithSelectManyAndIgnorePagination()
    {
        List<CustomerWithMails> input =
        [
            new(1, "axxa", "axya", []),
            new(2, "aaaa", "axya", []),
            new(3, "aaaa", "axya", ["zzz", "www"]),
            new(4, "aaaa", "axya", ["yyy"])
        ];

        List<string> expected = ["zzz", "www", "yyy"];

        var spec = new Specification<CustomerWithMails, string>();
        spec.Query
            .OrderBy(x => x.Id)
            .Skip(1)
            .Take(2)
            .SelectMany(x => x.Emails);

        AssertForEvaluate(spec, input, expected, ignorePaging: true);
    }

    private static void AssertForEvaluate<T>(
        Specification<T> spec,
        List<T> input,
        IEnumerable<T> expected,
        bool ignorePaging = false)
    {
        var actual = _evaluator.Evaluate(input, spec, ignorePaging);
        var actualFromSpec = spec.Evaluate(input, ignorePaging);

        actual.Should().Equal(actualFromSpec);
        actual.Should().NotBeNull();
        actual.Should().HaveSameCount(expected);
        actual.Should().Equal(expected);
    }

    private static void AssertForEvaluate<T, TResult>(
        Specification<T, TResult> spec,
        List<T> input,
        IEnumerable<TResult> expected,
        bool ignorePaging = false)
    {
        var actual = _evaluator.Evaluate(input, spec, ignorePaging);
        var actualFromSpec = spec.Evaluate(input, ignorePaging);

        actual.Should().Equal(actualFromSpec);
        actual.Should().NotBeNull();
        actual.Should().HaveSameCount(expected);
        actual.Should().Equal(expected);
    }

    [Fact]
    public void Constructor_SetsProvidedEvaluators()
    {
        var evaluators = new List<IInMemoryEvaluator>
        {
            WhereEvaluator.Instance,
            OrderEvaluator.Instance,
            WhereEvaluator.Instance,
        };

        var evaluator = new SpecificationInMemoryEvaluator(evaluators);

        var state = EvaluatorsOf(evaluator);
        state.Should().HaveSameCount(evaluators);
        state.Should().Equal(evaluators);
    }

    [Fact]
    public void DerivedSpecificationEvaluatorCanAlterDefaultEvaluator()
    {
        var evaluator = new SpecificationEvaluatorDerived();

        var state = EvaluatorsOf(evaluator);
        state.Should().HaveCount(5);
        state[0].Should().BeOfType<LikeMemoryEvaluator>();
        state[1].Should().BeOfType<WhereEvaluator>();
        state[2].Should().BeOfType<LikeMemoryEvaluator>();
        state[3].Should().BeOfType<OrderEvaluator>();
        state[4].Should().BeOfType<WhereEvaluator>();
    }

    private class SpecificationEvaluatorDerived : SpecificationInMemoryEvaluator
    {
        public SpecificationEvaluatorDerived()
        {
            Evaluators.Add(WhereEvaluator.Instance);
            Evaluators.Insert(0, LikeMemoryEvaluator.Instance);
        }
    }

    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "<Evaluators>k__BackingField")]
    public static extern ref List<IInMemoryEvaluator> EvaluatorsOf(SpecificationInMemoryEvaluator @this);
}
