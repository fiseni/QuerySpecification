﻿using FluentAssertions;
using Pozitron.QuerySpecification.EntityFrameworkCore.Tests.Fixture;
using Pozitron.QuerySpecification.Tests.Fixture.Entities.Seeds;
using Pozitron.QuerySpecification.Tests.Fixture.Specs;
using Xunit;

namespace Pozitron.QuerySpecification.EntityFrameworkCore.Tests;

public class Repository_ListAsync : IntegrationTestBase
{
    public Repository_ListAsync(SharedDatabaseFixture fixture) : base(fixture) { }

    [Fact]
    public async Task ReturnsStoreWithProducts_GivenStoreIncludeProductsSpec()
    {
        var result = await storeRepository.ListAsync(new StoreIncludeProductsSpec());

        result.Should().NotBeNull();
        result.Should().NotBeEmpty();
        result[0].Products.Should().NotBeEmpty();
    }

    [Fact]
    public async Task ReturnsStoreWithAddress_GivenStoreIncludeAddressSpec()
    {
        var result = await storeRepository.ListAsync(new StoreIncludeAddressSpec());

        result.Should().NotBeNull();
        result.Should().NotBeEmpty();
        result[0].Address.Should().NotBeNull();
    }

    [Fact]
    public async Task ReturnsStoreWithAddressAndProduct_GivenStoreIncludeProductsThenStoreSpec()
    {
        var result = await storeRepository.ListAsync(new StoreIncludeProductsThenStoreSpec());

        result.Should().NotBeNull();
        result.Should().NotBeEmpty();
        result[0].Products.Should().NotBeEmpty();
        result[0].Products.First().Store.Should().NotBeNull();
    }

    [Fact]
    public async Task ReturnsCompanyWithStoreWithIdOne_GivenCompanyIncludeFilteredStoresSpec()
    {
        var result = await companyRepository.ListAsync(new CompanyIncludeFilteredStoresSpec(1));

        result.Should().NotBeNull();
        result.Should().NotBeEmpty();
        result[0].Stores.Should().NotBeEmpty();
        result[0].Stores.Should().HaveCount(1);
        result[0].Stores.First().Id.Should().Be(1);
    }

    [Fact]
    public async Task ReturnsStoreWithIdFrom15To30_GivenStoresByIdListSpec()
    {
        var ids = Enumerable.Range(15, 16);
        var spec = new StoresByIdListSpec(ids);

        var stores = await storeRepository.ListAsync(spec);

        stores.Count.Should().Be(16);
        stores.OrderBy(x => x.Id).First().Id.Should().Be(15);
        stores.OrderBy(x => x.Id).Last().Id.Should().Be(30);
    }

    [Fact]
    public async Task ReturnsSecondPageOfStoreNames_GivenStoreNamesPaginatedSpec()
    {
        int take = 10; // pagesize 10
        int skip = (2 - 1) * 10; // page 2

        var spec = new StoreNamesPaginatedSpec(skip, take);

        var storeNames = await storeRepository.ListAsync(spec);

        storeNames.Count.Should().Be(take);
        storeNames.First().Should().Be("Store 11");
        storeNames.Last().Should().Be("Store 20");
    }

    [Fact]
    public async Task ReturnsSecondPageOfStores_GivenStoresPaginatedSpec()
    {
        int take = 10; // pagesize 10
        int skip = (2 - 1) * 10; // page 2

        var spec = new StoresPaginatedSpec(skip, take);

        var stores = await storeRepository.ListAsync(spec);

        stores.Count.Should().Be(take);
        stores.OrderBy(x => x.Id).First().Id.Should().Be(11);
        stores.OrderBy(x => x.Id).Last().Id.Should().Be(20);
    }

    [Fact]
    public async Task ReturnsOrderStoresByNameDescForCompanyWithId2_GivenStoresByCompanyOrderedDescByNameSpec()
    {
        var spec = new StoresByCompanyOrderedDescByNameSpec(2);

        var stores = await storeRepository.ListAsync(spec);

        stores.First().Id.Should().Be(StoreSeed.ORDERED_BY_NAME_DESC_FOR_COMPANY2_FIRST_ID);
        stores.Last().Id.Should().Be(StoreSeed.ORDERED_BY_NAME_DESC_FOR_COMPANY2_LAST_ID);
    }

    [Fact]
    public async Task ReturnsOrderStoresByNameDescThenByIdForCompanyWithId2_GivenStoresByCompanyOrderedDescByNameThenByIdSpec()
    {
        var spec = new StoresByCompanyOrderedDescByNameThenByIdSpec(2);

        var stores = await storeRepository.ListAsync(spec);

        stores.First().Id.Should().Be(99);
        stores.Last().Id.Should().Be(98);
    }

    [Fact]
    public async Task ReturnsSecondPageOfStoresForCompanyWithId2_GivenStoresByCompanyPaginatedOrderedDescByNameSpec()
    {
        int take = 10; // pagesize 10
        int skip = (2 - 1) * 10; // page 2

        var spec = new StoresByCompanyPaginatedOrderedDescByNameSpec(2, skip, take);

        var stores = await storeRepository.ListAsync(spec);

        stores.Count.Should().Be(take);
        stores.First().Id.Should().Be(StoreSeed.ORDERED_BY_NAME_DESC_FOR_COMPANY2_PAGE2_FIRST_ID);
        stores.Last().Id.Should().Be(StoreSeed.ORDERED_BY_NAME_DESC_FOR_COMPANY2_PAGE2_LAST_ID);
    }

    [Fact]
    public async Task ReturnsSecondPageOfStoresForCompanyWithId2_GivenStoresByCompanyPaginatedSpec()
    {
        int take = 10; // pagesize 10
        int skip = (2 - 1) * 10; // page 2

        var spec = new StoresByCompanyPaginatedSpec(2, skip, take);

        var stores = await storeRepository.ListAsync(spec);

        stores.Count.Should().Be(take);
        stores.OrderBy(x => x.Id).First().Id.Should().Be(61);
        stores.OrderBy(x => x.Id).Last().Id.Should().Be(70);
    }

    [Fact]
    public async Task ReturnsOrderedStores_GivenStoresOrderedSpecByName()
    {
        var spec = new StoresOrderedSpecByName();

        var stores = await storeRepository.ListAsync(spec);

        stores.First().Id.Should().Be(StoreSeed.ORDERED_BY_NAME_FIRST_ID);
        stores.Last().Id.Should().Be(StoreSeed.ORDERED_BY_NAME_LAST_ID);
    }

    [Fact]
    public async Task ReturnsOrderedStores_GivenStoresOrderedDescendingByNameSpec()
    {
        var spec = new StoresOrderedDescendingByNameSpec();

        var stores = await storeRepository.ListAsync(spec);

        stores.First().Id.Should().Be(StoreSeed.ORDERED_BY_NAME_DESC_FIRST_ID);
        stores.Last().Id.Should().Be(StoreSeed.ORDERED_BY_NAME_DESC_LAST_ID);
    }

    [Fact]
    public async Task ReturnsStoreContainingCity1_GivenStoreIncludeProductsSpec()
    {
        var result = await storeRepository.ListAsync(new StoreSearchByNameOrCitySpec(StoreSeed.VALID_Search_City_Key));

        result.Should().NotBeNull();
        result.Should().ContainSingle();
        result[0].Id.Should().Be(StoreSeed.VALID_Search_City_ID);
        result[0].City.Should().Contain(StoreSeed.VALID_Search_City_Key);
    }
}
