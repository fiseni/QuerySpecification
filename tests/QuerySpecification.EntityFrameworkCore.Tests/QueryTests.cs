namespace Tests;

[Collection("SharedCollection")]
public class QueryTests(TestFactory factory) : IntegrationTest(factory)
{
    public record CountryDto(string? Name);
    public record ProductImageDto(string? ImageUrl);

    [Fact]
    public async Task ComplexQuery()
    {
        var validCountryName = "CountryX";
        var validStoreName = "StoreX";
        var validProductName = "ProductX";
        var validProductImageUrl = "ImageUrlX";
        var validCity = "CityX";
        var validCompanyName = "CompanyX";
        var validStreet = "StreetX";

        var validCityTerm = "ityX";
        var validCompanyTerm = "ompanyX";
        var validStreetTerm = "treetX";

        var validCountry = new Country { Name = validCountryName, IsDeleted = true };
        var validCompany = new Company { Name = validCompanyName, Country = validCountry };
        var validAddress = new Address { Street = validStreet };
        var invalidCompany = new Company { Name = "Fails", Country = validCountry };
        var invalidAddress = new Address { Street = "Fails" };

        List<Product> NewProducts() =>
        [
            new() { Name = validProductName, Images = [new() { ImageUrl = validProductImageUrl }] },
            new() { Name = validProductName, Images = null },
            new() { Name = "Fails", Images = [new() { ImageUrl = "Fails" }] },
        ];

        // The second item is expected based on descending city order.
        var stores = new List<Store>
        {
            new() { Name = validStoreName, City = validCity, Company = invalidCompany, Address = validAddress with { }, Products = NewProducts() }, // this passes, city-company same LIKE group
            new() { Name = validStoreName, City = "WWW", Company = validCompany, Address = validAddress with { }, Products = NewProducts() }, // this passes, city-company same LIKE group
            new() { Name = validStoreName, City = "Fails", Company = invalidCompany, Address = validAddress with { }, Products = NewProducts() }, // fails, city and company
            new() { Name = validStoreName, City = validCity, Company = validCompany, Address = invalidAddress with { }, Products = NewProducts() }, // fails, address
            new() { Name = "Fails", City = validCity, Company = validCompany, Address = validAddress with { }, Products = NewProducts() }, // fails name
            new() { Name = validStoreName, City = validCity, Company = validCompany, Address = validAddress with { }, Products = NewProducts() }, // this passes
        };

        await SeedRangeAsync(stores);

        var spec = new Specification<Store>();
        spec.Query
            .Where(x => x.Name == validStoreName)
            .Like(x => x.City, $"%{validCityTerm}%")
            .Like(x => x.Company.Name, $"%{validCompanyTerm}%")
            .Like(x => x.Address.Street, $"%{validStreetTerm}%", 2)
            .Include(nameof(Address))
            .Include(x => x.Products.Where(x => x.Name == validProductName))
                .ThenInclude(x => x.Images)
            .Include(x => x.Company)
                .ThenInclude(x => x.Country)
            .OrderBy(x => x.Id)
                .ThenByDescending(x => x.City)
            .Skip(1)
            .Take(1)
            .IgnoreQueryFilters();

        var result = await DbContext.Stores
            .WithSpecification(spec)
            .ToListAsync();

        result.Should().ContainSingle();
        result[0].Name.Should().Be(validStoreName);
        result[0].City.Should().Be("WWW");
        result[0].Address.Street.Should().Be(validStreet);
        result[0].Company.Name.Should().Be(validCompanyName);
        result[0].Company.Country.Name.Should().Be(validCountryName);
        result[0].Products.Should().HaveCount(2);
        result[0].Products[0].Name.Should().Be(validProductName);
        result[0].Products[0].Images.Should().HaveCount(1);
        result[0].Products[0].Images![0].ImageUrl.Should().Be(validProductImageUrl);
        result[0].Products[1].Name.Should().Be(validProductName);
        result[0].Products[1].Images.Should().BeEmpty();
    }
}
