using FluentAssertions;
using Pozitron.QuerySpecification.Tests.Fixture.Specs;
using Xunit;

namespace Pozitron.QuerySpecification.Tests;

public class SpecificationBuilderExtensions_OrderBy
{
    [Fact]
    public void AddsNothingToList_GivenNoOrderExpression()
    {
        var spec = new StoreEmptySpec();

        spec.OrderExpressions.Should().BeEmpty();
    }

    [Fact]
    public void AddsOrderExpressionToListWithOrderByType_GivenOrderByExpression()
    {
        var spec = new StoresOrderedSpecByName();

        spec.OrderExpressions.Should().ContainSingle();
        spec.OrderExpressions.Single().OrderType.Should().Be(OrderTypeEnum.OrderBy);
    }
}
