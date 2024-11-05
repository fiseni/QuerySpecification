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

    // TODO: We should allow overwriting. Think about this. [fatii, 26/10/2024]
    //[Fact]
    //public void Evaluate_ThrowsConcurrentSelectorsException_GivenBothSelectAndSelectMany()
    //{
    //    var spec = new Specification<CustomerWithMails, string>();
    //    spec.Query
    //        .Select(x => x.FirstName);
    //    spec.Query
    //        .SelectMany(x => x.Emails);

    //    var sut = () => _evaluator.Evaluate([], spec);

    //    sut.Should().Throw<ConcurrentSelectorsException>();
    //}

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
    public void Constructor_SetsProvidedEvaluators()
    {
        var evaluators = new List<IInMemoryEvaluator>
        {
            WhereEvaluator.Instance,
            OrderEvaluator.Instance,
            WhereEvaluator.Instance,
        };

        var evaluator = new SpecificationInMemoryEvaluator(evaluators);

        var result = EvaluatorsOf(evaluator);
        result.Should().HaveSameCount(evaluators);
        result.Should().Equal(evaluators);
    }

    [Fact]
    public void DerivedSpecificationEvaluatorCanAlterDefaultEvaluator()
    {
        var evaluator = new SpecificationEvaluatorDerived();

        var result = EvaluatorsOf(evaluator);
        result.Should().HaveCount(5);
        result[0].Should().BeOfType<LikeMemoryEvaluator>();
        result[1].Should().BeOfType<WhereEvaluator>();
        result[2].Should().BeOfType<OrderEvaluator>();
        result[3].Should().BeOfType<LikeMemoryEvaluator>();
        result[4].Should().BeOfType<WhereEvaluator>();
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
