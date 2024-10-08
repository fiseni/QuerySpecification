﻿namespace Tests.Builders;

public class SpecificationBuilderExtensions_AsNoTracking
{
    public record Customer(int Id, string Name);

    [Fact]
    public void DoesNothing_GivenNoAsNoTracking()
    {
        var spec1 = new Specification<Customer>();
        var spec2 = new Specification<Customer, string>();

        spec1.AsNoTracking.Should().Be(false);
        spec2.AsNoTracking.Should().Be(false);
    }

    [Fact]
    public void DoesNothing_GivenAsNoTrackingWithFalseCondition()
    {
        var spec1 = new Specification<Customer>();
        spec1.Query
            .AsNoTracking(false);

        var spec2 = new Specification<Customer, string>();
        spec2.Query
            .AsNoTracking(false);

        spec1.AsNoTracking.Should().Be(false);
        spec2.AsNoTracking.Should().Be(false);
    }

    [Fact]
    public void SetsAsNoTracking_GivenAsNoTracking()
    {
        var spec1 = new Specification<Customer>();
        spec1.Query
            .AsNoTracking();

        var spec2 = new Specification<Customer, string>();
        spec2.Query
            .AsNoTracking();

        spec1.AsNoTracking.Should().Be(true);
        spec2.AsNoTracking.Should().Be(true);
    }

    [Fact]
    public void SetsAsNoTracking_GivenAsNoTrackingAndAsNoTrackingWithIdentityResolution()
    {
        var spec1 = new Specification<Customer>();
        spec1.Query
            .AsNoTrackingWithIdentityResolution()
            .AsNoTracking();

        var spec2 = new Specification<Customer>();
        spec2.Query
            .AsNoTrackingWithIdentityResolution()
            .AsNoTracking();

        spec1.AsNoTrackingWithIdentityResolution.Should().Be(false);
        spec1.AsNoTracking.Should().Be(true);
        spec2.AsNoTrackingWithIdentityResolution.Should().Be(false);
        spec2.AsNoTracking.Should().Be(true);
    }
}
