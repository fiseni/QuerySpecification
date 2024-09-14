namespace Pozitron.QuerySpecification.Tests;

public class SpecificationBuilderExtensions_Like
{
    public record Customer(int Id, string FirstName, string LastName);

    [Fact]
    public void DoesNothing_GivenNoLike()
    {
        var spec1 = new Specification<Customer>();
        var spec2 = new Specification<Customer, string>();

        spec1.LikeExpressions.Should().BeEmpty();
        spec2.LikeExpressions.Should().BeEmpty();
    }

    [Fact]
    public void DoesNothing_GivenLikeWithFalseCondition()
    {
        var spec1 = new Specification<Customer>();
        spec1.Query
            .Like(x => x.FirstName, "%a%", false);

        var spec2 = new Specification<Customer, string>();
        spec2.Query
            .Like(x => x.FirstName, "%a%", false);

        spec1.LikeExpressions.Should().BeEmpty();
        spec2.LikeExpressions.Should().BeEmpty();
    }

    [Fact]
    public void AddsLike_GivenSingleLike()
    {
        Expression<Func<Customer, string>> expr = x => x.FirstName;
        var pattern = "%a%";

        var spec1 = new Specification<Customer>();
        spec1.Query
            .Like(expr, pattern);

        var spec2 = new Specification<Customer, string>();
        spec2.Query
            .Like(expr, pattern);

        spec1.LikeExpressions.Should().ContainSingle();
        spec1.LikeExpressions.First().KeySelector.Should().BeSameAs(expr);
        spec1.LikeExpressions.First().Pattern.Should().Be(pattern);
        spec1.LikeExpressions.First().Group.Should().Be(1);
        spec2.LikeExpressions.Should().ContainSingle();
        spec2.LikeExpressions.First().KeySelector.Should().BeSameAs(expr);
        spec2.LikeExpressions.First().Pattern.Should().Be(pattern);
        spec2.LikeExpressions.First().Group.Should().Be(1);
    }

    [Fact]
    public void AddsLike_GivenMultipleLikeInSameGroup()
    {
        var spec1 = new Specification<Customer>();
        spec1.Query
            .Like(x => x.FirstName, "%a%")
            .Like(x => x.LastName, "%a%");

        var spec2 = new Specification<Customer, string>();
        spec2.Query
            .Like(x => x.FirstName, "%a%")
            .Like(x => x.LastName, "%a%");

        spec1.LikeExpressions.Should().HaveCount(2);
        spec1.LikeExpressions.Should().AllSatisfy(x => x.Group.Should().Be(1));
        spec2.LikeExpressions.Should().HaveCount(2);
        spec2.LikeExpressions.Should().AllSatisfy(x => x.Group.Should().Be(1));
    }

    [Fact]
    public void AddsLike_GivenMultipleLikeInDifferentGroups()
    {
        var spec1 = new Specification<Customer>();
        spec1.Query
            .Like(x => x.FirstName, "%a%", 1)
            .Like(x => x.LastName, "%a%", 2);

        var spec2 = new Specification<Customer, string>();
        spec2.Query
            .Like(x => x.FirstName, "%a%", 1)
            .Like(x => x.LastName, "%a%", 2);

        spec1.LikeExpressions.Should().HaveCount(2);
        spec1.LikeExpressions.Should().OnlyHaveUniqueItems(x => x.Group);
        spec2.LikeExpressions.Should().HaveCount(2);
        spec2.LikeExpressions.Should().OnlyHaveUniqueItems(x => x.Group);
    }
}
