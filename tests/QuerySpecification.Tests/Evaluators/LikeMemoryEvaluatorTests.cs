namespace Tests.Evaluators;

public class LikeMemoryEvaluatorTests
{
    private static readonly LikeMemoryEvaluator _evaluator = LikeMemoryEvaluator.Instance;

    public record Customer(int Id, string FirstName, string? LastName);

    [Fact]
    public void Filters_GivenSingleLike()
    {
        List<Customer> input =
        [
            new(1, "axxa", "axya"),
            new(2, "aaaa", "aaaa"),
            new(3, "aaaa", "axya"),
            new(4, "aaaa", null)
        ];

        List<Customer> expected =
        [
            new(2, "aaaa", "aaaa"),
        ];

        var spec = new Specification<Customer>();
        spec.Query
            .Like(x => x.LastName, "%aa%");

        // Not materializing with ToList() intentionally to test cloning in the iterator
        var actual = _evaluator.Evaluate(input, spec);

        // Multiple iterations will force cloning
        actual.Should().HaveSameCount(expected);
        actual.Should().Equal(expected);
    }

    [Fact]
    public void Filters_GivenLikeInSameGroup()
    {
        List<Customer> input =
        [
            new(1, "axxa", "axya"),
            new(2, "aaaa", "aaaa"),
            new(3, "aaaa", "axya"),
            new(4, "aaaa", null)
        ];

        List<Customer> expected =
        [
            new(1, "axxa", "axya"),
            new(3, "aaaa", "axya")
        ];

        var spec = new Specification<Customer>();
        spec.Query
            .Like(x => x.FirstName, "%xx%")
            .Like(x => x.LastName, "%xy%");

        // Not materializing with ToList() intentionally to test cloning in the iterator
        var actual = _evaluator.Evaluate(input, spec);

        // Multiple iterations will force cloning
        actual.Should().HaveSameCount(expected);
        actual.Should().Equal(expected);
    }

    [Fact]
    public void Filters_GivenLikeInDifferentGroup()
    {
        List<Customer> input =
        [
            new(1, "axxa", "axya"),
            new(2, "aaaa", "aaaa"),
            new(3, "aaaa", "axya"),
            new(4, "aaaa", null)
        ];

        List<Customer> expected =
        [
            new(1, "axxa", "axya")
        ];

        var spec = new Specification<Customer>();
        spec.Query
            .Like(x => x.FirstName, "%xx%", 1)
            .Like(x => x.LastName, "%xy%", 2);

        var actual = _evaluator.Evaluate(input, spec).ToList();

        actual.Should().Equal(expected);
    }

    [Fact]
    public void Filters_GivenLikeComplexGrouping()
    {
        List<Customer> input =
        [
            new(1, "axxa", "axya"),
            new(2, "aaaa", "aaaa"),
            new(3, "axxa", "axza"),
            new(4, "aaaa", null),
            new(5, "axxa", null)
        ];

        List<Customer> expected =
        [
            new(1, "axxa", "axya"),
            new(3, "axxa", "axza"),
        ];

        var spec = new Specification<Customer>();
        spec.Query
            .Like(x => x.FirstName, "%xx%", 1)
            .Like(x => x.LastName, "%xy%", 2)
            .Like(x => x.LastName, "%xz%", 2);

        var actual = _evaluator.Evaluate(input, spec).ToList();

        actual.Should().Equal(expected);
    }

    [Fact]
    public void DoesNotFilter_GivenNoLike()
    {
        List<Customer> input =
        [
            new(1, "axxa", "axya"),
            new(2, "aaaa", "aaaa"),
            new(3, "aaaa", "axya")
        ];

        List<Customer> expected =
        [
            new(1, "axxa", "axya"),
            new(2, "aaaa", "aaaa"),
            new(3, "aaaa", "axya")
        ];

        var spec = new Specification<Customer>();
        spec.Query
            .Where(x => x.Id > 0);

        var actual = _evaluator.Evaluate(input, spec);

        actual.Should().Equal(expected);
    }

    [Fact]
    public void DoesNotFilter_GivenEmptySpec()
    {
        List<Customer> input =
        [
            new(1, "axxa", "axya"),
            new(2, "aaaa", "aaaa"),
            new(3, "aaaa", "axya")
        ];

        List<Customer> expected =
        [
            new(1, "axxa", "axya"),
            new(2, "aaaa", "aaaa"),
            new(3, "aaaa", "axya")
        ];

        var spec = new Specification<Customer>();

        var actual = _evaluator.Evaluate(input, spec);

        actual.Should().Equal(expected);
    }
}
