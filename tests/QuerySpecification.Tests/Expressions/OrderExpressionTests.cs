using System.Runtime.CompilerServices;

namespace Tests.Expressions;

// TODO: Do we need this anymore? [fatii, 01/11/2024]
public class OrderExpressionTests
{
    public record Customer(int Id, string Name);

    //[Fact]
    //public void Constructor_ThrowsArgumentNullException_GivenNullExpression()
    //{
    //    var sut = () => new OrderExpression<Customer>(null!, OrderType.OrderBy);

    //    sut.Should().Throw<ArgumentNullException>().WithParameterName("keySelector");
    //}

    //[Fact]
    //public void Constructor_GivenValidValues()
    //{
    //    Expression<Func<Customer, object?>> expr = x => x.Name;

    //    var sut = new OrderExpression<Customer>(expr, OrderType.OrderBy);

    //    sut.KeySelector.Should().Be(expr);
    //    sut.Type.Should().Be(OrderType.OrderBy);
    //    Accessors<Customer>.FuncFieldOf(sut).Should().BeNull();
    //    sut.KeySelectorFunc.Should().NotBeNull();
    //    //sut.KeySelectorFunc.Should().BeEquivalentTo(expr.Compile());
    //    Accessors<Customer>.FuncFieldOf(sut).Should().NotBeNull();
    //}

    private class Accessors<T>
    {
        [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_keySelectorFunc")]
        public static extern ref Func<T, object?>? FuncFieldOf(OrderExpression<T> @this);
    }
}
