namespace Tests.Builders;

public class SpecificationBuilderExtensions_Take
{
    public record Customer(int Id, string Name);

    [Fact]
    public void DoesNothing_GivenNoTake()
    {
        var spec1 = new Specification<Customer>();
        var spec2 = new Specification<Customer, string>();

        spec1.Take.Should().BeLessThan(0);
        spec2.Take.Should().BeLessThan(0);
    }

    [Fact]
    public void DoesNothing_GivenTakeWithFalseCondition()
    {
        var take = 1;

        var spec1 = new Specification<Customer>();
        spec1.Query
            .Take(take, false);

        var spec2 = new Specification<Customer, string>();
        spec2.Query
            .Take(take, false);

        spec1.Take.Should().BeLessThan(0);
        spec2.Take.Should().BeLessThan(0);
    }

    [Fact]
    public void SetsTake_GivenTake()
    {
        var take = 1;

        var spec1 = new Specification<Customer>();
        spec1.Query
            .Take(take);

        var spec2 = new Specification<Customer, string>();
        spec2.Query
            .Take(take);

        spec1.Take.Should().Be(take);
        spec2.Take.Should().Be(take);
    }

    [Fact]
    public void OverwritesTake_GivenNewTake()
    {
        var take = 1;
        var takeNew = 2;

        var spec1 = new Specification<Customer>();
        spec1.Query
            .Take(take)
            .Take(takeNew);

        var spec2 = new Specification<Customer, string>();
        spec2.Query
            .Take(take)
            .Take(takeNew);

        spec1.Take.Should().Be(takeNew);
        spec2.Take.Should().Be(takeNew);
    }
}
