namespace Tests.Evaluators;

public class LikeMemoryEvaluatorTests
{
    private static readonly LikeMemoryEvaluator _evaluator = LikeMemoryEvaluator.Instance;

    public record Customer(int Id, string FirstName, string? LastName);

    [Fact]
    public void FiltersItems_GivenLikeInSameGroup()
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

        AssertForEvaluate(spec, input, expected);
    }

    [Fact]
    public void FiltersItems_GivenLikeInDifferentGroup()
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

        AssertForEvaluate(spec, input, expected);
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

        AssertForEvaluate(spec, input, expected);
    }

    private static void AssertForEvaluate<T>(Specification<T> spec, List<T> input, IEnumerable<T> expected)
    {
        var actual = _evaluator.Evaluate(input, spec);

        actual.Should().NotBeNull();
        actual.Should().HaveSameCount(expected);
        actual.Should().Equal(expected);
    }
}
