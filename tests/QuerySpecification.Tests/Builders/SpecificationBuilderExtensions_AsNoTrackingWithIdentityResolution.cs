namespace Pozitron.QuerySpecification.Tests;

public class SpecificationBuilderExtensions_AsNoTrackingWithIdentityResolution
{
    [Fact]
    public void DoesNothing_GivenSpecWithoutAsNoTrackingWithIdentityResolution()
    {
        var spec = new StoreEmptySpec();

        spec.AsNoTrackingWithIdentityResolution.Should().Be(false);
    }

    [Fact]
    public void DoesNothing_GivenAsNoTrackingWithIdentityResolutionWithFalseCondition()
    {
        var spec = new CompanyByIdWithFalseConditions(1);

        spec.AsNoTrackingWithIdentityResolution.Should().Be(false);
    }

    [Fact]
    public void FlagsAsNoTracking_GivenSpecWithAsNoTrackingWithIdentityResolution()
    {
        var spec = new CompanyByIdAsUntrackedWithIdentityResolutionSpec(1);

        spec.AsNoTrackingWithIdentityResolution.Should().Be(true);
    }

    [Fact]
    // TODO: Finish FlagsAsNoTracking_GivenSpecWithAsTrackingAndEndWithAsNoTrackingWithIdentityResolution. [fatii, 03/09/2024]
    public void FlagsAsNoTracking_GivenSpecWithAsTrackingAndEndWithAsNoTrackingWithIdentityResolution()
    {
        Assert.Equal(1, 1);
    }
}
