namespace Tests.Validators;

public class WhereValidatorTests
{
    private static readonly WhereValidator _validator = WhereValidator.Instance;
    private static readonly Customer _customer = new(1, "Customer1");

    public record Customer(int Id, string Name);

    [Fact]
    public void ReturnsTrue_GivenSpecWithSingleWhere_WithValidEntity()
    {
        var id = 1;
        var spec = new Specification<Customer>();
        spec.Query
            .Where(x => x.Id == id);

        var result = _validator.IsValid(_customer, spec);

        result.Should().BeTrue();
    }

    [Fact]
    public void ReturnsFalse_GivenSpecWithSingleWhere_WithInvalidEntity()
    {
        var id = 2;
        var spec = new Specification<Customer>();
        spec.Query
            .Where(x => x.Id == id);

        var result = _validator.IsValid(_customer, spec);
        
        result.Should().BeFalse();
    }

    [Fact]
    public void ReturnsTrue_GivenSpecWithMultipleWhere_WithValidEntity()
    {
        var id = 1;
        var spec = new Specification<Customer>();
        spec.Query
            .Where(x => x.Id == id).Where(x => x.Name == "Customer1");

        var result = _validator.IsValid(_customer, spec);
        
        result.Should().BeTrue();
    }

    [Fact]
    public void ReturnsFalse_GivenSpecWithMultipleWhere_WithSingleInvalidValue()
    {
        var id = 2;
        var spec = new Specification<Customer>();
        spec.Query
            .Where(x => x.Id == id).Where(x => x.Name == "Customer1");

        var result = _validator.IsValid(_customer, spec);
        
        result.Should().BeFalse();
    }

    [Fact]
    public void ReturnsFalse_GivenSpecWithMultipleWhere_WithAllInvalidValues()
    {
        var id = 2;
        var spec = new Specification<Customer>();
        spec.Query
            .Where(x => x.Id == id).Where(x => x.Name == "Customer2");

        var result = _validator.IsValid(_customer, spec);
        
        result.Should().BeFalse();
    }
}
