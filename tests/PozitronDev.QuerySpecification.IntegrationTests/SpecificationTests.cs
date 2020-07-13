using FluentAssertions;
using PozitronDev.QuerySpecification.IntegrationTests.Data.Seeds;
using PozitronDev.QuerySpecification.IntegrationTests.Specs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PozitronDev.QuerySpecification.IntegrationTests
{
    // Testing the "Include" and various repository methods should be done in QuerySpecification.EF implementation package.
    public class SpecificationTests : SpecificationTestBase
    {

        [Fact]
        public async Task GetStoreWithId10_Using_StoreByIdSpec()
        {
            var store = await storeRepository.GetBySpecAsync(new StoreByIdSpec(10));

            store.Id.Should().Be(10);
        }

        [Fact]
        public async Task GetStoreWithIdFrom15To30_Using_StoresByIdListSpec()
        {
            var ids = Enumerable.Range(15, 16);
            var stores = await storeRepository.ListAsync(new StoresByIdListSpec(ids));

            stores.Count.Should().Be(16);
            stores.OrderBy(x=>x.Id).First().Id.Should().Be(15);
            stores.OrderBy(x=>x.Id).Last().Id.Should().Be(30);
        }

        [Fact]
        public async Task GetSecondPageOfStoreNames_Using_StoreNamesPaginatedSpec()
        {
            int take = 10; // pagesize 10
            int skip = (2 - 1) * 10; // page 2

            var storeNames = await storeRepository.ListAsync(new StoreNamesPaginatedSpec(skip, take));

            storeNames.Count.Should().Be(take);
            storeNames.First().Should().Be("Store 11");
            storeNames.Last().Should().Be("Store 20");
        }

        [Fact]
        public async Task GetSecondPageOfStores_Using_StoresPaginatedSpec()
        {
            int take = 10; // pagesize 10
            int skip = (2 - 1) * 10; // page 2

            var stores = await storeRepository.ListAsync(new StoresPaginatedSpec(skip, take));

            stores.Count.Should().Be(take);
            stores.OrderBy(x=>x.Id).First().Id.Should().Be(11);
            stores.OrderBy(x=>x.Id).Last().Id.Should().Be(20);
        }

        [Fact]
        public async Task GetOrderStoresByNameDescForCompanyWithId2_Using_StoresByCompanyOrderedDescByNameSpec()
        {
            var stores = await storeRepository.ListAsync(new StoresByCompanyOrderedDescByNameSpec(2));

            stores.First().Id.Should().Be(StoreSeed.ORDERED_BY_NAME_DESC_FOR_COMPANY2_FIRST_ID);
            stores.Last().Id.Should().Be(StoreSeed.ORDERED_BY_NAME_DESC_FOR_COMPANY2_LAST_ID);
        }

        [Fact]
        public async Task GetOrderStoresByNameDescThenByIdForCompanyWithId2_Using_StoresByCompanyOrderedDescByNameThenByIdSpec()
        {
            var stores = await storeRepository.ListAsync(new StoresByCompanyOrderedDescByNameThenByIdSpec(2));

            stores.First().Id.Should().Be(51);
            stores.Last().Id.Should().Be(100);
        }

        [Fact]
        public async Task GetFirstPageOfStoresForCompanyWithId2_Using_StoresByCompanyPaginatedOrderedDescByNameSpec()
        {
            int take = 10; // pagesize 10
            int skip = (1 - 1) * 10; // page 1

            var stores = await storeRepository.ListAsync(new StoresByCompanyPaginatedOrderedDescByNameSpec(2, skip, take));

            stores.Count.Should().Be(take);
            stores.First().Id.Should().Be(StoreSeed.ORDERED_BY_NAME_DESC_FOR_COMPANY2_FIRST_ID);
            stores.Last().Id.Should().Be(90);
        }

        [Fact]
        public async Task GetSecondPageOfStoresForCompanyWithId2_Using_StoresByCompanyPaginatedSpec()
        {
            int take = 10; // pagesize 10
            int skip = (2 - 1) * 10; // page 2

            var stores = await storeRepository.ListAsync(new StoresByCompanyPaginatedSpec(2, skip, take));

            stores.Count.Should().Be(take);
            stores.OrderBy(x => x.Id).First().Id.Should().Be(61);
            stores.OrderBy(x => x.Id).Last().Id.Should().Be(70);
        }

        [Fact]
        public async Task GetOrderedStores_Using_StoresOrderedSpecByName()
        {
            var stores = await storeRepository.ListAsync(new StoresOrderedSpecByName());

            stores.First().Id.Should().Be(StoreSeed.ORDERED_BY_NAME_FIRST_ID);
            stores.Last().Id.Should().Be(StoreSeed.ORDERED_BY_NAME_LAST_ID);
        }

        [Fact]
        public async Task GetOrderedStores_Using_StoresOrderedDescendingByNameSpec()
        {
            var stores = await storeRepository.ListAsync(new StoresOrderedDescendingByNameSpec());

            stores.First().Id.Should().Be(StoreSeed.ORDERED_BY_NAME_DESC_FIRST_ID);
            stores.Last().Id.Should().Be(StoreSeed.ORDERED_BY_NAME_DESC_LAST_ID);
        }
    }
}
