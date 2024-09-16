namespace Tests.Evaluators;

[Collection("SharedCollection")]
public class AsNoTrackingWithIdentityResolutionEvaluatorTests(TestFactory factory) : IntegrationTest(factory)
{
    private static readonly AsNoTrackingWithIdentityResolutionEvaluator _evaluator = AsNoTrackingWithIdentityResolutionEvaluator.Instance;

    [Fact]
    public void Apply_GivenAsNoTrackingWithIdentityResolution()
    {
        var spec = new Specification<Country>();
        spec.Query.AsNoTrackingWithIdentityResolution();

        var actual = _evaluator.GetQuery(DbContext.Countries, spec)
            .Expression
            .ToString();

        var expected = DbContext.Countries
            .AsNoTrackingWithIdentityResolution()
            .Expression
            .ToString();

        actual.Should().Be(expected.ToString());
    }

    [Fact]
    public void NotApply_GivenEmptySpec()
    {
        var spec = new Specification<Country>();

        var actual = _evaluator.GetQuery(DbContext.Countries, spec)
            .Expression
            .ToString();

        var expected = DbContext.Countries
            .AsNoTrackingWithIdentityResolution()
            .Expression
            .ToString();

        actual.Should().NotBe(expected.ToString());
    }
}
