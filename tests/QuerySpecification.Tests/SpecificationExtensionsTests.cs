using System.Runtime.CompilerServices;

namespace Tests;

public class SpecificationExtensionsTests
{
    private record Address(int Id, string Street);
    private record Person(int Id, string Name, List<string> Names, Address Address);

    [Fact]
    public void WithProjectionOf_ReturnsCopyWithProjection_GivenProjectionSpec()
    {
        var spec = new Specification<Person>();
        spec.Query
            .Where(x => x.Name == "test")
            .Include(x => x.Address)
            .Include("Address")
            .OrderBy(x => x.Id)
            .Like(x => x.Name, "test")
            .Take(2)
            .Skip(3)
            .WithCacheKey("testKey")
            .IgnoreQueryFilters()
            .IgnoreQueryFilters()
            .AsSplitQuery()
            .AsNoTracking()
            .TagWith("testQuery1");

        var projectionSpec = new Specification<Person, string>();
        projectionSpec.Query.Select(x => x.Name);

        var newSpec = spec.WithProjectionOf(projectionSpec);

        newSpec.Should().NotBeSameAs(spec);
        Accessors<Person>.Items(newSpec).Should().NotBeSameAs(Accessors<Person>.Items(spec));
        Accessors<Person>.Items(newSpec)!.Take(8).Should().Equal(Accessors<Person>.Items(spec)!.Take(8));

        // Unnecessary, but let's assert the states individually as well.
        // Compare expressions by their string representation for logical equivalence

        newSpec.WhereExpressions.Should().NotBeSameAs(spec.WhereExpressions);
        newSpec.WhereExpressions.Should().BeEquivalentTo(spec.WhereExpressions);
        newSpec.WhereExpressions.Select(x => x.Filter.ToString())
            .Should().BeEquivalentTo(spec.WhereExpressions.Select(x => x.Filter.ToString()));

        newSpec.IncludeExpressions.Should().NotBeSameAs(spec.IncludeExpressions);
        newSpec.IncludeExpressions.Should().BeEquivalentTo(spec.IncludeExpressions);
        newSpec.IncludeExpressions.Select(x => x.LambdaExpression.ToString())
            .Should().Equal(spec.IncludeExpressions.Select(x => x.LambdaExpression.ToString()));

        newSpec.OrderExpressions.Should().NotBeSameAs(spec.OrderExpressions);
        newSpec.OrderExpressions.Should().BeEquivalentTo(spec.OrderExpressions);
        newSpec.OrderExpressions.Select(x => x.KeySelector.ToString())
            .Should().Equal(spec.OrderExpressions.Select(x => x.KeySelector.ToString()));

        newSpec.LikeExpressions.Should().NotBeSameAs(spec.LikeExpressions);
        newSpec.LikeExpressions.Should().BeEquivalentTo(spec.LikeExpressions);
        newSpec.LikeExpressions.Select(x => x.KeySelector.ToString())
            .Should().Equal(spec.LikeExpressions.Select(x => x.KeySelector.ToString()));

        newSpec.IncludeStrings.Should().NotBeSameAs(spec.IncludeStrings);
        newSpec.IncludeStrings.Should().Equal(spec.IncludeStrings);

        newSpec.QueryTags.Should().NotBeSameAs(spec.QueryTags);
        newSpec.QueryTags.Should().Equal(spec.QueryTags);

        newSpec.Take.Should().Be(spec.Take);
        newSpec.Skip.Should().Be(spec.Skip);
        newSpec.CacheKey.Should().Be(spec.CacheKey);
        newSpec.IgnoreQueryFilters.Should().Be(spec.IgnoreQueryFilters);
        newSpec.IgnoreAutoIncludes.Should().Be(spec.IgnoreAutoIncludes);
        newSpec.AsSplitQuery.Should().Be(spec.AsSplitQuery);
        newSpec.AsNoTracking.Should().Be(spec.AsNoTracking);
        newSpec.AsNoTrackingWithIdentityResolution.Should().Be(spec.AsNoTrackingWithIdentityResolution);
        newSpec.AsTracking.Should().Be(spec.AsTracking);

        // Assert that the projection is set from projectionSpec
        newSpec.Selector.Should().Be(projectionSpec.Selector);
    }

    [Fact]
    public void WithProjectionOf_ReturnsBaseCopy_GivenProjectionSpecWithNoSelect()
    {
        var spec = new Specification<Person>();
        spec.Query
            .Where(x => x.Name == "test")
            .Include(x => x.Address)
            .Include("Address")
            .OrderBy(x => x.Id)
            .Like(x => x.Name, "test")
            .Take(2)
            .Skip(3)
            .WithCacheKey("testKey")
            .IgnoreQueryFilters()
            .IgnoreQueryFilters()
            .AsSplitQuery()
            .AsNoTracking()
            .TagWith("testQuery1");

        var projectionSpec = new Specification<Person, string>();

        var newSpec = spec.WithProjectionOf(projectionSpec);

        newSpec.Should().NotBeSameAs(spec);
        Accessors<Person>.Items(newSpec).Should().NotBeSameAs(Accessors<Person>.Items(spec));
        Accessors<Person>.Items(newSpec)!.Take(8).Should().Equal(Accessors<Person>.Items(spec)!.Take(8));
        Accessors<Person>.Items(newSpec).Should().NotContain(x => x.Type == ItemType.Select);

        newSpec.Selector.Should().BeNull();
    }

    [Fact]
    public void WithProjectionOf_ReturnsBaseCopy_GivenProjectionSpecWithNullSelect()
    {
        var spec = new Specification<Person>();
        spec.Query
            .Where(x => x.Name == "test")
            .Include(x => x.Address)
            .Include("Address")
            .OrderBy(x => x.Id)
            .Like(x => x.Name, "test")
            .Take(2)
            .Skip(3)
            .WithCacheKey("testKey")
            .IgnoreQueryFilters()
            .IgnoreQueryFilters()
            .AsSplitQuery()
            .AsNoTracking()
            .TagWith("testQuery1");

        var projectionSpec = new Specification<Person, string>();
        projectionSpec.AddOrUpdateInternal(ItemType.Select, null!); // Explicitly set Selector to null

        var newSpec = spec.WithProjectionOf(projectionSpec);

        newSpec.Should().NotBeSameAs(spec);
        Accessors<Person>.Items(newSpec).Should().NotBeSameAs(Accessors<Person>.Items(spec));
        Accessors<Person>.Items(newSpec)!.Take(8).Should().Equal(Accessors<Person>.Items(spec)!.Take(8));
        Accessors<Person>.Items(newSpec).Should().NotContain(x => x.Type == ItemType.Select);

        newSpec.Selector.Should().BeNull();
    }

    private class Accessors<T>
    {
        [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_items")]
        public static extern ref SpecItem[]? Items(Specification<T> @this);
    }
}
