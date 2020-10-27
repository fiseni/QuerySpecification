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
    public class SpecificationBuilderExtensions_Skip
    {
        [Fact]
        public void SetsSkipProperty_GivenValue()
        {
            var skip = 1;

            var spec = new StoreNamesPaginatedSpec(skip, 10);

            spec.Skip.Should().Be(skip);
            spec.IsPagingEnabled.Should().BeTrue();
        }

        [Fact]
        public void ThrowsDuplicateSkipException_GivenSkipUsedMoreThanOnce()
        {
            Assert.Throws<DuplicateSkipException>(() => new StoreDuplicateSkipSpec());
        }
    }
}
