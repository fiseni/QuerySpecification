using System.Collections;

namespace Tests;

public class SpecificationTests
{
    private static readonly SpecItem _emptySpecItem = new();
    public record Customer(int Id, string Name, Address Address);
    public record Address(int Id, City City);
    public record City(int Id, string Name);

    [Fact]
    public void WhereExpressionsCompiled()
    {
        Expression<Func<Customer, bool>> filter = x => x.Id == 1;
        var spec = new Specification<Customer>();
        spec.Query
            .Where(filter);

        var expressions = spec.WhereExpressionsCompiled.ToList();

        expressions.Should().HaveCount(1);
        expressions[0].Filter.Should().BeOfType<Func<Customer, bool>>();
    }

    [Fact]
    public void OrderExpressionsCompiled()
    {
        Expression<Func<Customer, object?>> orderBy = x => x.Id;
        Expression<Func<Customer, object?>> orderThenBy = x => x.Name;
        var spec = new Specification<Customer>();
        spec.Query
            .OrderBy(orderBy)
            .ThenBy(orderThenBy);

        var expressions = spec.OrderExpressionsCompiled.ToList();

        expressions.Should().HaveCount(2);
        expressions[0].KeySelector.Should().BeOfType<Func<Customer, object?>>();
        expressions[0].Type.Should().Be(OrderType.OrderBy);
        expressions[1].KeySelector.Should().BeOfType<Func<Customer, object?>>();
        expressions[1].Type.Should().Be(OrderType.ThenBy);
    }

    [Fact]
    public void LikeExpressionsCompiled()
    {
        Expression<Func<Customer, string?>> selector = x => x.Name;
        var searchTerm = "%abc%";
        var group = 10;
        var spec = new Specification<Customer>();
        spec.Query
            .Like(selector, searchTerm, group);

        var expressions = spec.LikeExpressionsCompiled.ToList();

        expressions.Should().HaveCount(1);
        expressions[0].KeySelector.Should().BeOfType<Func<Customer, string?>>();
        expressions[0].Pattern.Should().Be(searchTerm);
        expressions[0].Group.Should().Be(group);
    }

    [Fact]
    public void WhereExpressions()
    {
        Expression<Func<Customer, bool>> filter = x => x.Id == 1;
        var spec = new Specification<Customer>();
        spec.Query
            .Where(filter);

        var expressions = spec.WhereExpressions.ToList();

        expressions.Should().HaveCount(1);
        expressions[0].Filter.Should().BeSameAs(filter);
    }

    [Fact]
    public void OrderExpressions()
    {
        Expression<Func<Customer, object?>> orderBy = x => x.Id;
        Expression<Func<Customer, object?>> orderThenBy = x => x.Name;
        var spec = new Specification<Customer>();
        spec.Query
            .OrderBy(orderBy)
            .ThenBy(orderThenBy);

        var expressions = spec.OrderExpressions.ToList();

        expressions.Should().HaveCount(2);
        expressions[0].KeySelector.Should().BeSameAs(orderBy);
        expressions[0].Type.Should().Be(OrderType.OrderBy);
        expressions[1].KeySelector.Should().BeSameAs(orderThenBy);
        expressions[1].Type.Should().Be(OrderType.ThenBy);
    }

    [Fact]
    public void LikeExpressions()
    {
        Expression<Func<Customer, string?>> selector = x => x.Name;
        var searchTerm = "%abc%";
        var group = 10;
        var spec = new Specification<Customer>();
        spec.Query
            .Like(selector, searchTerm, group);

        var expressions = spec.LikeExpressions.ToList();

        expressions.Should().HaveCount(1);
        expressions[0].KeySelector.Should().BeSameAs(selector);
        expressions[0].Pattern.Should().Be(searchTerm);
        expressions[0].Group.Should().Be(group);
    }

