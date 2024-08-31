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
    public class SpecificationBuilderExtensions_Include
    {
        [Fact]
        public void AddsNothingToList_GivenNoIncludeExpression()
        {
            var spec = new StoreEmptySpec();

            spec.IncludeExpressions.Should().BeEmpty();
        }

        [Fact]
        public void AddsIncludeExpressionInfoToListWithTypeInclude_GivenIncludeExpression()
        {
            var spec = new StoreIncludeAddressSpec();

            spec.IncludeExpressions.Should().ContainSingle();
            spec.IncludeExpressions.Single().Type.Should().Be(IncludeTypeEnum.Include);
        }
    }
}
