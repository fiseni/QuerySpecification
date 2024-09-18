﻿namespace Tests.Repositories;

[Collection("SharedCollection")]
public class Repository_ListTests(TestFactory factory) : IntegrationTest(factory)
{
    public record CountryDto(string? Name);

    [Fact]
    public async Task ListAsync_ReturnsAllItems()
    {
        var expected = new List<Country>
        {
            new() { Name = "a" },
            new() { Name = "b" },
            new() { Name = "c" },
        };
        await SeedRangeAsync(expected);

        var repo = new Repository<Country>(DbContext);

        var result = await repo.ListAsync();

        result.Should().HaveSameCount(expected);
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task ListAsync_ReturnsFilteredItems_GivenSpec()
    {
        var expected = new List<Country>
        {
            new() { Name = "b" },
            new() { Name = "b" },
            new() { Name = "b" },
        };
        await SeedRangeAsync(
        [
            new() { Name = "a" },
            new() { Name = "c" },
            .. expected,
            new() { Name = "d" },
        ]);

        var repo = new Repository<Country>(DbContext);
        var spec = new Specification<Country>();
        spec.Query
            .Where(x => x.Name == "b");

        var result = await repo.ListAsync(spec);

        result.Should().HaveSameCount(expected);
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task ListAsync_ReturnsFilteredItems_GivenProjectionSpec()
    {
        var expected = new List<CountryDto>
        {
            new("b"),
            new("b"),
            new("b"),
        };
        await SeedRangeAsync<Country>(
        [
            new() { Name = "a" },
            new() { Name = "c" },
            new() { Name = "b" },
            new() { Name = "b" },
            new() { Name = "b" },
            new() { Name = "d" },
        ]);

        var repo = new Repository<Country>(DbContext);
        var spec = new Specification<Country, CountryDto>();
        spec.Query
            .Where(x => x.Name == "b")
            .Select(x => new CountryDto(x.Name));

        var result = await repo.ListAsync(spec);

        result.Should().HaveSameCount(expected);
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task AsAsyncEnumerable_ReturnsFilteredItems_GivenSpec()
    {
        var expected = new List<Country>
        {
            new() { Name = "b1" },
            new() { Name = "b2" },
            new() { Name = "b3" },
        };
        await SeedRangeAsync(
        [
            new() { Name = "a" },
            new() { Name = "c" },
            .. expected,
            new() { Name = null },
        ]);

        var repo = new Repository<Country>(DbContext);
        var spec = new Specification<Country>();
        spec.Query
            .Like(x => x.Name, "b%")
            .OrderBy(x => x.Name);

        var suffix = 1;
        await foreach (var item in repo.AsAsyncEnumerable(spec))
        {
            item.Name.Should().Be($"b{suffix++}");
        }
    }
}
