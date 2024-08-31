using FluentAssertions;
using Pozitron.QuerySpecification.Tests.Fixture.Specs;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Pozitron.QuerySpecification.Tests.BuilderTests
{
    public class SpecificationBuilderExtensions_AsNoTrackingWithIdentityResolution
    {
        [Fact]
        public void DoesNothing_GivenSpecWithoutAsNoTracking()
        {
            var spec = new StoreEmptySpec();

            spec.AsNoTrackingWithIdentityResolution.Should().Be(false);
        }

        [Fact]
        public void FlagsAsNoTracking_GivenSpecWithAsNoTracking()
        {
            var spec = new CompanyByIdAsUntrackedWithIdentityResolutionSpec(1);

            spec.AsNoTrackingWithIdentityResolution.Should().Be(true);
        }
    }
}
