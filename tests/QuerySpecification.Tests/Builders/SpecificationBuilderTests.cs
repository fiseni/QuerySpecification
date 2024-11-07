using System.Runtime.CompilerServices;

namespace Tests.Builders;

// The behavior is already extensively tested.
// These are just dummy tests to ensure the methods call the underlying Specification methods.
public class SpecificationBuilderTests
{
    public record Customer(int Id, string Name);

    [Fact]
    public void Add()
    {
        var spec1 = new Specification<Customer>();
        var spec2 = new Specification<Customer>();
        spec1.Query.Add(1, "test");
        spec2.Add(1, "test");

        AssertEquals(spec1, spec2);

        var spec3 = new Specification<Customer, string>();
        var spec4 = new Specification<Customer, string>();
        spec3.Query.Add(1, "test");
        spec4.Add(1, "test");

        AssertEquals(spec1, spec2);
    }

    [Fact]
    public void AddOrUpdate()
    {
        var spec1 = new Specification<Customer>();
        var spec2 = new Specification<Customer>();
        spec1.Query.AddOrUpdate(1, "test");
        spec2.AddOrUpdate(1, "test");

        AssertEquals(spec1, spec2);

        var spec3 = new Specification<Customer, string>();
        var spec4 = new Specification<Customer, string>();
        spec3.Query.AddOrUpdate(1, "test");
        spec4.AddOrUpdate(1, "test");

        AssertEquals(spec1, spec2);
    }

    private static void AssertEquals<T>(Specification<T> spec1, Specification<T> spec2)
    {
        var items1 = Accessors<T>.Items(spec1);
        var items2 = Accessors<T>.Items(spec2);
        items1.Should().Equal(items2);
    }

    private class Accessors<T>
    {
        [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_items")]
        public static extern ref SpecItem[]? Items(Specification<T> @this);
    }
}
