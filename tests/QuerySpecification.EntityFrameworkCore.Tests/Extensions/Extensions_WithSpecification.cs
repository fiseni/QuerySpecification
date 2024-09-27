namespace Tests.Extensions;

[Collection("SharedCollection")]
public class Extensions_WithSpecification(TestFactory factory) : IntegrationTest(factory)
{
    public record CountryDto(string? Name);

    [Fact]
    public void QueriesMatch_GivenFullQuery()
    {
        var id = 1;
        var name = "Store1";
        var storeTerm = "ab";
        var companyTerm = "ab";
        var streetTerm = "ab";

        var spec = new Specification<Store>();
        spec.Query
            .Where(x => x.Id > id)
            .Where(x => x.Name == name)
            .Like(x => x.Name, $"%{storeTerm}%")
            .Like(x => x.Company.Name, $"%{companyTerm}%")
            .Like(x => x.Address.Street, $"%{streetTerm}%", 2)
            .Include(nameof(Address))
            .Include(x => x.Products.Where(x => x.Id > 10))
                .ThenInclude(x => x.Images)
            .Include(x => x.Company)
                .ThenInclude(x => x.Country)
            .OrderBy(x => x.Id)
                .ThenByDescending(x => x.Name)
            .Skip(1)
            .Take(10)
        .IgnoreQueryFilters();

        var actual = DbContext.Stores
            .WithSpecification(spec)
            .ToQueryString()
            .Replace("__likeExpression_Pattern_", "__Format_"); //like parameter names are different

        var expected = DbContext.Stores
            .Where(x => x.Id > id)
            .Where(x => x.Name == name)
            .Where(x => EF.Functions.Like(x.Name, $"%{storeTerm}%")
                    || EF.Functions.Like(x.Company.Name, $"%{companyTerm}%"))
            .Where(x => EF.Functions.Like(x.Address.Street, $"%{streetTerm}%"))
            .Include(nameof(Address))
            .Include(x => x.Products.Where(x => x.Id > 10))
                .ThenInclude(x => x.Images)
            .Include(x => x.Company)
                .ThenInclude(x => x.Country)
            .OrderBy(x => x.Id)
                .ThenByDescending(x => x.Name)
            .Skip(1)
            .Take(10)
            .IgnoreQueryFilters()
            .ToQueryString();

        actual.Should().Be(expected);
    }

    [Fact]
    public void QueriesMatch_GivenFullQueryWithSelect()
    {
        var id = 1;
        var name = "Store1";
        var storeTerm = "ab";
        var companyTerm = "ab";
        var streetTerm = "ab";

        var spec = new Specification<Store, string?>();
        spec.Query
            .Where(x => x.Id > id)
            .Where(x => x.Name == name)
            .Like(x => x.Name, $"%{storeTerm}%")
            .Like(x => x.Company.Name, $"%{companyTerm}%")
            .Like(x => x.Address.Street, $"%{streetTerm}%", 2)
            .Include(nameof(Address))
            .Include(x => x.Products.Where(x => x.Id > 10))
                .ThenInclude(x => x.Images)
            .Include(x => x.Company)
                .ThenInclude(x => x.Country)
            .OrderBy(x => x.Id)
                .ThenByDescending(x => x.Name)
            .Skip(1)
            .Take(10)
            .IgnoreQueryFilters()
        .Select(x => x.Name);

        var actual = DbContext.Stores
            .WithSpecification(spec)
            .ToQueryString()
            .Replace("__likeExpression_Pattern_", "__Format_"); //like parameter names are different

        var expected = DbContext.Stores
            .Where(x => x.Id > id)
            .Where(x => x.Name == name)
            .Where(x => EF.Functions.Like(x.Name, $"%{storeTerm}%")
                    || EF.Functions.Like(x.Company.Name, $"%{companyTerm}%"))
            .Where(x => EF.Functions.Like(x.Address.Street, $"%{streetTerm}%"))
            .Include(nameof(Address))
            .Include(x => x.Products.Where(x => x.Id > 10))
                .ThenInclude(x => x.Images)
            .Include(x => x.Company)
                .ThenInclude(x => x.Country)
            .OrderBy(x => x.Id)
                .ThenByDescending(x => x.Name)
            .Skip(1)
            .Take(10)
            .IgnoreQueryFilters()
            .Select(x => x.Name)
            .ToQueryString();

        actual.Should().Be(expected);
    }

