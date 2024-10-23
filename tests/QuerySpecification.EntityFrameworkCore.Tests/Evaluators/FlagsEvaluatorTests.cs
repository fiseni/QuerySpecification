namespace Tests.Evaluators;

[Collection("SharedCollection")]
public class FlagsEvaluatorTests(TestFactory factory) : IntegrationTest(factory)
{
    private static readonly FlagsEvaluator _evaluator = FlagsEvaluator.Instance;

    [Fact]
    public void Applies_GivenAsNoTracking()
    {
        var spec = new Specification<Country>();
        spec.Query.AsNoTracking();

        var actual = _evaluator.Evaluate(DbContext.Countries, spec)
            .Expression
            .ToString();

        var expected = DbContext.Countries
            .AsNoTracking()
            .Expression
            .ToString();

        actual.Should().Be(expected);
    }

    [Fact]
    public void Applies_GivenAsNoTrackingWithIdentityResolution()
    {
        var spec = new Specification<Country>();
        spec.Query.AsNoTrackingWithIdentityResolution();

        var actual = _evaluator.Evaluate(DbContext.Countries, spec)
            .Expression
            .ToString();

        var expected = DbContext.Countries
            .AsNoTrackingWithIdentityResolution()
            .Expression
            .ToString();

        actual.Should().Be(expected);
    }

    [Fact]
    public void QueriesMatch_GivenIgnoreQueryFilters()
    {
        var spec = new Specification<Country>();
        spec.Query.IgnoreQueryFilters();

        var actual = _evaluator.Evaluate(DbContext.Countries, spec)
            .ToQueryString();

        var expected = DbContext.Countries
            .IgnoreQueryFilters()
            .ToQueryString();

        actual.Should().Be(expected);
    }

    [Fact]
    public void Applies_GivenIgnoreQueryFilters()
    {
        var spec = new Specification<Country>();
        spec.Query.IgnoreQueryFilters();

        var actual = _evaluator.Evaluate(DbContext.Countries, spec)
            .Expression
            .ToString();

        var expected = DbContext.Countries
            .IgnoreQueryFilters()
            .Expression
            .ToString();

        actual.Should().Be(expected);
    }

    [Fact]
    public void QueriesMatch_GivenAsSplitQuery()
    {
        var spec = new Specification<Country>();
        spec.Query.AsSplitQuery();

        var actual = _evaluator.Evaluate(DbContext.Countries, spec)
            .ToQueryString();

        var expected = DbContext.Countries
            .AsSplitQuery()
            .ToQueryString();

        actual.Should().Be(expected);
    }

    [Fact]
    public void Applies_GivenAsSplitQuery()
    {
        var spec = new Specification<Country>();
        spec.Query.AsSplitQuery();

        var actual = _evaluator.Evaluate(DbContext.Countries, spec)
            .Expression
            .ToString();

        var expected = DbContext.Countries
            .AsSplitQuery()
            .Expression
            .ToString();

        actual.Should().Be(expected);
    }
}
