using FluentAssertions;
using PozitronDev.QuerySpecification.EntityFrameworkCore3.IntegrationTests.Fixture;
using PozitronDev.QuerySpecification.UnitTests.Fixture.Entities.Seeds;
using PozitronDev.QuerySpecification.UnitTests.Fixture.Specs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PozitronDev.QuerySpecification.EntityFrameworkCore3.IntegrationTests
{
    public class Repository_GetById : IntegrationTestBase
    {
        [Fact]
        public async Task ReturnsStore_GivenId()
        {
            var result = await storeRepository.GetByIdAsync(StoreSeed.VALID_STORE_ID);

            result.Should().NotBeNull();
            result!.Name.Should().Be(StoreSeed.VALID_STORE_NAME);
        }

        [Fact]
        public async Task ReturnsStore_GivenGenericId()
        {
            var result = await storeRepository.GetByIdAsync<int>(StoreSeed.VALID_STORE_ID);

            result.Should().NotBeNull();
            result!.Name.Should().Be(StoreSeed.VALID_STORE_NAME);
        }
    }
}