    [Fact]
    public void QueriesMatch_GivenFullQueryWithSelectMany()
    {
        var id = 1;
        var name = "Store1";
        var storeTerm = "ab";
        var companyTerm = "ab";
        var streetTerm = "ab";

        var spec = new Specification<Store, string?>();
        spec.Query
            .Where(x => x.Id > id)
            .Where(x => x.Name == name)
            .Like(x => x.Name, $"%{storeTerm}%")
            .Like(x => x.Company.Name, $"%{companyTerm}%")
            .Like(x => x.Address.Street, $"%{streetTerm}%", 2)
            .Include(nameof(Address))
            .Include(x => x.Products.Where(x => x.Id > 10))
                .ThenInclude(x => x.Images)
            .Include(x => x.Company)
                .ThenInclude(x => x.Country)
            .OrderBy(x => x.Id)
                .ThenByDescending(x => x.Name)
            .Skip(1)
            .Take(10)
            .IgnoreQueryFilters()
            .SelectMany(x => x.Products.Select(x => x.Name));

        var actual = DbContext.Stores
            .WithSpecification(spec)
            .ToQueryString()
            .Replace("__likeExpression_Pattern_", "__Format_"); //like parameter names are different

        var expected = DbContext.Stores
            .Where(x => x.Id > id)
            .Where(x => x.Name == name)
            .Where(x => EF.Functions.Like(x.Name, $"%{storeTerm}%")
                    || EF.Functions.Like(x.Company.Name, $"%{companyTerm}%"))
            .Where(x => EF.Functions.Like(x.Address.Street, $"%{streetTerm}%"))
            .Include(nameof(Address))
            .Include(x => x.Products.Where(x => x.Id > 10))
                .ThenInclude(x => x.Images)
            .Include(x => x.Company)
                .ThenInclude(x => x.Country)
            .OrderBy(x => x.Id)
                .ThenByDescending(x => x.Name)
            .Skip(1)
            .Take(10)
            .IgnoreQueryFilters()
            .SelectMany(x => x.Products.Select(x => x.Name))
            .ToQueryString();

        actual.Should().Be(expected);
    }

    [Fact]
    public void QueriesMatch_GivenCustomEvaluator()
    {
        var id = 1;

        var spec = new Specification<Store>();
        spec.Query
            .Where(x => x.Id > id)
            .Skip(1)
            .Take(10); // should be ignored by the custom evaluator

        var actual = DbContext.Stores
            .WithSpecification(spec, new MySpecificationEvaluator())
            .ToQueryString();

        var expected = DbContext.Stores
            .Where(x => x.Id > id)
            .ToQueryString();

        actual.Should().Be(expected);
    }

    [Fact]
    public void QueriesMatch_GivenSelectorAndCustomEvaluator()
    {
        var id = 1;

        var spec = new Specification<Store, CountryDto>();
        spec.Query
            .Where(x => x.Id > id)
            .Skip(1)
            .Take(10) // should be ignored by the custom evaluator
            .Select(x => new CountryDto(x.Name));

        var actual = DbContext.Stores
            .WithSpecification(spec, new MySpecificationEvaluator())
            .ToQueryString();

        var expected = DbContext.Stores
            .Where(x => x.Id > id)
            .Select(x => new CountryDto(x.Name))
            .ToQueryString();

        actual.Should().Be(expected);
    }

    public class MySpecificationEvaluator : SpecificationEvaluator
    {
        public MySpecificationEvaluator()
        {
            Evaluators.Remove(PaginationEvaluator.Instance);
        }
    }
}