    [Fact]
    public void IncludeExpressions()
    {
        Expression<Func<Customer, Address>> include = x => x.Address;
        Expression<Func<Address, City>> thenInclude = x => x.City;
        var spec = new Specification<Customer>();
        spec.Query
            .Include(include)
            .ThenInclude(thenInclude);

        var expressions = spec.IncludeExpressions.ToList();

        expressions.Should().HaveCount(2);
        expressions[0].LambdaExpression.Should().BeSameAs(include);
        expressions[0].Type.Should().Be(IncludeType.Include);
        expressions[1].LambdaExpression.Should().BeSameAs(thenInclude);
        expressions[1].Type.Should().Be(IncludeType.ThenInclude);
    }

    [Fact]
    public void IncludeStrings()
    {
        var includeString = nameof(Address);
        var spec = new Specification<Customer>();
        spec.Query
            .Include(includeString);

        var expressions = spec.IncludeStrings.ToList();

        expressions.Should().HaveCount(1);
        expressions[0].Should().Be(includeString);
    }

    [Fact]
    public void Add()
    {
        var city1 = new City(1, "City1");
        var city2 = new City(2, "City2");

        var spec = new Specification<City>();

        spec.Add(10, city1);
        spec.Add(11, city2);

        var result = spec.Items.ToArray();
        result.Should().HaveCount(2);
        result[0].Reference.Should().BeSameAs(city1);
        result[0].Type.Should().Be(10);
        result[1].Reference.Should().BeSameAs(city2);
        result[1].Type.Should().Be(11);
    }

