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
    public class OrderedBuilderExtensions_ThenBy
    {
        [Fact]
        public void AppendOrderExpressionToListWithThenByType_GivenThenByExpression()
        {
            var spec = new StoresByCompanyOrderedDescByNameThenByIdSpec(1);

            var orderExpressions = spec.OrderExpressions.ToList();

            // The list must have two items, since Then can be applied once the first level is applied.
            orderExpressions.Should().HaveCount(2);

            orderExpressions[1].OrderType.Should().Be(OrderTypeEnum.ThenBy);
        }
    }
}
