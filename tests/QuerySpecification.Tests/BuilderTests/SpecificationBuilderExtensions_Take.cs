namespace Pozitron.QuerySpecification.Tests;

public class SpecificationBuilderExtensions_Take
{
    [Fact]
    public void SetsTakeProperty_GivenValue()
    {
        var take = 10;
        var spec = new StoreNamesPaginatedSpec(0, take);

        spec.Take.Should().Be(take);
    }

    [Fact]
    public void DoesNothing_GivenTakeWithFalseCondition()
    {
        var spec = new CompanyByIdWithFalseConditions(1);

        spec.Take.Should().BeLessThan(0);
    }
}
