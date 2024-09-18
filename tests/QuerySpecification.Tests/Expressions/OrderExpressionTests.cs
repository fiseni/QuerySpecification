using System.Runtime.CompilerServices;

namespace Tests.Expressions;

public class OrderExpressionTests
{
    public record Customer(int Id, string Name);

    [Fact]
    public void Constructor_ThrowsArgumentNullException_GivenNullExpression()
    {
        Action sutAction = () => new OrderExpression<Customer>(null!, OrderTypeEnum.OrderBy);

        sutAction.Should().Throw<ArgumentNullException>().WithParameterName("keySelector");
    }

    [Fact]
    public void Constructor_GivenValidValues()
    {
        Expression<Func<Customer, object?>> expr = x => x.Name;

        var sut = new OrderExpression<Customer>(expr, OrderTypeEnum.OrderBy);

        sut.KeySelector.Should().Be(expr);
        sut.OrderType.Should().Be(OrderTypeEnum.OrderBy);
        Accessors<Customer>.FuncFieldOf(sut).Should().BeNull();
        sut.KeySelectorFunc.Should().NotBeNull();
        //sut.KeySelectorFunc.Should().BeEquivalentTo(expr.Compile());
        Accessors<Customer>.FuncFieldOf(sut).Should().NotBeNull();
    }

    private class Accessors<T>
    {
        [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_keySelectorFunc")]
        public static extern ref Func<T, object?>? FuncFieldOf(OrderExpression<T> @this);
    }
}
