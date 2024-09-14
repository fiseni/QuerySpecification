namespace Pozitron.QuerySpecification.Tests.Evaluators;

public class WhereEvaluatorTests
{
    private static readonly WhereEvaluator _evaluator = WhereEvaluator.Instance;

    public record Customer(int Id);

    [Fact]
    public void WithWhereExpression_ReturnsFilteredItems()
    {
        List<Customer> input = [new(1), new(2), new(3), new(4), new(5)];
        List<Customer> expected = [new(4), new(5)];

        var spec = new Specification<Customer>();
        spec.Query
            .Where(x => x.Id > 3);

        AssertForEvaluate(spec, input, expected);
        AssertForGetQuery(spec, input, expected);
    }

    [Fact]
    public void WithoutWhereExpression_ReturnsNonFilteredItems()
    {
        List<Customer> input = [new(1), new(2), new(3), new(4), new(5)];
        List<Customer> expected = [new(1), new(2), new(3), new(4), new(5)];

        var spec = new Specification<Customer>();

        AssertForEvaluate(spec, input, expected);
        AssertForGetQuery(spec, input, expected);
    }

    private static void AssertForEvaluate<T>(Specification<T> spec, List<T> input, IEnumerable<T> expected)
    {
        var actual = _evaluator.Evaluate(input, spec);

        actual.Should().NotBeNull();
        actual.Should().HaveSameCount(expected);
        actual.Should().Equal(expected);
    }

    private static void AssertForGetQuery<T>(Specification<T> spec, List<T> input, IEnumerable<T> expected) where T : class
    {
        var actual = _evaluator.GetQuery(input.AsQueryable(), spec);

        actual.Should().NotBeNull();
        actual.Should().HaveSameCount(expected);
        actual.Should().Equal(expected);
    }
}
