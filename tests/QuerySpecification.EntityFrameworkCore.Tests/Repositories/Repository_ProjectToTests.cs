using AutoMapper;

namespace QuerySpecification.EntityFrameworkCore.Tests.Repositories;

[Collection("SharedCollection")]
public class Repository_ProjectToTests(TestFactory factory) : IntegrationTest(factory)
{
    public record CountryDto(int No, string Name);
    public class CountryDtoProfile : Profile
    {
        public CountryDtoProfile()
        {
            CreateMap<Country, CountryDto>();
        }
    }

    [Fact]
    public async Task ProjectToFirstAsync_ReturnsProjectedItem_GivenSpec()
    {
        var expected = new CountryDto(1, "b");
        await SeedRangeAsync<Country>(
        [
            new() { No = 9, Name = "a" },
            new() { No = 9, Name = "c" },
            new() { No = 1, Name = "b" },
            new() { No = 2, Name = "b" },
            new() { No = 3, Name = "b" },
            new() { No = 9, Name = "d" },
        ]);

        var repo = new Repository<Country>(DbContext);
        var spec = new Specification<Country>();
        spec.Query
            .Where(x => x.Name == "b")
            .OrderBy(x => x.No);

        var result = await repo.ProjectToFirstAsync<CountryDto>(spec);

        result.Should().NotBeNull();
        result.Should().Be(expected);
    }

    [Fact]
    public async Task ProjectToFirstAsync_ThrowsEntityNotFound_GivenSpecWithNoMatch()
    {
        await SeedRangeAsync<Country>(
        [
            new() { No = 9, Name = "a" },
            new() { No = 9, Name = "c" },
            new() { No = 1, Name = "b" },
            new() { No = 2, Name = "b" },
            new() { No = 3, Name = "b" },
            new() { No = 9, Name = "d" },
        ]);

        var repo = new Repository<Country>(DbContext);
        var spec = new Specification<Country>();
        spec.Query
            .Where(x => x.Name == "x")
            .OrderBy(x => x.No);

        var result = () => repo.ProjectToFirstAsync<CountryDto>(spec);

        await result.Should().ThrowAsync<EntityNotFoundException>();
    }

    [Fact]
    public async Task ProjectToFirstOrDefaultAsync_ReturnsProjectedItem_GivenSpec()
    {
        var expected = new CountryDto(1, "b");
        await SeedRangeAsync<Country>(
        [
            new() { No = 9, Name = "a" },
            new() { No = 9, Name = "c" },
            new() { No = 1, Name = "b" },
            new() { No = 2, Name = "b" },
            new() { No = 3, Name = "b" },
            new() { No = 9, Name = "d" },
        ]);

        var repo = new Repository<Country>(DbContext);
        var spec = new Specification<Country>();
        spec.Query
            .Where(x => x.Name == "b")
            .OrderBy(x => x.No);

        var result = await repo.ProjectToFirstOrDefaultAsync<CountryDto>(spec);

        result.Should().NotBeNull();
        result.Should().Be(expected);
    }

    [Fact]
    public async Task ProjectToFirstOrDefaultAsync_ReturnsNull_GivenSpecAndNoMatch()
    {
        await SeedRangeAsync<Country>(
        [
            new() { No = 9, Name = "a" },
            new() { No = 9, Name = "c" },
            new() { No = 1, Name = "b" },
            new() { No = 2, Name = "b" },
            new() { No = 3, Name = "b" },
            new() { No = 9, Name = "d" },
        ]);

        var repo = new Repository<Country>(DbContext);
        var spec = new Specification<Country>();
        spec.Query
            .Where(x => x.Name == "x")
            .OrderBy(x => x.No);

        var result = await repo.ProjectToFirstOrDefaultAsync<CountryDto>(spec);

        result.Should().BeNull();
    }

    [Fact]
    public async Task ProjectToListAsync_ReturnsProjectedItems_GivenSpec()
    {
        var expected = new List<CountryDto>
        {
            new(1, "b"),
            new(2, "b"),
            new(3, "b"),
        };
        await SeedRangeAsync<Country>(
        [
            new() { No = 9, Name = "a" },
            new() { No = 9, Name = "c" },
            new() { No = 1, Name = "b" },
            new() { No = 2, Name = "b" },
            new() { No = 3, Name = "b" },
            new() { No = 9, Name = "d" },
        ]);

        var repo = new Repository<Country>(DbContext);
        var spec = new Specification<Country>();
        spec.Query
            .Where(x => x.Name == "b")
            .OrderBy(x => x.No);

        var result = await repo.ProjectToListAsync<CountryDto>(spec);

        result.Should().HaveSameCount(expected);
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task ProjectToListAsync_ReturnsPaginatedItems_GivenSpecAndPagingFilter()
    {
        var expected = new List<CountryDto>
        {
            new(1, "b"),
            new(2, "b"),
            new(3, "b"),
            new(4, "b"),
        };
        await SeedRangeAsync<Country>(
        [
            new() { No = 9, Name = "a" },
            new() { No = 9, Name = "c" },
            new() { No = 1, Name = "b" },
            new() { No = 2, Name = "b" },
            new() { No = 3, Name = "b" },
            new() { No = 4, Name = "b" },
            new() { No = 9, Name = "d" },
        ]);

        var filter = new PagingFilter() { Page = 2, PageSize = 3 };
        var repo = new Repository<Country>(DbContext);
        var spec = new Specification<Country>();
        spec.Query
            .Where(x => x.Name == "b")
            .OrderBy(x => x.No);

        var result = await repo.ProjectToListAsync<CountryDto>(spec, filter);

        result.Should().BeOfType<PagedResponse<CountryDto>>();
        result.Pagination.Page.Should().Be(filter.Page);
        result.Pagination.PageSize.Should().Be(filter.PageSize);
        result.Data.Should().HaveCount(1);
        result.Data.First().No.Should().Be(4);
    }

    [Fact]
    public async Task ProjectToListAsync_IgnoresSpecPagination_GivenSpecAndPagingFilter()
    {
        var expected = new List<CountryDto>
        {
            new(1, "b"),
            new(2, "b"),
            new(3, "b"),
            new(4, "b"),
        };
        await SeedRangeAsync<Country>(
        [
            new() { No = 9, Name = "a" },
            new() { No = 9, Name = "c" },
            new() { No = 1, Name = "b" },
            new() { No = 2, Name = "b" },
            new() { No = 3, Name = "b" },
            new() { No = 4, Name = "b" },
            new() { No = 9, Name = "d" },
        ]);

        var filter = new PagingFilter() { Page = 2, PageSize = 3 };
        var repo = new Repository<Country>(DbContext);
        var spec = new Specification<Country>();
        spec.Query
            .Where(x => x.Name == "b")
            .OrderBy(x => x.No)
            .Skip(1)
            .Take(2);

        var result = await repo.ProjectToListAsync<CountryDto>(spec, filter);

        result.Should().BeOfType<PagedResponse<CountryDto>>();
        result.Pagination.Page.Should().Be(filter.Page);
        result.Pagination.PageSize.Should().Be(filter.PageSize);
        result.Data.Should().HaveCount(1);
        result.Data.First().No.Should().Be(4);

        // Ensure that the spec's pagination is not altered.
        spec.Skip.Should().Be(1);
        spec.Take.Should().Be(2);
    }
}
