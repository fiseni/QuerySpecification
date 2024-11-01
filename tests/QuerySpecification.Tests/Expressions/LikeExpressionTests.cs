using System.Runtime.CompilerServices;

namespace Tests.Expressions;

// TODO: Do we need this anymore? [fatii, 01/11/2024]
public class LikeExpressionTests
{
    public record Customer(int Id, string Name);

    //[Fact]
    //public void Constructor_ThrowsArgumentNullException_GivenNullExpression()
    //{
    //    var sut = () => new LikeExpression<Customer>(null!, "x");

    //    sut.Should().Throw<ArgumentNullException>().WithParameterName("keySelector");
    //}

    //[Fact]
    //public void Constructor_ThrowsArgumentNullException_GivenNullPattern()
    //{
    //    Expression<Func<Customer, string?>> expr = x => x.Name;
    //    var sut = () => new LikeExpression<Customer>(expr, null!);

    //    sut.Should().Throw<ArgumentNullException>().WithParameterName("pattern");
    //}

    // TODO: Do we need this anymore? [fatii, 01/11/2024]
    //[Fact]
    //public void Constructor_GivenValidValues()
    //{
    //    Expression<Func<Customer, string?>> expr = x => x.Name;

    //    var sut = new LikeExpression<Customer>(expr, "x", 99);

    //    sut.KeySelector.Should().Be(expr);
    //    sut.Pattern.Should().Be("x");
    //    sut.Group.Should().Be(99);
    //    Accessors<Customer>.FuncFieldOf(sut).Should().BeNull();
    //    sut.KeySelectorFunc.Should().NotBeNull();
    //    //sut.KeySelectorFunc.Should().BeEquivalentTo(expr.Compile());
    //    Accessors<Customer>.FuncFieldOf(sut).Should().NotBeNull();
    //}

    private class Accessors<T>
    {
        [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_keySelectorFunc")]
        public static extern ref Func<T, string>? FuncFieldOf(LikeExpression<T> @this);
    }
}
