using FluentAssertions;
using Pozitron.QuerySpecification.Tests.Fixture.Entities;
using Pozitron.QuerySpecification.Tests.Fixture.Specs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Pozitron.QuerySpecification.Tests
{
    public class SpecificationBuilderExtensions_OrderByDescending
    {
        [Fact]
        public void AddsNothingToList_GivenNoOrderExpression()
        {
            var spec = new StoreEmptySpec();

            spec.OrderExpressions.Should().BeEmpty();
        }

        [Fact]
        public void AddsOrderExpressionToListWithOrderByDescendingType_GivenOrderByDescendingExpression()
        {
            var spec = new StoresOrderedDescendingByNameSpec();

            spec.OrderExpressions.Should().ContainSingle();
            spec.OrderExpressions.Single().OrderType.Should().Be(OrderTypeEnum.OrderByDescending);
        }
    }
}
