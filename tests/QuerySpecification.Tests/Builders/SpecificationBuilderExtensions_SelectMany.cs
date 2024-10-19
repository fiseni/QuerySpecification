namespace Tests.Builders;

public class SpecificationBuilderExtensions_SelectMany
{
    public record Customer(int Id, List<string> FirstName, List<string> LastName);

    [Fact]
    public void DoesNothing_GivenNoSelectMany()
    {
        var spec = new Specification<Customer, string>();

        spec.SelectExpression.Should().BeNull();
    }

    [Fact]
    public void AddsSelectorMany_GivenSelectMany()
    {
        Expression<Func<Customer, IEnumerable<string>>> expr = x => x.FirstName;

        var spec = new Specification<Customer, string>();
        spec.Query
            .SelectMany(expr);

        spec.SelectExpression.Should().NotBeNull();
        spec.SelectExpression!.SelectorMany.Should().NotBeNull();
        spec.SelectExpression!.SelectorMany.Should().BeSameAs(expr);
    }

    [Fact]
    public void OverwritesSelectorMany_GivenMultipleSelectMany()
    {
        Expression<Func<Customer, IEnumerable<string>>> expr = x => x.FirstName;

        var spec = new Specification<Customer, string>();
        spec.Query
            .SelectMany(x => x.LastName);
        spec.Query
            .SelectMany(expr);

        spec.SelectExpression.Should().NotBeNull();
        spec.SelectExpression!.SelectorMany.Should().NotBeNull();
        spec.SelectExpression!.SelectorMany.Should().BeSameAs(expr);
    }
}
