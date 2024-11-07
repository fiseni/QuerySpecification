namespace Tests.Builders;

public class Builder_Skip
{
    public record Customer(int Id, string Name);

    [Fact]
    public void DoesNothing_GivenNoSkip()
    {
        var spec1 = new Specification<Customer>();
        var spec2 = new Specification<Customer, string>();

        spec1.Skip.Should().BeLessThan(0);
        spec2.Skip.Should().BeLessThan(0);
    }

    [Fact]
    public void DoesNothing_GivenSkipWithFalseCondition()
    {
        var skip = 1;

        var spec1 = new Specification<Customer>();
        spec1.Query
            .Skip(skip, false);

        var spec2 = new Specification<Customer, string>();
        spec2.Query
            .Skip(skip, false);

        spec1.Skip.Should().BeLessThan(0);
        spec2.Skip.Should().BeLessThan(0);
    }

    [Fact]
    public void SetsSkip_GivenSkip()
    {
        var skip = 1;

        var spec1 = new Specification<Customer>();
        spec1.Query
            .Skip(skip);

        var spec2 = new Specification<Customer, string>();
        spec2.Query
            .Skip(skip);

        spec1.Skip.Should().Be(skip);
        spec2.Skip.Should().Be(skip);
    }

    [Fact]
    public void OverwritesSkip_GivenNewSkip()
    {
        var skip = 1;
        var skipNew = 2;

        var spec1 = new Specification<Customer>();
        spec1.Query
            .Skip(skip)
            .Skip(skipNew);

        var spec2 = new Specification<Customer, string>();
        spec2.Query
            .Skip(skip)
            .Skip(skipNew);

        spec1.Skip.Should().Be(skipNew);
        spec2.Skip.Should().Be(skipNew);
    }
}
