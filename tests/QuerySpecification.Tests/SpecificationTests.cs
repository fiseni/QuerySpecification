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

    private class Accessors<T>
    {
        [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_states")]
        public static extern ref SpecState[]? State(Specification<T> @this);
    }
}
