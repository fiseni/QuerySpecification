﻿using FluentAssertions;
using Pozitron.QuerySpecification.EntityFrameworkCore.Tests.Fixture;
using Pozitron.QuerySpecification.Tests.Fixture.Entities.Seeds;
using Pozitron.QuerySpecification.Tests.Fixture.Specs;
using Pozitron.QuerySpecification.Tests;
using Pozitron.QuerySpecification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Pozitron.QuerySpecification.EntityFrameworkCore.Tests
{
    public class Repository_GetBySpec : IntegrationTestBase
    {
        public Repository_GetBySpec(SharedDatabaseFixture fixture) : base(fixture) { }

        [Fact]
        public async Task ReturnsStoreWithProducts_GivenStoreByIdIncludeProductsSpec()
        {
            var result = await storeRepository.GetBySpecAsync(new StoreByIdIncludeProductsSpec(StoreSeed.VALID_STORE_ID));

            result.Should().NotBeNull();
            result!.Name.Should().Be(StoreSeed.VALID_STORE_NAME);
            result.Products.Count.Should().BeGreaterThan(1);
        }

        [Fact]
        public async Task ReturnsStoreWithAddress_GivenStoreByIdIncludeAddressSpec()
        {
            var result = await storeRepository.GetBySpecAsync(new StoreByIdIncludeAddressSpec(StoreSeed.VALID_STORE_ID));

            result.Should().NotBeNull();
            result!.Name.Should().Be(StoreSeed.VALID_STORE_NAME);
            result.Address?.Street.Should().Be(AddressSeed.VALID_STREET_FOR_STOREID1);
        }

        [Fact]
        public async Task ReturnsStoreWithAddressAndProduct_GivenStoreByIdIncludeAddressAndProductsSpec()
        {
            var result = await storeRepository.GetBySpecAsync(new StoreByIdIncludeAddressAndProductsSpec(StoreSeed.VALID_STORE_ID));

            result.Should().NotBeNull();
            result!.Name.Should().Be(StoreSeed.VALID_STORE_NAME);
            result.Products.Count.Should().BeGreaterThan(1);
            result.Address?.Street.Should().Be(AddressSeed.VALID_STREET_FOR_STOREID1);
        }

        [Fact]
        public async Task ReturnsStoreWithProducts_GivenStoreByIdIncludeProductsUsingStringSpec()
        {
            var result = await storeRepository.GetBySpecAsync(new StoreByIdIncludeProductsUsingStringSpec(StoreSeed.VALID_STORE_ID));

            result.Should().NotBeNull();
            result!.Name.Should().Be(StoreSeed.VALID_STORE_NAME);
            result.Products.Count.Should().BeGreaterThan(1);
        }

        [Fact]
        public async Task ReturnsCompanyWithStoresAndAddress_GivenCompanyByIdIncludeStoresThenIncludeAddressSpec()
        {
            var result = await companyRepository.GetBySpecAsync(new CompanyByIdIncludeStoresThenIncludeAddressSpec(CompanySeed.VALID_COMPANY_ID));

            result.Should().NotBeNull();
            result!.Name.Should().Be(CompanySeed.VALID_COMPANY_NAME);
            result.Stores.Count.Should().BeGreaterThan(49);
            result.Stores.Select(x => x.Address).Count().Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task ReturnsCompanyWithStoresAndProducts_GivenCompanyByIdIncludeStoresThenIncludeProductsSpec()
        {
            var result = await companyRepository.GetBySpecAsync(new CompanyByIdIncludeStoresThenIncludeProductsSpec(CompanySeed.VALID_COMPANY_ID));

            result.Should().NotBeNull();
            result!.Name.Should().Be(CompanySeed.VALID_COMPANY_NAME);
            result.Stores.Count.Should().BeGreaterThan(49);
            result.Stores.Select(x => x.Products).Count().Should().BeGreaterThan(1);
        }

        [Fact]
        public async Task ReturnsUntrackedCompany_GivenCompanyByIdAsUntrackedSpec()
        {
            dbContext.ChangeTracker.Clear();

            var result = await companyRepository.GetBySpecAsync(new CompanyByIdAsUntrackedSpec(CompanySeed.VALID_COMPANY_ID));

            result.Should().NotBeNull();
            result?.Name.Should().Be(CompanySeed.VALID_COMPANY_NAME);
            dbContext.Entry(result).State.Should().Be(Microsoft.EntityFrameworkCore.EntityState.Detached);
        }
    }
}