    [Fact]
    public void Add_ThrowsArgumentNullException_GivenNullValue()
    {
        var spec = new Specification<City>();

        var action = () => spec.Add(10, null!);

        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Add_ArgumentOutOfRangeException_GivenZeroType()
    {
        var spec = new Specification<City>();

        var action = () => spec.Add(0, new object());

        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Add_ArgumentOutOfRangeException_GivenNegativeType()
    {
        var spec = new Specification<City>();

        var action = () => spec.Add(-1, new object());

        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void AddOrUpdate_GivenItemNotExist()
    {
        var city1 = new City(1, "City1");
        var city2 = new City(2, "City2");

        var spec = new Specification<City>();

        spec.AddOrUpdate(10, city1);
        spec.AddOrUpdate(11, city2);

        var result = spec.Items.ToArray();
        result.Should().HaveCount(2);
        result[0].Reference.Should().BeSameAs(city1);
        result[0].Type.Should().Be(10);
        result[1].Reference.Should().BeSameAs(city2);
        result[1].Type.Should().Be(11);
    }

    [Fact]
    public void AddOrUpdate_GivenItemExists()
    {
        var city1 = new City(1, "City1");
        var city2 = new City(2, "City2");

        var spec = new Specification<City>();

        spec.AddOrUpdate(10, city1);
        spec.AddOrUpdate(10, city2);

        var result = spec.Items.ToArray();
        result.Should().HaveCount(2);
        result[0].Reference.Should().BeSameAs(city2);
        result[0].Type.Should().Be(10);
        result[1].Should().Be(_emptySpecItem);
    }

    [Fact]
    public void AddOrUpdate_ThrowsArgumentNullException_GivenNullValue()
    {
        var spec = new Specification<City>();

        var action = () => spec.AddOrUpdate(10, null!);

        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void AddOrUpdate_ArgumentOutOfRangeException_GivenZeroType()
    {
        var spec = new Specification<City>();

        var action = () => spec.AddOrUpdate(0, new object());

        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void AddOrUpdate_ArgumentOutOfRangeException_GivenNegativeType()
    {
        var spec = new Specification<City>();

        var action = () => spec.AddOrUpdate(-1, new object());

        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void FirstOrDefault()
    {
        var city1 = new City(1, "City1");
        var city2 = new City(2, "City2");

        var spec = new Specification<City>();
        spec.Query
            .Where(x => x.Id == 1);

        spec.Add(10, city1);
        spec.Add(10, city2);
        spec.Add(11, new City(3, "City3"));

        var result = spec.FirstOrDefault<City>(10);

        result.Should().BeSameAs(city1);
    }

    [Fact]
    public void FirstOrDefault_ReturnsNull_GivenNoItems()
    {
        var spec = new Specification<City>();
        spec.Query
            .Where(x => x.Id == 1);

        var result = spec.FirstOrDefault<City>(10);

        result.Should().BeNull();
    }

    [Fact]
    public void FirstOrDefault_ReturnsNull_GivenEmptySpec()
    {
        var spec = new Specification<City>();

        var result = spec.FirstOrDefault<City>(10);

        result.Should().BeNull();
    }

    [Fact]
    public void First()
    {
        var city1 = new City(1, "City1");
        var city2 = new City(2, "City2");

        var spec = new Specification<City>();
        spec.Query
            .Where(x => x.Id == 1);

        spec.Add(10, city1);
        spec.Add(10, city2);
        spec.Add(11, new City(3, "City3"));

        var result = spec.First<City>(10);

        result.Should().BeSameAs(city1);
    }

    [Fact]
    public void First_ThrowsInvalidOperationException_GivenNoItems()
    {
        var spec = new Specification<City>();
        spec.Query
            .Where(x => x.Id == 1);

        var action = () => spec.First<City>(10);

        action.Should().Throw<InvalidOperationException>().WithMessage("Specification contains no matching item");
    }

    [Fact]
    public void First_ThrowsInvalidOperationException_GivenEmptySpec()
    {
        var spec = new Specification<City>();

        var action = () => spec.First<City>(10);

        action.Should().Throw<InvalidOperationException>().WithMessage("Specification contains no items");
    }

    [Fact]
    public void OfType()
    {
        var city1 = new City(1, "City1");
        var city2 = new City(2, "City2");

        var spec = new Specification<City>();
        spec.Query
            .Where(x => x.Id == 1);

        spec.Add(10, city1);
        spec.Add(10, city2);
        spec.Add(11, new City(3, "City3"));

        var result = spec.OfType<City>(10).ToList();

        result.Should().HaveCount(2);
        result[0].Should().BeSameAs(city1);
        result[1].Should().BeSameAs(city2);
    }

    [Fact]
    public void OfType_IteratorClone()
    {
        var city1 = new City(1, "City1");
        var city2 = new City(2, "City2");

        var spec = new Specification<City>();
        spec.Query
            .Where(x => x.Id == 1);

        spec.Add(10, city1);
        spec.Add(10, city2);
        spec.Add(11, new City(3, "City3"));

        var result = spec.OfType<City>(10);

        // Not materializing the result with ToList() to test the iterator cloning.
        // The following assertions will iterate multiple times and will force cloning.
        result.Should().HaveCount(2);
        result.First().Should().BeSameAs(city1);
        result.Skip(1).First().Should().BeSameAs(city2);

        // Testing the IEnumerable APIs of Iterator.
        IEnumerable enumerable = result.Cast<object>();
        var count = 0;
        foreach (var item in enumerable) count++;
        count.Should().Be(2);
    }

    [Fact]
    public void OfType_ReturnsEmptyEnumerable_GivenEmptySpec()
    {
        var spec = new Specification<City>();

        var result = spec.OfType<City>(10);

        result.Should().BeEmpty();
        result.Should().BeSameAs(Enumerable.Empty<City>());
    }

    [Fact]
    public void CollectionsProperties_ReturnEmptyEnumerable_GivenEmptySpec()
    {
        var spec = new Specification<Customer>();

        spec.WhereExpressionsCompiled.Should().BeSameAs(Enumerable.Empty<WhereExpressionCompiled<Customer>>());
        spec.LikeExpressionsCompiled.Should().BeSameAs(Enumerable.Empty<LikeExpressionCompiled<Customer>>());
        spec.OrderExpressionsCompiled.Should().BeSameAs(Enumerable.Empty<OrderExpressionCompiled<Customer>>());
        spec.WhereExpressions.Should().BeSameAs(Enumerable.Empty<WhereExpression<Customer>>());
        spec.LikeExpressions.Should().BeSameAs(Enumerable.Empty<LikeExpression<Customer>>());
        spec.OrderExpressions.Should().BeSameAs(Enumerable.Empty<OrderExpression<Customer>>());
        spec.IncludeExpressions.Should().BeSameAs(Enumerable.Empty<IncludeExpression<Customer>>());
        spec.IncludeStrings.Should().BeSameAs(Enumerable.Empty<string>());
    }
}
