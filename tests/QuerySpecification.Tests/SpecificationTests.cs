using System.Runtime.CompilerServices;

namespace Tests;

public class SpecificationTests
{
    public record Customer(int Id, string Name);

    [Fact]
    public void StateIsNull_GivenEmptySpec()
    {
        var spec = new Specification<Customer>();

        Accessors<Customer>.State(spec).Should().BeNull();
    }

    [Fact]
    public void CollectionsProperties_ReturnEmptyEnumerable_GivenEmptySpec()
    {
        var spec = new Specification<Customer>();

        spec.WhereExpressions.Should().BeSameAs(Enumerable.Empty<WhereExpression<Customer>>());
        spec.LikeExpressions.Should().BeSameAs(Enumerable.Empty<LikeExpression<Customer>>());
        spec.OrderExpressions.Should().BeSameAs(Enumerable.Empty<OrderExpression<Customer>>());
        spec.IncludeExpressions.Should().BeSameAs(Enumerable.Empty<IncludeExpression>());
        spec.IncludeStrings.Should().BeSameAs(Enumerable.Empty<string>());
    }

    [Fact]
    public void Items_ReturnsNewDictionaryOnFirstAccess()
    {
        var spec = new Specification<Customer>();

        var state = Accessors<Customer>.State(spec);
        Accessors<Customer>.State(spec).Should().BeNull();
        spec.Items.Should().NotBeNull();
        Accessors<Customer>.State(spec).Should().NotBeNull();
    }

    private class Accessors<T>
    {
        [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_state")]
        public static extern ref object?[]? State(Specification<T> @this);
    }
}
