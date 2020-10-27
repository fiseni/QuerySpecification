using FluentAssertions;
using PozitronDev.QuerySpecification.UnitTests.Fixture.Entities;
using PozitronDev.QuerySpecification.UnitTests.Fixture.Specs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace PozitronDev.QuerySpecification.UnitTests
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
