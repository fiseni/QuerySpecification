namespace Tests.Extensions;

[Collection("SharedCollection")]
public class LikeExtensionTests(TestFactory factory) : IntegrationTest(factory)
{
    [Fact]
    public void QueriesMatch_GivenLikeExpressions()
    {
        var storeTerm = "ab";
        var companyTerm = "ab";

        var spec = new Specification<Store>();
        spec.Query
            .Like(x11 => x11.Name, $"%{storeTerm}%")
            .Like(x22 => x22.Company.Name, $"%{companyTerm}%");

        var actual = DbContext.Stores
            .AsQueryable()
            .Like(spec.LikeExpressions)
            .ToQueryString()
            .Replace("__likeExpression_Pattern_", "__Format_"); //expr parameter names are different

        var expected = DbContext.Stores
            .Where(x => EF.Functions.Like(x.Name, $"%{storeTerm}%")
                    || EF.Functions.Like(x.Company.Name, $"%{companyTerm}%"))
            .ToQueryString();

        actual.Should().Be(expected);
    }

    [Fact]
    public void QueriesMatch_GivenEmptySpec()
    {
        var spec = new Specification<Store>();

        var actual = DbContext.Stores
            .AsQueryable()
            .Like(spec.LikeExpressions)
            .ToQueryString()
            .Replace("__likeExpression_Pattern_", "__Format_"); //expr parameter names are different

        var expected = DbContext.Stores
            .ToQueryString();

        actual.Should().Be(expected);
    }
}
