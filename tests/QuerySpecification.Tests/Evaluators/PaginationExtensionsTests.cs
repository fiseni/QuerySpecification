namespace Tests.Evaluators;

public class PaginationExtensionsTests
{
    public record Customer(int Id);

    [Fact]
    public void Filters_GivenPaginatedSpec()
    {
        List<Customer> input = [new(1), new(2), new(3), new(4), new(5)];
        List<Customer> expected = [new(3), new(4)];

        var spec = new Specification<Customer>();
        spec.Query
            .Skip(2)
            .Take(2);

        var actual = input.ApplyPaging(spec);
        Assert(actual, expected);

        actual = input.AsQueryable().ApplyPaging(spec);
        Assert(actual, expected);

        var pagination = new Pagination(input.Count, 2, 2);
        actual = input.AsQueryable().ApplyPaging(pagination);
        Assert(actual, expected);
    }

    [Fact]
    public void Filters_GivenPaginatedSpecWithSelect()
    {
        List<Customer> input = [new(1), new(2), new(3), new(4), new(5)];
        List<Customer> expected = [new(3), new(4)];

        var spec = new Specification<Customer, int>();
        spec.Query
            .Skip(2)
            .Take(2)
            .Select(x => x.Id);

        var actual = input.ApplyPaging(spec);
        Assert(actual, expected);

        actual = input.AsQueryable().ApplyPaging(spec);
        Assert(actual, expected);
    }

    [Fact]
    public void DoesNotFilter_GivenEmptySpec()
    {
        List<Customer> input = [new(1), new(2), new(3), new(4), new(5)];
        List<Customer> expected = [new(1), new(2), new(3), new(4), new(5)];

        var spec = new Specification<Customer>();

        var actual = input.ApplyPaging(spec);
        Assert(actual, expected);

        actual = input.AsQueryable().ApplyPaging(spec);
        Assert(actual, expected);
    }

    [Fact]
    public void DoesNotFilter_GivenNegativeTakeSkip()
    {
        List<Customer> input = [new(1), new(2), new(3), new(4), new(5)];
        List<Customer> expected = [new(1), new(2), new(3), new(4), new(5)];

        var spec = new Specification<Customer>();
        spec.Query
            .Skip(-1)
            .Take(-1);

        var actual = input.ApplyPaging(spec);
        Assert(actual, expected);

        actual = input.AsQueryable().ApplyPaging(spec);
        Assert(actual, expected);
    }

    [Fact]
    public void DoesNotFilter_GivenZeroSkip()
    {
        List<Customer> input = [new(1), new(2), new(3), new(4), new(5)];
        List<Customer> expected = [new(1), new(2), new(3), new(4), new(5)];

        var spec = new Specification<Customer>();
        spec.Query
            .Skip(0);

        var actual = input.ApplyPaging(spec);
        Assert(actual, expected);

        actual = input.AsQueryable().ApplyPaging(spec);
        Assert(actual, expected);
    }

    private static void Assert<T>(IEnumerable<T> actual, IEnumerable<T> expected)
    {
        actual.Should().NotBeNull();
        actual.Should().HaveSameCount(expected);
        actual.Should().Equal(expected);
    }
}
