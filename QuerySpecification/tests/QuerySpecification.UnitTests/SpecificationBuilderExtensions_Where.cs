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
    public class SpecificationBuilderExtensions_Where
    {
        [Fact]
        public void AddsNothingToList_GivenNoWhereExpression()
        {
            var spec = new StoreEmptySpec();

            spec.WhereExpressions.Should().BeEmpty();
        }

        [Fact]
        public void AddsOneExpressionToList_GivenOneWhereExpression()
        {
            var spec = new StoreByIdSpec(1);

            spec.WhereExpressions.Should().ContainSingle();
        }

        [Fact]
        public void AddsTwoExpressionsToList_GivenTwoWhereExpressions()
        {
            var spec = new StoreByIdAndNameSpec(1, "name");

            spec.WhereExpressions.Should().HaveCount(2);
        }
    }
}
