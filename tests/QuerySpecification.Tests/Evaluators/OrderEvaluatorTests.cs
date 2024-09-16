namespace QuerySpecification.Tests.Evaluators;

public class OrderEvaluatorTests
{
    private static readonly OrderEvaluator _evaluator = OrderEvaluator.Instance;

    public record Customer(int Id, string? Name = null);

    [Fact]
    public void WithOrderBy_ReturnsAscendingItems()
    {
        List<Customer> input = [new(3), new(1), new(2), new(5), new(4)];
        List<Customer> expected = [new(1), new(2), new(3), new(4), new(5)];

        var spec = new Specification<Customer>();
        spec.Query
            .OrderBy(x => x.Id);

        AssertForEvaluate(spec, input, expected);
        AssertForGetQuery(spec, input, expected);
    }

    [Fact]
    public void WithOrderByDescending_ReturnsDescendingItems()
    {
        List<Customer> input = [new(3), new(1), new(2), new(5), new(4)];
        List<Customer> expected = [new(5), new(4), new(3), new(2), new(1)];

        var spec = new Specification<Customer>();
        spec.Query
            .OrderByDescending(x => x.Id);

        AssertForEvaluate(spec, input, expected);
        AssertForGetQuery(spec, input, expected);
    }

    [Fact]
    public void WithOrderByThenBy_ReturnsOrderedItems()
    {
        List<Customer> input = [new(3, "c"), new(1, "b"), new(1, "a")];
        List<Customer> expected = [new(1, "a"), new(1, "b"), new(3, "c")];

        var spec = new Specification<Customer>();
        spec.Query
            .OrderBy(x => x.Id)
            .ThenBy(x => x.Name);

        AssertForEvaluate(spec, input, expected);
        AssertForGetQuery(spec, input, expected);
    }

    [Fact]
    public void WithOrderByThenByDescending_ReturnsOrderedItems()
    {
        List<Customer> input = [new(3, "c"), new(1, "a"), new(1, "b")];
        List<Customer> expected = [new(1, "b"), new(1, "a"), new(3, "c")];

        var spec = new Specification<Customer>();
        spec.Query
            .OrderBy(x => x.Id)
            .ThenByDescending(x => x.Name);

        AssertForEvaluate(spec, input, expected);
        AssertForGetQuery(spec, input, expected);
    }

    [Fact]
    public void WithOrderByDescendingThenBy_ReturnsOrderedItems()
    {
        List<Customer> input = [new(1, "b"), new(1, "a"), new(3, "c")];
        List<Customer> expected = [new(3, "c"), new(1, "a"), new(1, "b")];

        var spec = new Specification<Customer>();
        spec.Query
            .OrderByDescending(x => x.Id)
            .ThenBy(x => x.Name);

        AssertForEvaluate(spec, input, expected);
        AssertForGetQuery(spec, input, expected);
    }

    [Fact]
    public void WithOrderByDescendingThenByDescending_ReturnsOrderedItems()
    {
        List<Customer> input = [new(1, "a"), new(1, "b"), new(3, "c")];
        List<Customer> expected = [new(3, "c"), new(1, "b"), new(1, "a")];

        var spec = new Specification<Customer>();
        spec.Query
            .OrderByDescending(x => x.Id)
            .ThenByDescending(x => x.Name);

        AssertForEvaluate(spec, input, expected);
        AssertForGetQuery(spec, input, expected);
    }

    [Fact]
    public void WithoutOrder_ReturnsNonOrderedItems()
    {
        List<Customer> input = [new(3), new(1), new(2), new(5), new(4)];
        List<Customer> expected = [new(3), new(1), new(2), new(5), new(4)];
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
