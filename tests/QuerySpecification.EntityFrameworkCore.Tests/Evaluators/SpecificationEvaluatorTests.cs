using System.Runtime.CompilerServices;

namespace Tests.Evaluators;

[Collection("SharedCollection")]
public class SpecificationEvaluatorTests(TestFactory factory) : IntegrationTest(factory)
{
    private static readonly SpecificationEvaluator _evaluator = SpecificationEvaluator.Default;

    public record Customer(int Id, string FirstName, string LastName, List<string>? Emails = null);

    [Fact]
    public void Evaluate_ThrowsArgumentNullException_GivenNullSpec()
    {
        Action sutAction = () => _evaluator.GetQuery(DbContext.Countries, (Specification<Country>)null!);

        sutAction.Should().Throw<ArgumentNullException>().WithParameterName("specification");
    }

    [Fact]
    public void Evaluate_ThrowsArgumentNullException_GivenNullSpecificationWithSelector()
    {
        Action sutAction = () => _evaluator.GetQuery(DbContext.Countries, (Specification<Country, string>)null!);

        sutAction.Should().Throw<ArgumentNullException>().WithParameterName("specification");
    }

    [Fact]
    public void Evaluate_ThrowsSelectorNotFoundException_GivenNoSelector()
    {
        var spec = new Specification<Country, string>();

        Action sutAction = () => _evaluator.GetQuery(DbContext.Countries, spec);

        sutAction.Should().Throw<SelectorNotFoundException>();
    }

    [Fact]
    public void Evaluate_ThrowsConcurrentSelectorsException_GivenBothSelectAndSelectMany()
    {
        var spec = new Specification<Store, string?>();
        spec.Query
            .Select(x => x.Name);
        spec.Query
            .SelectMany(x => x.Products.Select(x => x.Name));

        Action sutAction = () => _evaluator.GetQuery(DbContext.Stores, spec);

        sutAction.Should().Throw<ConcurrentSelectorsException>();
    }

    [Fact]
    public void QueriesMatch_GivenFullQuery()
    {
        var id = 2;
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

        var actual = _evaluator.GetQuery(DbContext.Stores, spec)
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
        var id = 2;
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

        var actual = _evaluator.GetQuery(DbContext.Stores, spec)
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
        var id = 2;
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

        var actual = _evaluator.GetQuery(DbContext.Stores, spec)
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
    public void ConstructorSetsProvidedEvaluators()
    {
        var evaluators = new List<IEvaluator>
        {
            WhereEvaluator.Instance,
            OrderEvaluator.Instance,
            WhereEvaluator.Instance,
        };

        var evaluator = new SpecificationEvaluator(evaluators);

        var state = EvaluatorsOf(evaluator);
        state.Should().HaveSameCount(evaluators);
        state.Should().Equal(evaluators);
    }

    [Fact]
    public void DerivedSpecificationEvaluatorCanAlterDefaultEvaluator()
    {
        var evaluator = new SpecificationEvaluatorDerived();

        var state = EvaluatorsOf(evaluator);
        state.Should().HaveCount(11);
        state[0].Should().BeOfType<LikeEvaluator>();
        state[1].Should().BeOfType<WhereEvaluator>();
        state[2].Should().BeOfType<LikeEvaluator>();
        state[3].Should().BeOfType<IncludeEvaluator>();
        state[4].Should().BeOfType<OrderEvaluator>();
        state[5].Should().BeOfType<PaginationEvaluator>();
        state[6].Should().BeOfType<AsNoTrackingEvaluator>();
        state[7].Should().BeOfType<AsNoTrackingWithIdentityResolutionEvaluator>();
        state[8].Should().BeOfType<IgnoreQueryFiltersEvaluator>();
        state[9].Should().BeOfType<AsSplitQueryEvaluator>();
        state[10].Should().BeOfType<WhereEvaluator>();
    }

    private class SpecificationEvaluatorDerived : SpecificationEvaluator
    {
        public SpecificationEvaluatorDerived()
        {
            Evaluators.Add(WhereEvaluator.Instance);
            Evaluators.Insert(0, LikeEvaluator.Instance);
        }
    }

    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "<Evaluators>k__BackingField")]
    public static extern ref List<IEvaluator> EvaluatorsOf(SpecificationEvaluator @this);
}
