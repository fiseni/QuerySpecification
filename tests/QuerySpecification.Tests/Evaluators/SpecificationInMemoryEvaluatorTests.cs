using System.Runtime.CompilerServices;

namespace Pozitron.QuerySpecification.Tests.Evaluators;

public class SpecificationInMemoryEvaluatorTests
{
    private static readonly SpecificationInMemoryEvaluator _evaluator = SpecificationInMemoryEvaluator.Default;

    public record Customer(int Id, string FirstName, string LastName, List<string>? Emails = null);

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
        var spec = new Specification<Customer, string>();
        spec.Query
            .Select(x => x.FirstName);
        spec.Query
            .SelectMany(x => x.Emails);

        Action sutAction = () => _evaluator.Evaluate([], spec);

        sutAction.Should().Throw<ConcurrentSelectorsException>();
    }

    [Fact]
    public void Evaluate_Given_ReturnsFilteredItems()
    {
        List<Customer> input = [new(1, "axxa", "axya"), new(2, "aaaa", "axya"), new(3, "aaaa", "axya"), new(4, "aaaa", "axya")];
        List<Customer> expected = [new(3, "aaaa", "axya")];

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
    public void Evaluate_GivenSpecWithSelect_ReturnsFilteredItems()
    {
        List<Customer> input = [new(1, "axxa", "axya"), new(2, "aaaa", "axya"), new(3, "vvvv", "axya"), new(4, "aaaa", "axya")];
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
    public void Evaluate_GivenSpecWithSelectMany_ReturnsFilteredItems()
    {
        List<Customer> input = [new(1, "axxa", "axya"), new(2, "aaaa", "axya"), new(3, "aaaa", "axya", ["zzz"]), new(4, "aaaa", "axya")];
        List<string> expected = ["zzz"];

        var spec = new Specification<Customer, string>();
        spec.Query
            .Where(x => x.Id > 1)
            .Like(x => x.LastName, "%xy%")
            .OrderBy(x => x.Id)
            .Skip(1)
            .Take(1)
            .SelectMany(x => x.Emails);

        AssertForEvaluate(spec, input, expected);
    }

    private static void AssertForEvaluate<T>(Specification<T> spec, List<T> input, IEnumerable<T> expected)
    {
        var actual = _evaluator.Evaluate(input, spec);
        var actualFromSpec = spec.Evaluate(input);

        actual.Should().Equal(actualFromSpec);
        actual.Should().NotBeNull();
        actual.Should().HaveSameCount(expected);
        actual.Should().Equal(expected);
    }

    private static void AssertForEvaluate<T, TResult>(Specification<T, TResult> spec, List<T> input, IEnumerable<TResult> expected)
    {
        var actual = _evaluator.Evaluate(input, spec);
        var actualFromSpec = spec.Evaluate(input);

        actual.Should().Equal(actualFromSpec);
        actual.Should().NotBeNull();
        actual.Should().HaveSameCount(expected);
        actual.Should().Equal(expected);
    }

    [Fact]
    public void ConstructorSetsProvidedEvaluators()
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
        state.Should().HaveCount(6);
        state[0].Should().BeOfType<LikeEvaluator>();
        state[1].Should().BeOfType<WhereEvaluator>();
        state[2].Should().BeOfType<LikeEvaluator>();
        state[3].Should().BeOfType<OrderEvaluator>();
        state[4].Should().BeOfType<PaginationEvaluator>();
        state[5].Should().BeOfType<WhereEvaluator>();
    }

    private class SpecificationEvaluatorDerived : SpecificationInMemoryEvaluator
    {
        public SpecificationEvaluatorDerived()
        {
            Evaluators.Add(WhereEvaluator.Instance);
            Evaluators.Insert(0, LikeEvaluator.Instance);
        }
    }

    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "<Evaluators>k__BackingField")]
    public static extern ref List<IInMemoryEvaluator> EvaluatorsOf(SpecificationInMemoryEvaluator @this);
}
