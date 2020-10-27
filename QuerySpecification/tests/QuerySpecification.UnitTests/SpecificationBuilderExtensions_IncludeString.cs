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
    public class SpecificationBuilderExtensions_IncludeString
    {
        [Fact]
        public void AddsNothingToList_GivenNoIncludeStringExpression()
        {
            var spec = new StoreEmptySpec();

            spec.WhereExpressions.Should().BeEmpty();
        }

        [Fact]
        public void AddsIncludeStringToList_GivenString()
        {
            var spec = new StoreIncludeCompanyThenCountryAsStringSpec();

            var expected = $"{nameof(Company)}.{nameof(Company.Country)}";

            spec.IncludeStrings.Should().ContainSingle();
            spec.IncludeStrings.Single().Should().Be(expected);
        }
    }
}
