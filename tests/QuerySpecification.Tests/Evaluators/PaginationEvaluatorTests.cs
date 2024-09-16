namespace QuerySpecification.Tests.Evaluators;

public class PaginationEvaluatorTests
{
    private static readonly PaginationEvaluator _evaluator = PaginationEvaluator.Instance;

    public record Customer(int Id);

    [Fact]
    public void WithPagination_ReturnsFilteredItems()
    {
        List<Customer> input = [new(1), new(2), new(3), new(4), new(5)];
        List<Customer> expected = [new(3), new(4)];

        var spec = new Specification<Customer>();
        spec.Query
            .Skip(2)
            .Take(2);

        AssertForEvaluate(spec, input, expected);
        AssertForGetQuery(spec, input, expected);
    }

    [Fact]
    public void WithoutPagination_ReturnsNonFilteredItems()
    {
        List<Customer> input = [new(1), new(2), new(3), new(4), new(5)];
        List<Customer> expected = [new(1), new(2), new(3), new(4), new(5)];

        var spec = new Specification<Customer>();

        AssertForEvaluate(spec, input, expected);
        AssertForGetQuery(spec, input, expected);
    }


    [Fact]
    public void WithNegativeTakeSkip_ReturnsNonFilteredItems()
    {
        List<Customer> input = [new(1), new(2), new(3), new(4), new(5)];
        List<Customer> expected = [new(1), new(2), new(3), new(4), new(5)];

        var spec = new Specification<Customer>();
        spec.Query
            .Skip(-1)
            .Take(-1);

        AssertForEvaluate(spec, input, expected);
        AssertForGetQuery(spec, input, expected);
    }


    [Fact]
    public void WithZeroSkip_ReturnsNonFilteredItems()
    {
        List<Customer> input = [new(1), new(2), new(3), new(4), new(5)];
        List<Customer> expected = [new(1), new(2), new(3), new(4), new(5)];

        var spec = new Specification<Customer>();
        spec.Query
            .Skip(0);

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
