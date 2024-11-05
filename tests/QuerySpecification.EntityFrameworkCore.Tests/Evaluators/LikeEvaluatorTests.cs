namespace Tests.Evaluators;

[Collection("SharedCollection")]
public class LikeEvaluatorTests(TestFactory factory) : IntegrationTest(factory)
{
    private static readonly LikeEvaluator _evaluator = LikeEvaluator.Instance;

    [Fact]
    public void QueriesMatch_GivenNoLike()
    {
        var spec = new Specification<Store>();
        spec.Query
            .Where(x => x.Id > 0);

        var actual = _evaluator.Evaluate(DbContext.Stores, spec)
            .ToQueryString();

        var expected = DbContext.Stores
            .ToQueryString();

        actual.Should().Be(expected);
    }

    [Fact]
    public void QueriesMatch_GivenSingleLike()
    {
        var storeTerm = "ab1";

        var spec = new Specification<Store>();
        spec.Query
            .Where(x => x.Id > 0)
            .Like(x => x.Name, $"%{storeTerm}%");

        var actual = _evaluator.Evaluate(DbContext.Stores, spec)
            .ToQueryString();

        var expected = DbContext.Stores
            .Where(x => EF.Functions.Like(x.Name, $"%{storeTerm}%"))
            .ToQueryString();

        actual.Should().Be(expected);
    }

    [Fact]
    public void QueriesMatch_GivenMultipleLike()
    {
        var storeTerm = "ab1";
        var companyTerm = "ab2";
        var countryTerm = "ab3";
        var streetTerm = "ab4";

        var spec = new Specification<Store>();
        spec.Query
            .Where(x => x.Id > 0)
            .Like(x => x.Name, $"%{storeTerm}%")
            .Like(x => x.Company.Name, $"%{companyTerm}%")
            .Like(x => x.Company.Country.Name, $"%{countryTerm}%", 3)
            .Like(x => x.Address.Street, $"%{streetTerm}%", 2);

        var actual = _evaluator.Evaluate(DbContext.Stores, spec)
            .ToQueryString();

        var expected = DbContext.Stores
            .Where(x => EF.Functions.Like(x.Name, $"%{storeTerm}%")
                    || EF.Functions.Like(x.Company.Name, $"%{companyTerm}%"))
            .Where(x => EF.Functions.Like(x.Address.Street, $"%{streetTerm}%"))
            .Where(x => EF.Functions.Like(x.Company.Country.Name, $"%{countryTerm}%"))
            .ToQueryString();

        actual.Should().Be(expected);
    }
}
