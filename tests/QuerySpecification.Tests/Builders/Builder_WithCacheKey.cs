namespace Tests.Builders;

public class Builder_WithCacheKey
{
    public record Customer(int Id, string Name);

    [Fact]
    public void DoesNothing_GivenNoWithCacheKey()
    {
        var spec1 = new Specification<Customer>();
        var spec2 = new Specification<Customer, string>();

        spec1.CacheKey.Should().BeNull();
        spec1.HasCacheKey.Should().BeFalse();

        spec2.CacheKey.Should().BeNull();
        spec2.HasCacheKey.Should().BeFalse();
    }

    [Fact]
    public void DoesNothing_GivenWithCacheKeyWithFalseCondition()
    {
        var key = "someKey";

        var spec1 = new Specification<Customer>();
        spec1.Query
            .Where(x=> x.Id > 0)
            .WithCacheKey(key, false);

        var spec2 = new Specification<Customer, string>();
        spec2.Query
            .Where(x=> x.Id > 0)
            .WithCacheKey(key, false);

        spec1.CacheKey.Should().BeNull();
        spec1.HasCacheKey.Should().BeFalse();

        spec2.CacheKey.Should().BeNull();
        spec2.HasCacheKey.Should().BeFalse();
    }

    [Fact]
    public void SetsCacheKey_GivenWithCacheKey()
    {
        var key = "someKey";

        var spec1 = new Specification<Customer>();
        spec1.Query
            .WithCacheKey(key);

        var spec2 = new Specification<Customer, string>();
        spec2.Query
            .WithCacheKey(key);

        spec1.CacheKey.Should().Be(key);
        spec1.HasCacheKey.Should().BeTrue();

        spec1.CacheKey.Should().Be(key);
        spec2.HasCacheKey.Should().BeTrue();
    }
}
