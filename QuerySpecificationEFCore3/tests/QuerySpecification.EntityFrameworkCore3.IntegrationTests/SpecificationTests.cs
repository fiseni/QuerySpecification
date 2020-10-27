using FluentAssertions;
using PozitronDev.QuerySpecification.EntityFrameworkCore3.IntegrationTests.Fixture;
using PozitronDev.QuerySpecification.EntityFrameworkCore3.IntegrationTests.Fixture.Entities.Seeds;
using PozitronDev.QuerySpecification.EntityFrameworkCore3.IntegrationTests.Fixture.Specs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PozitronDev.QuerySpecification.EntityFrameworkCore3.IntegrationTests
{
    public class SpecificationTests : SpecificationTestBase
    {

        [Fact]
        public async Task ReturnsStoreWithProducts_GivenStoreByIdIncludeProductsSpec()
        {
            var result = (await storeRepository.ListAsync(new StoreByIdIncludeProductsSpec(StoreSeed.VALID_STORE_ID))).SingleOrDefault();

            result.Should().NotBeNull();
            result.Name.Should().Be(StoreSeed.VALID_STORE_NAME);
            result.Products.Count.Should().BeGreaterThan(1);
        }

        [Fact]
        public async Task ReturnsStoreWithAddress_GivenStoreByIdIncludeAddressSpec()
        {
            var result = (await storeRepository.ListAsync(new StoreByIdIncludeAddressSpec(StoreSeed.VALID_STORE_ID))).SingleOrDefault();

            result.Should().NotBeNull();
            result.Name.Should().Be(StoreSeed.VALID_STORE_NAME);
            result.Address?.Street.Should().Be(AddressSeed.VALID_STREET_FOR_STOREID1);
        }

        [Fact]
        public async Task ReturnsStoreWithAddressAndProduct_GivenStoreByIdIncludeAddressAndProductsSpec()
        {
            var result = (await storeRepository.ListAsync(new StoreByIdIncludeAddressAndProductsSpec(StoreSeed.VALID_STORE_ID))).SingleOrDefault();

            result.Should().NotBeNull();
            result.Name.Should().Be(StoreSeed.VALID_STORE_NAME);
            result.Products.Count.Should().BeGreaterThan(1);
            result.Address?.Street.Should().Be(AddressSeed.VALID_STREET_FOR_STOREID1);
        }

        [Fact]
        public async Task ReturnsStoreWithProducts_GivenStoreByIdIncludeProductsUsingStringSpec()
        {
            var result = (await storeRepository.ListAsync(new StoreByIdIncludeProductsUsingStringSpec(StoreSeed.VALID_STORE_ID))).SingleOrDefault();

            result.Should().NotBeNull();
            result.Name.Should().Be(StoreSeed.VALID_STORE_NAME);
            result.Products.Count.Should().BeGreaterThan(1);
        }

        [Fact]
        public async Task ReturnsCompanyWithStoresAndAddress_GivenCompanyByIdIncludeStoresThenIncludeAddressSpec()
        {
            var result = (await companyRepository.ListAsync(new CompanyByIdIncludeStoresThenIncludeAddressSpec(CompanySeed.VALID_COMPANY_ID))).SingleOrDefault();

            result.Should().NotBeNull();
            result.Name.Should().Be(CompanySeed.VALID_COMPANY_NAME);
            result.Stores.Count.Should().BeGreaterThan(49);
            result.Stores.Select(x => x.Address).Count().Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task ReturnsCompanyWithStoresAndProducts_GivenCompanyByIdIncludeStoresThenIncludeProductsSpec()
        {
            var result = (await companyRepository.ListAsync(new CompanyByIdIncludeStoresThenIncludeProductsSpec(CompanySeed.VALID_COMPANY_ID))).SingleOrDefault();

            result.Should().NotBeNull();
            result.Name.Should().Be(CompanySeed.VALID_COMPANY_NAME);
            result.Stores.Count.Should().BeGreaterThan(49);
            result.Stores.Select(x => x.Products).Count().Should().BeGreaterThan(1);
        }
    }
}
