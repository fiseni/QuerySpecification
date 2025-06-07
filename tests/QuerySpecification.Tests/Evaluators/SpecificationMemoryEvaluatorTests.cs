using System.Runtime.CompilerServices;

namespace Tests.Evaluators;

public class SpecificationMemoryEvaluatorTests
{
    private static readonly SpecificationMemoryEvaluator _evaluator = SpecificationMemoryEvaluator.Default;

    public record Customer(int Id, string FirstName, string LastName);
    public record CustomerWithMails(int Id, string FirstName, string LastName, List<string> Emails);

    [Fact]
    public void Evaluate_ThrowsArgumentNullException_GivenNullSpec()
    {
        var sut = () => _evaluator.Evaluate([], (Specification<Customer>)null!);

        sut.Should().Throw<ArgumentNullException>().WithParameterName("specification");
    }

    [Fact]
    public void Evaluate_ThrowsArgumentNullException_GivenNullSpecificationWithSelector()
    {
        var sut = () => _evaluator.Evaluate([], (Specification<Customer, string>)null!);

        sut.Should().Throw<ArgumentNullException>().WithParameterName("specification");
    }

    [Fact]
    public void Evaluate_ThrowsSelectorNotFoundException_GivenNoSelector()
    {
        var spec = new Specification<Customer, string>();

        var sut = () => _evaluator.Evaluate([], spec);

        sut.Should().Throw<SelectorNotFoundException>();
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

        var actual = _evaluator.Evaluate(input, spec).ToList();
        var actualFromSpec = spec.Evaluate(input).ToList();

        actual.Should().Equal(actualFromSpec);
        actual.Should().Equal(expected);
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

        var actual = _evaluator.Evaluate(input, spec).ToList();
        var actualFromSpec = spec.Evaluate(input).ToList();

        actual.Should().Equal(actualFromSpec);
        actual.Should().Equal(expected);
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

        var actual = _evaluator.Evaluate(input, spec).ToList();
        var actualFromSpec = spec.Evaluate(input).ToList();

        actual.Should().Equal(actualFromSpec);
        actual.Should().Equal(expected);
    }

    [Fact]
    public void Evaluate_DoesNotFilter_GivenEmptySpec()
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
            new(1, "axxa", "axya"),
            new(2, "aaaa", "axya"),
            new(3, "aaaa", "axya"),
            new(4, "aaaa", "axya")
        ];

        var spec = new Specification<Customer>();

        var actual = _evaluator.Evaluate(input, spec).ToList();
        var actualFromSpec = spec.Evaluate(input).ToList();

        actual.Should().Equal(actualFromSpec);
        actual.Should().Equal(expected);
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

        List<Customer> expected =
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

        var actual = _evaluator.Evaluate(input, spec, ignorePaging: true).ToList();
        var actualFromSpec = spec.Evaluate(input, ignorePaging: true).ToList();

        actual.Should().Equal(actualFromSpec);
        actual.Should().Equal(expected);
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

        var actual = _evaluator.Evaluate(input, spec, ignorePaging: true).ToList();
        var actualFromSpec = spec.Evaluate(input, ignorePaging: true).ToList();

        actual.Should().Equal(actualFromSpec);
        actual.Should().Equal(expected);
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

        var actual = _evaluator.Evaluate(input, spec, true).ToList();
        var actualFromSpec = spec.Evaluate(input, true).ToList();

        actual.Should().Equal(actualFromSpec);
        actual.Should().Equal(expected);
    }

    [Fact]
    public void Constructor_SetsDefaultEvaluators()
    {
        var expectedEvaluators = new List<IMemoryEvaluator>
        {
            WhereEvaluator.Instance,
            LikeMemoryEvaluator.Instance,
            OrderEvaluator.Instance,
        };

        var evaluator = new SpecificationMemoryEvaluator();

        var result = EvaluatorsOf(evaluator);
        result.Should().HaveSameCount(expectedEvaluators);
        result.Should().Equal(expectedEvaluators);
    }

    [Fact]
    public void Constructor_SetsProvidedEvaluators()
    {
        var evaluators = new List<IMemoryEvaluator>
        {
            WhereEvaluator.Instance,
            OrderEvaluator.Instance,
            WhereEvaluator.Instance,
        };

        var evaluator = new SpecificationMemoryEvaluator(evaluators);

        var result = EvaluatorsOf(evaluator);
        result.Should().HaveSameCount(evaluators);
        result.Should().Equal(evaluators);
    }

    [Fact]
    public void DerivedSpecificationEvaluatorCanAlterDefaultEvaluators()
    {
        var expectedEvaluators = new List<IMemoryEvaluator>
        {
            LikeMemoryEvaluator.Instance,
            WhereEvaluator.Instance,
            LikeMemoryEvaluator.Instance,
            OrderEvaluator.Instance,
            WhereEvaluator.Instance
        };

        var evaluator = new SpecificationEvaluatorDerived();

        var result = EvaluatorsOf(evaluator);
        result.Should().Equal(expectedEvaluators);
    }

    private class SpecificationEvaluatorDerived : SpecificationMemoryEvaluator
    {
        public SpecificationEvaluatorDerived()
        {
            Evaluators.Add(WhereEvaluator.Instance);
            Evaluators.Insert(0, LikeMemoryEvaluator.Instance);
        }
    }

    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "<Evaluators>k__BackingField")]
    public static extern ref List<IMemoryEvaluator> EvaluatorsOf(SpecificationMemoryEvaluator @this);
}
