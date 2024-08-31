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
}
