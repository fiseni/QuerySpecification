﻿using FluentAssertions;
using Pozitron.QuerySpecification.EntityFrameworkCore.Tests.Fixture;
using Pozitron.QuerySpecification.Tests.Fixture.Entities.Seeds;
using Pozitron.QuerySpecification.Tests.Fixture.Specs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Pozitron.QuerySpecification.EntityFrameworkCore.Tests
{
    public class Repository_GetById : IntegrationTestBase
    {
        public Repository_GetById(SharedDatabaseFixture fixture) : base(fixture) { }

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
