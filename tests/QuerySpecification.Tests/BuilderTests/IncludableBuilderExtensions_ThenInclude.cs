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
    public class IncludableBuilderExtensions_ThenInclude
    {
        [Fact]
        public void AppendIncludeExpressionInfoToListWithTypeThenInclude_GivenThenIncludeExpression()
        {
            var spec = new StoreIncludeCompanyThenCountrySpec();

            var includeExpressions = spec.IncludeExpressions.ToList();

            // The list must have two items, since ThenInclude can be applied once the first level is applied.
            includeExpressions.Should().HaveCount(2);

            includeExpressions[1].Type.Should().Be(IncludeTypeEnum.ThenInclude);
        }
    }
}
