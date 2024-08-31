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
