namespace Pozitron.QuerySpecification.Tests;

public class SpecificationBuilderExtensions_Like
{
    [Fact]
    public void AddsNothingToList_GivenNoWhereExpression()
    {
        var spec = new StoreEmptySpec();

        spec.LikeExpressions.Should().BeEmpty();
    }

    [Fact]
    public void AddsNothingToList_GivenLikeExpressionWithFalseCondition()
    {
        var spec = new CompanyByIdWithFalseConditions(1);

        spec.LikeExpressions.Should().BeEmpty();
    }

    [Fact]
    public void AddsOneCriteriaWithDefaultGroupToList_GivenOneLikeExpressionWithNoGroup()
    {
        var spec = new StoreSearchByNameSpec("test");

        spec.LikeExpressions.Should().ContainSingle();
        spec.LikeExpressions.Single().Pattern.Should().Be("%test%");
        spec.LikeExpressions.Single().Group.Should().Be(1);
    }

    [Fact]
    public void AddsTwoCriteriaWithSameGroupToList_GivenTwoLikeExpressionWithNoGroup()
    {
        var spec = new StoreSearchByNameOrCitySpec("test");

        var criterias = spec.LikeExpressions.ToList();

        criterias.Should().HaveCount(2);
        criterias.ForEach(x => x.Pattern.Should().Be("%test%"));
        criterias.ForEach(x => x.Group.Should().Be(1));
    }

    [Fact]
    public void AddsTwoCriteriaWithDifferentGroupToList_GivenTwoLikeExpressionWithDistinctGroup()
    {
        var spec = new StoreSearchByNameAndCitySpec("test");

        var criterias = spec.LikeExpressions.ToList();

        criterias.Should().HaveCount(2);
        criterias.ForEach(x => x.Pattern.Should().Be("%test%"));
        criterias[0].Group.Should().Be(1);
        criterias[1].Group.Should().Be(2);
    }
}
