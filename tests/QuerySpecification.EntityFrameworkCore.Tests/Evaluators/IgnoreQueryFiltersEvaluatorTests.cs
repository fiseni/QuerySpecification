namespace Tests.Evaluators;

[Collection("SharedCollection")]
public class IgnoreQueryFiltersEvaluatorTests(TestFactory factory) : IntegrationTest(factory)
{
    private static readonly IgnoreQueryFiltersEvaluator _evaluator = IgnoreQueryFiltersEvaluator.Instance;

    [Fact]
    public void QueriesMatch_GivenAIgnoreQueryFilters()
    {
        var spec = new Specification<Country>();
        spec.Query.IgnoreQueryFilters();

        var actual = _evaluator.GetQuery(DbContext.Countries, spec)
            .ToQueryString();

        var expected = DbContext.Countries
            .IgnoreQueryFilters()
            .ToQueryString();

        actual.Should().Be(expected.ToString());
    }

    [Fact]
    public void Applies_GivenAIgnoreQueryFilters()
    {
        var spec = new Specification<Country>();
        spec.Query.IgnoreQueryFilters();

        var actual = _evaluator.GetQuery(DbContext.Countries, spec)
            .Expression
            .ToString();

        var expected = DbContext.Countries
            .IgnoreQueryFilters()
            .Expression
            .ToString();

        actual.Should().Be(expected.ToString());
    }
}
