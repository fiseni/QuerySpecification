using System.Runtime.CompilerServices;

namespace Pozitron.QuerySpecification.Tests;

public class SpecificationTests
{
    public record Customer(int Id, string Name);

    [Fact]
    public void CollectionFieldsAreNull_GivenEmptySpec()
    {
        var spec = new Specification<Customer>();

        Accessors<Customer>.WhereExpressionsOf(spec).Should().BeNull();
        Accessors<Customer>.LikeExpressionsOf(spec).Should().BeNull();
        Accessors<Customer>.OrderExpressionsOf(spec).Should().BeNull();
        Accessors<Customer>.IncludeExpressionsOf(spec).Should().BeNull();
        Accessors<Customer>.IncludeStringsOf(spec).Should().BeNull();
        Accessors<Customer>.ItemsOf(spec).Should().BeNull();
    }

    [Fact]
    public void CollectionsPropertiesReturnEmptyEnumerable_GivenEmptySpec()
    {
        var spec = new Specification<Customer>();

        spec.WhereExpressions.Should().BeSameAs(Enumerable.Empty<WhereExpression<Customer>>());
        spec.LikeExpressions.Should().BeSameAs(Enumerable.Empty<LikeExpression<Customer>>());
        spec.OrderExpressions.Should().BeSameAs(Enumerable.Empty<OrderExpression<Customer>>());
        spec.IncludeExpressions.Should().BeSameAs(Enumerable.Empty<IncludeExpression>());
        spec.IncludeStrings.Should().BeSameAs(Enumerable.Empty<string>());
    }

    [Fact]
    public void ItemsReturnsNewDictionaryOnFirstAccess()
    {
        var spec = new Specification<Customer>();

        Accessors<Customer>.ItemsOf(spec).Should().BeNull();
        spec.Items.Should().NotBeNull();
        Accessors<Customer>.ItemsOf(spec).Should().NotBeNull();
    }

    private class Accessors<T>
    {
        [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_whereExpressions")]
        public static extern ref List<WhereExpression<T>>? WhereExpressionsOf(Specification<T> @this);

        [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_likeExpressions")]
        public static extern ref List<LikeExpression<T>>? LikeExpressionsOf(Specification<T> @this);

        [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_orderExpressions")]
        public static extern ref List<OrderExpression<T>>? OrderExpressionsOf(Specification<T> @this);

        [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_includeExpressions")]
        public static extern ref List<IncludeExpression>? IncludeExpressionsOf(Specification<T> @this);

        [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_includeStrings")]
        public static extern ref List<string>? IncludeStringsOf(Specification<T> @this);

        [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_items")]
        public static extern ref Dictionary<string, object>? ItemsOf(Specification<T> @this);
    }
}
