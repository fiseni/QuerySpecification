namespace Pozitron.QuerySpecification.Tests;

public class SpecificationBuilderExtensions_Skip
{
    [Fact]
    public void SetsSkipProperty_GivenValue()
    {
        var skip = 1;

        var spec = new StoreNamesPaginatedSpec(skip, 10);

        spec.Skip.Should()
            .Be(skip);
    }

    [Fact]
    public void DoesNothing_GivenSkipWithFalseCondition()
    {
        var spec = new CompanyByIdWithFalseConditions(1);

        spec.Skip.Should().BeLessThan(0);
    }
}
