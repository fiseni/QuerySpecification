using System.Runtime.CompilerServices;

namespace QuerySpecification.Tests.Expressions;

public class LikeExpressionTests
{
    public record Customer(int Id, string Name);

    [Fact]
    public void ThrowsArgumentNullException_GivenNullExpression()
    {
        Action sutAction = () => new LikeExpression<Customer>(null!, "x");

        sutAction.Should().Throw<ArgumentNullException>().WithParameterName("keySelector");
    }

    [Fact]
    public void ThrowsArgumentNullException_GivenNullPattern()
    {
        Expression<Func<Customer, string?>> expr = x => x.Name;
        Action sutAction = () => new LikeExpression<Customer>(expr, null!);

        sutAction.Should().Throw<ArgumentNullException>().WithParameterName("pattern");
    }

    [Fact]
    public void SetsStateAndFunc()
    {
        Expression<Func<Customer, string?>> expr = x => x.Name;

        var sut = new LikeExpression<Customer>(expr, "x", 99);

        sut.KeySelector.Should().Be(expr);
        sut.Pattern.Should().Be("x");
        sut.Group.Should().Be(99);
        Accessors<Customer>.FuncFieldOf(sut).Should().BeNull();
        sut.KeySelectorFunc.Should().NotBeNull();
        //sut.KeySelectorFunc.Should().BeEquivalentTo(expr.Compile());
        Accessors<Customer>.FuncFieldOf(sut).Should().NotBeNull();
    }

    private class Accessors<T>
    {
        [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_keySelectorFunc")]
        public static extern ref Func<T, string>? FuncFieldOf(LikeExpression<T> @this);
    }
}
