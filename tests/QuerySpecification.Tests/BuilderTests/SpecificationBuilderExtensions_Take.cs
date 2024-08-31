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
    public class SpecificationBuilderExtensions_Take
    {
        [Fact]
        public void SetsTakeProperty_GivenValue()
        {
            var take = 10;
            var spec = new StoreNamesPaginatedSpec(0, take);

            spec.Take.Should().Be(take);
            spec.IsPagingEnabled.Should().BeTrue();
        }

        [Fact]
        public void ThrowsDuplicateTakeException_GivenTakeUsedMoreThanOnce()
        {
            Assert.Throws<DuplicateTakeException>(() => new StoreDuplicateTakeSpec());
        }
    }
}
