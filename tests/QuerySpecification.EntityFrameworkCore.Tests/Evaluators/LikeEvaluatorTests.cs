namespace Tests.Evaluators;

[Collection("SharedCollection")]
public class LikeEvaluatorTests(TestFactory factory) : IntegrationTest(factory)
{
    private static readonly LikeEvaluator _evaluator = LikeEvaluator.Instance;

    [Fact]
    public void QueriesMatch_GivenLikeExpressions()
    {
        var storeTerm = "ab1";
        var companyTerm = "ab2";
        var streetTerm = "ab3";

        var spec = new Specification<Store>();
        spec.Query
            .Like(x => x.Name, $"%{storeTerm}%")
            .Like(x => x.Company.Name, $"%{companyTerm}%")
            .Like(x => x.Address.Street, $"%{streetTerm}%", 2);

        var actual = _evaluator.Evaluate(DbContext.Stores, spec)
            .ToQueryString();

        var expected = DbContext.Stores
            .Where(x => EF.Functions.Like(x.Name, $"%{storeTerm}%")
                    || EF.Functions.Like(x.Company.Name, $"%{companyTerm}%"))
            .Where(x => EF.Functions.Like(x.Address.Street, $"%{streetTerm}%"))
            .ToQueryString();

        actual.Should().Be(expected);
    }
}
