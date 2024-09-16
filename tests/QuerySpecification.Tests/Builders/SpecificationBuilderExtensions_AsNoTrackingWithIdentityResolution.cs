namespace QuerySpecification.Tests.Builders;

public class SpecificationBuilderExtensions_AsNoTrackingWithIdentityResolution
{
    public record Customer(int Id, string Name);

    [Fact]
    public void DoesNothing_GivenNoAsNoTrackingWithIdentityResolution()
    {
        var spec1 = new Specification<Customer>();
        var spec2 = new Specification<Customer, string>();

        spec1.AsNoTrackingWithIdentityResolution.Should().Be(false);
        spec2.AsNoTrackingWithIdentityResolution.Should().Be(false);
    }

    [Fact]
    public void DoesNothing_GivenAsNoTrackingWithIdentityResolutionWithFalseCondition()
    {
        var spec1 = new Specification<Customer>();
        spec1.Query
            .AsNoTrackingWithIdentityResolution(false);

        var spec2 = new Specification<Customer, string>();
        spec2.Query
            .AsNoTrackingWithIdentityResolution(false);

        spec1.AsNoTrackingWithIdentityResolution.Should().Be(false);
        spec2.AsNoTrackingWithIdentityResolution.Should().Be(false);
    }

    [Fact]
    public void SetsAsNoTracking_GivenAsNoTrackingWithIdentityResolution()
    {
        var spec1 = new Specification<Customer>();
        spec1.Query
            .AsNoTrackingWithIdentityResolution();

        var spec2 = new Specification<Customer, string>();
        spec2.Query
            .AsNoTrackingWithIdentityResolution();

        spec1.AsNoTrackingWithIdentityResolution.Should().Be(true);
        spec2.AsNoTrackingWithIdentityResolution.Should().Be(true);
    }

    [Fact]
    public void SetsAsNoTracking_GivenAsNoTrackingAndAsNoTrackingWithIdentityResolution()
    {
        var spec1 = new Specification<Customer>();
        spec1.Query
            .AsNoTracking()
            .AsNoTrackingWithIdentityResolution();

        var spec2 = new Specification<Customer>();
        spec2.Query
            .AsNoTracking()
            .AsNoTrackingWithIdentityResolution();

        spec1.AsNoTracking.Should().Be(false);
        spec1.AsNoTrackingWithIdentityResolution.Should().Be(true);
        spec2.AsNoTracking.Should().Be(false);
        spec2.AsNoTrackingWithIdentityResolution.Should().Be(true);
    }
}
