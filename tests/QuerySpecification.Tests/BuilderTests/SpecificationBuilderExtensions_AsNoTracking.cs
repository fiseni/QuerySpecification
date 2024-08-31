using FluentAssertions;
using PozitronDev.QuerySpecification.UnitTests.Fixture.Specs;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace PozitronDev.QuerySpecification.UnitTests.BuilderTests
{
    public class SpecificationBuilderExtensions_AsNoTracking
    {
        [Fact]
        public void DoesNothing_GivenSpecWithoutAsNoTracking()
        {
            var spec = new StoreEmptySpec();

            spec.AsNoTracking.Should().Be(false);
        }

        [Fact]
        public void FlagsAsNoTracking_GivenSpecWithAsNoTracking()
        {
            var spec = new CompanyByIdAsUntrackedSpec(1);

            spec.AsNoTracking.Should().Be(true);
        }
    }
}
