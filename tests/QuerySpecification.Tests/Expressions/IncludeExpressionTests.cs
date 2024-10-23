//namespace Tests.Expressions;

//public class IncludeExpressionTests
//{
//    public record Customer(int Id, Address Address);
//    public record Address(int Id, City City);
//    public record City(int Id);

//    [Fact]
//    public void Constructor_ThrowsArgumentNullException_GivenNullForLambdaExpression()
//    {
//        var sut = () => new IncludeExpression(null!, typeof(Customer), typeof(Address));

//        sut.Should().Throw<ArgumentNullException>().WithParameterName("expression");
//    }

//    [Fact]
//    public void Constructor_ThrowsArgumentNullException_GivenNullForEntityType()
//    {
//        Expression<Func<Customer, Address>> expr = x => x.Address;
//        var sut = () => new IncludeExpression(expr, null!, typeof(Address));

//        sut.Should().Throw<ArgumentNullException>().WithParameterName("entityType");
//    }

//    [Fact]
//    public void Constructor_ThrowsArgumentNullException_GivenNullForPropertyType()
//    {
//        Expression<Func<Customer, Address>> expr = x => x.Address;
//        var sut = () => new IncludeExpression(expr, typeof(Customer), null!);

//        sut.Should().Throw<ArgumentNullException>().WithParameterName("propertyType");
//    }

//    [Fact]
//    public void Constructor_ThrowsArgumentNullException_GivenNullForPreviousPropertyType()
//    {
//        Expression<Func<Customer, Address>> expr = x => x.Address;
//        var sut = () => new IncludeThenExpression(expr, typeof(Customer), typeof(Address), null!);

//        sut.Should().Throw<ArgumentNullException>().WithParameterName("previousPropertyType");
//    }

//    [Fact]
//    public void Constructor_GivenIncludeExpression()
//    {
//        Expression<Func<Customer, Address>> expr = x => x.Address;
//        var sut = new IncludeExpression(expr, typeof(Customer), typeof(Address));

//        sut.LambdaExpression.Should().Be(expr);
//        sut.EntityType.Should().Be<Customer>();
//        sut.PropertyType.Should().Be<Address>();
//    }

//    [Fact]
//    public void Constructor_GivenIncludeThenExpression()
//    {
//        Expression<Func<Address, City>> expr = x => x.City;
//        var sut = new IncludeThenExpression(expr, typeof(Customer), typeof(City), typeof(Address));

//        sut.LambdaExpression.Should().Be(expr);
//        sut.EntityType.Should().Be<Customer>();
//        sut.PropertyType.Should().Be<City>();
//        sut.PreviousPropertyType.Should().Be<Address>();
//    }
//}
