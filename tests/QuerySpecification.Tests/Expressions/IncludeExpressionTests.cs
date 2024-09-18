namespace Tests.Expressions;

public class IncludeExpressionTests
{
    public record Customer(int Id, Address Address);
    public record Address(int Id, City City);
    public record City(int Id);

    [Fact]
    public void Constructor_ThrowsArgumentNullException_GivenNullForLambdaExpression()
    {
        Action sutAction = () => new IncludeExpression(null!, typeof(Customer), typeof(Address));

        sutAction.Should().Throw<ArgumentNullException>().WithParameterName("expression");
    }

    [Fact]
    public void Constructor_ThrowsArgumentNullException_GivenNullForEntityType()
    {
        Expression<Func<Customer, Address>> expr = x => x.Address;
        Action sutAction = () => new IncludeExpression(expr, null!, typeof(Address));

        sutAction.Should().Throw<ArgumentNullException>().WithParameterName("entityType");
    }

    [Fact]
    public void Constructor_ThrowsArgumentNullException_GivenNullForPropertyType()
    {
        Expression<Func<Customer, Address>> expr = x => x.Address;
        Action sutAction = () => new IncludeExpression(expr, typeof(Customer), null!);

        sutAction.Should().Throw<ArgumentNullException>().WithParameterName("propertyType");
    }

    [Fact]
    public void Constructor_ThrowsArgumentNullException_GivenNullForPreviousPropertyType()
    {
        Expression<Func<Customer, Address>> expr = x => x.Address;
        Action sutAction = () => new IncludeExpression(expr, typeof(Customer), typeof(Address), null!);

        sutAction.Should().Throw<ArgumentNullException>().WithParameterName("previousPropertyType");
    }

    [Fact]
    public void Constructor_GivenIncludeExpression()
    {
        Expression<Func<Customer, Address>> expr = x => x.Address;
        var sut = new IncludeExpression(expr, typeof(Customer), typeof(Address));

        sut.Type.Should().Be(IncludeTypeEnum.Include);
        sut.LambdaExpression.Should().Be(expr);
        sut.EntityType.Should().Be(typeof(Customer));
        sut.PropertyType.Should().Be(typeof(Address));
        sut.PreviousPropertyType.Should().BeNull();
    }

    [Fact]
    public void Constructor_GivenIncludeThenExpression()
    {
        Expression<Func<Address, City>> expr = x => x.City;
        var sut = new IncludeExpression(expr, typeof(Customer), typeof(City), typeof(Address));

        sut.Type.Should().Be(IncludeTypeEnum.ThenInclude);
        sut.LambdaExpression.Should().Be(expr);
        sut.EntityType.Should().Be(typeof(Customer));
        sut.PropertyType.Should().Be(typeof(City));
        sut.PreviousPropertyType.Should().Be(typeof(Address));
    }
}
