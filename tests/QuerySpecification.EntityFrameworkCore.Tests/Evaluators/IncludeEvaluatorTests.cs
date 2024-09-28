namespace Tests.Evaluators;

[Collection("SharedCollection")]
public class IncludeEvaluatorTests(TestFactory factory) : IntegrationTest(factory)
{
    private static readonly IncludeEvaluator _evaluator = IncludeEvaluator.Instance;

    [Fact]
    public void QueriesMatch_GivenIncludeExpressions()
    {
        var spec = new Specification<Store>();
        spec.Query
            .Include(nameof(Address))
            .Include(x => x.Products.Where(x => x.Id > 10))
                .ThenInclude(x => x.Images)
            .Include(x => x.Company)
                .ThenInclude(x => x.Country);

        var actual = _evaluator.Evaluate(DbContext.Stores, spec)
            .ToQueryString();

        var expected = DbContext.Stores
            .Include(nameof(Address))
            .Include(x => x.Products.Where(x => x.Id > 10))
                .ThenInclude(x => x.Images)
            .Include(x => x.Company)
                .ThenInclude(x => x.Country)
            .ToQueryString();

        actual.Should().Be(expected);
    }
}
