namespace QuerySpecification.EntityFrameworkCore.Tests.Extensions;

[Collection("SharedCollection")]
public class DbSetExtensionsTests(TestFactory factory) : IntegrationTest(factory)
{
    public record CountryDto(string? Name);
    public record ProductImageDto(string? ImageUrl);

    [Fact]
    public async Task WithSpecification_AppliesSpec()
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

        var spec = new Specification<Country>();
        spec.Query
            .Where(x => x.Name == "b");

        var result = await DbContext.Countries
            .WithSpecification(spec)
            .ToListAsync();

        result.Should().HaveSameCount(expected);
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task WithSpecification_AppliesProjectionSpec()
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

        var spec = new Specification<Country, CountryDto>();
        spec.Query
            .Where(x => x.Name == "b")
            .Select(x => new CountryDto(x.Name));

        var result = await DbContext.Countries
            .WithSpecification(spec)
            .ToListAsync();

        result.Should().HaveSameCount(expected);
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task WithSpecification_AppliesProjectionSpecWithSelectMany()
    {
        var expected = new List<ProductImageDto>
        {
            new("b"),
            new("b"),
            new("b"),
        };
        var store = new Store
        {
            Name = "Store1",
            Company = new Company
            {
                Name = "Company1",
                Country = new Country { Name = "b" }
            },
        };
        await SeedRangeAsync(
            [
                new Product()
                {
                    Store = store,
                    Images =
                    [
                        new() { ImageUrl = "a" },
                        new() { ImageUrl = null },
                        new() { ImageUrl = "b" },
                        new() { ImageUrl = "b" },
                        new() { ImageUrl = "b" },
                        new() { ImageUrl = "d" },
                    ]
                },
                new Product()
                {
                    Store = store,
                    Images = null
                }
            ]);

        var spec = new Specification<Product, ProductImageDto>();
        spec.Query
            .SelectMany(x => x.Images!.Where(x => x.ImageUrl == "b").Select(x => new ProductImageDto(x.ImageUrl)));

        var result = await DbContext.Products
            .WithSpecification(spec)
            .ToListAsync();

        result.Should().HaveSameCount(expected);
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task WithSpecification_AppliesSpec_GivenCustomEvaluator()
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

        var spec = new Specification<Country>();
        spec.Query
            .Where(x => x.Name == "b")
            .Take(1); // should be ignores by the custom evaluator

        var result = await DbContext.Countries
            .WithSpecification(spec, new MySpecificationEvaluator())
            .ToListAsync();

        result.Should().HaveSameCount(expected);
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task WithSpecification_AppliesProjectionSpec_GivenCustomEvaluator()
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

        var spec = new Specification<Country, CountryDto>();
        spec.Query
            .Where(x => x.Name == "b")
            .Take(1) // should be ignores by the custom evaluator
            .Select(x => new CountryDto(x.Name));

        var result = await DbContext.Countries
            .WithSpecification(spec, new MySpecificationEvaluator())
            .ToListAsync();

        result.Should().HaveSameCount(expected);
        result.Should().BeEquivalentTo(expected);
    }

    public class MySpecificationEvaluator : SpecificationEvaluator
    {
        public MySpecificationEvaluator()
        {
            Evaluators.Remove(PaginationEvaluator.Instance);
        }
    }
}
