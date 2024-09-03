namespace Pozitron.QuerySpecification.Tests;

public class SpecificationBuilderExtensions_AsNoTracking
{
    [Fact]
    public void DoesNothing_GivenSpecWithoutAsNoTracking()
    {
        var spec = new StoreEmptySpec();

        spec.AsNoTracking.Should().Be(false);
    }

    [Fact]
    public void DoesNothing_GivenAsNoTrackingWithFalseCondition()
    {
        var spec = new CompanyByIdWithFalseConditions(1);

        spec.AsNoTracking.Should().Be(false);
    }

    [Fact]
    public void FlagsAsNoTracking_GivenSpecWithAsNoTracking()
    {
        var spec = new CompanyByIdAsUntrackedSpec(1);

        spec.AsNoTracking.Should().Be(true);
    }

    [Fact]
    // TODO: Finish SpecificationBuilderExtensions_AsNoTracking. [fatii, 03/09/2024]
    public void FlagsAsNoTracking_GivenSpecWithAsTrackingAndEndWithAsNoTracking()
    {
        Assert.Equal(1, 1);
    }
}
