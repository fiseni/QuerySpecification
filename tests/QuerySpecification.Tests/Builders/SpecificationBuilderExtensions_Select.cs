namespace Tests.Builders;

public class SpecificationBuilderExtensions_Select
{
    public record Customer(int Id, string FirstName, string LastName);

    [Fact]
    public void DoesNothing_GivenNoSelect()
    {
        var spec = new Specification<Customer, string>();

        spec.SelectExpression.Should().BeNull();
    }

    [Fact]
    public void AddsSelector_GivenSelect()
    {
        Expression<Func<Customer, string>> expr = x => x.FirstName;

        var spec = new Specification<Customer, string>();
        spec.Query
            .Select(expr);

        spec.SelectExpression.Should().NotBeNull();
        spec.SelectExpression!.Selector.Should().NotBeNull();
        spec.SelectExpression!.Selector.Should().BeSameAs(expr);
    }

    [Fact]
    public void OverwritesSelector_GivenMultipleSelect()
    {
        Expression<Func<Customer, string>> expr = x => x.FirstName;

        var spec = new Specification<Customer, string>();
        spec.Query
            .Select(x => x.LastName);
        spec.Query
            .Select(expr);

        spec.SelectExpression.Should().NotBeNull();
        spec.SelectExpression!.Selector.Should().NotBeNull();
        spec.SelectExpression!.Selector.Should().BeSameAs(expr);
    }
}
