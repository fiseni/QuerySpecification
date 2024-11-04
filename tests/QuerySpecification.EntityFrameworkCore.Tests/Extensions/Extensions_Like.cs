namespace Tests.Extensions;

[Collection("SharedCollection")]
public class Extensions_Like(TestFactory factory) : IntegrationTest(factory)
{
    [Fact]
    public void QueriesMatch_GivenSpecWithMultipleLike()
    {
        var storeTerm = "ab1";
        var companyTerm = "ab2";

        var spec = new Specification<Store>();
        spec.Query
            .Like(x11 => x11.Name, $"%{storeTerm}%")
            .Like(x22 => x22.Company.Name, $"%{companyTerm}%");

        var actual = DbContext.Stores
            .ApplyLikesAsOrGroup(spec.Items)
            .ToQueryString();

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
            .ApplyLikesAsOrGroup(spec.Items)
            .ToQueryString();

        var expected = DbContext.Stores
            .ToQueryString();

        actual.Should().Be(expected);
    }
}
