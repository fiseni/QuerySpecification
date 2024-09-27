using System.Runtime.CompilerServices;

namespace Tests.Expressions;

public class WhereExpressionTests
{
    public record Customer(int Id);

    [Fact]
    public void Constructor_ThrowsArgumentNullException_GivenNullExpression()
    {
        var sutAction = () => new WhereExpression<Customer>(null!);

        sutAction.Should().Throw<ArgumentNullException>().WithParameterName("filter");
    }

    [Fact]
    public void Constructor_GivenValidValues()
    {
        Expression<Func<Customer, bool>> expr = x => x.Id == 1;

        var sut = new WhereExpression<Customer>(expr);

        sut.Filter.Should().Be(expr);
        Accessors<Customer>.FuncFieldOf(sut).Should().BeNull();
        sut.FilterFunc.Should().NotBeNull();
        //sut.FilterFunc.Should().BeEquivalentTo(expr.Compile());
        Accessors<Customer>.FuncFieldOf(sut).Should().NotBeNull();
    }

    private class Accessors<T>
    {
        [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_filterFunc")]
        public static extern ref Func<T, bool>? FuncFieldOf(WhereExpression<T> @this);
    }
}
