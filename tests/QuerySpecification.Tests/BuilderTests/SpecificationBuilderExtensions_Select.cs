using FluentAssertions;
using Pozitron.QuerySpecification.Tests.Fixture.Specs;
using Xunit;

namespace Pozitron.QuerySpecification.Tests;

public class SpecificationBuilderExtensions_Select
{
    [Fact]
    public void SetsNothing_GivenNoSelectExpression()
    {
        var spec = new StoreNamesEmptySpec();

        spec.Selector.Should().BeNull();
    }

    [Fact]
    public void SetsSelector_GivenSelectExpression()
    {
        var spec = new StoreNamesSpec();

        spec.Selector.Should().NotBeNull();
    }
}
