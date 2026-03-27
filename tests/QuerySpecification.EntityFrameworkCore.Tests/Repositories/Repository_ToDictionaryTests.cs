namespace Tests.Repositories;

[Collection("SharedCollection")]
public class Repository_ToDictionaryTests(TestFactory factory) : IntegrationTest(factory)
{
    public record CountryDto(int Id, string? Name);

    [Fact]
    public async Task ToDictionaryAsync_ReturnsAllItems()
    {
        var expected = new List<Country>
        {
            new() { Name = "a" },
            new() { Name = "b" },
            new() { Name = "c" },
        };
        await SeedRangeAsync(expected);

        var repo = new Repository<Country>(DbContext);

        var result = await repo.ToDictionaryAsync(x => x.Id);

        result.Should().HaveSameCount(expected);
        result.Values.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task ToDictionaryAsync_ReturnsFilteredItems_GivenSpec()
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

        var result = await repo.ToDictionaryAsync(spec, x => x.Id);

        result.Should().HaveSameCount(expected);
        result.Values.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task ToDictionaryAsync_ReturnsFilteredItems_GivenProjectionSpec()
    {
        var seeded = new List<Country>
        {
            new() { Name = "b" },
            new() { Name = "b" },
            new() { Name = "b" },
        };
        await SeedRangeAsync<Country>(
        [
            new() { Name = "a" },
            new() { Name = "c" },
            .. seeded,
            new() { Name = "d" },
        ]);

        var repo = new Repository<Country>(DbContext);
        var spec = new Specification<Country, CountryDto>();
        spec.Query
            .Where(x => x.Name == "b")
            .Select(x => new CountryDto(x.Id, x.Name));

        var result = await repo.ToDictionaryAsync(spec, x => x.Id);

        result.Should().HaveCount(seeded.Count);
        result.Values.Should().AllSatisfy(dto => dto.Name.Should().Be("b"));
    }
}
