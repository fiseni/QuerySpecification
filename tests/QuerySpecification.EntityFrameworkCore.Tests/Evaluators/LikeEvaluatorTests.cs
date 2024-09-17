namespace Tests.Evaluators;

[Collection("SharedCollection")]
public class LikeEvaluatorTests(TestFactory factory) : IntegrationTest(factory)
{
    private static readonly LikeEvaluator _evaluator = LikeEvaluator.Instance;

    [Fact]
    public void QueriesMatch_GivenLikeExpressions()
    {
        var storeTerm = "ab";
        var companyTerm = "ab";
        var streetTerm = "ab";

        var spec = new Specification<Store>();
        spec.Query
            .Like(x => x.Name, $"%{storeTerm}%")
            .Like(x => x.Company.Name, $"%{companyTerm}%")
            .Like(x => x.Address.Street, $"%{streetTerm}%", 2);

        var actual = _evaluator.GetQuery(DbContext.Stores, spec)
            .ToQueryString()
            .Replace("__likeExpression_Pattern_", "__Format_"); //expr parameter names are different

        var expected = DbContext.Stores
            .Where(x => EF.Functions.Like(x.Name, $"%{storeTerm}%")
                    || EF.Functions.Like(x.Company.Name, $"%{companyTerm}%"))
            .Where(x => EF.Functions.Like(x.Address.Street, $"%{streetTerm}%"))
            .ToQueryString();

        actual.Should().Be(expected);
    }
}
