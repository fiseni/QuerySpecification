namespace Tests.Validators;

public class LikeValidatorTests
{
    private static readonly LikeValidator _validator = LikeValidator.Instance;
    private static readonly Customer _customer = new(1, "FirstName1", "LastName1");

    public record Customer(int Id, string FirstName, string? LastName);

    [Fact]
    public void ReturnsTrue_GivenSpecWithSingleLike_WithValidEntity()
    {
        var term = "irst";
        var spec = new Specification<Customer>();
        spec.Query
            .Like(x => x.FirstName, $"%{term}%");

        var result = _validator.IsValid(_customer, spec);
       
        result.Should().BeTrue();
    }

    [Fact]
    public void ReturnsFalse_GivenSpecWithSingleLike_WithInvalidEntity()
    {
        var term = "irstt";
        var spec = new Specification<Customer>();
        spec.Query
            .Like(x => x.FirstName, $"%{term}%");

        var result = _validator.IsValid(_customer, spec);
        
        result.Should().BeFalse();
    }

    [Fact]
    public void ReturnsTrue_GivenSpecWithMultipleLikeSameGroup_WithValidEntity()
    {
        var term = "irst";
        var spec = new Specification<Customer>();
        spec.Query
            .Like(x => x.FirstName, $"%{term}%")
            .Like(x => x.LastName, $"%{term}%");

        var result = _validator.IsValid(_customer, spec);
        
        result.Should().BeTrue();
    }

    [Fact]
    public void ReturnsFalse_GivenSpecWithMultipleLikeSameGroup_WithInvalidEntity()
    {
        var term = "irstt";
        var spec = new Specification<Customer>();
        spec.Query
            .Like(x => x.FirstName, $"%{term}%")
            .Like(x => x.LastName, $"%{term}%");

        var result = _validator.IsValid(_customer, spec);
        
        result.Should().BeFalse();
    }

    [Fact]
    public void ReturnsTrue_GivenSpecWithMultipleLikeDifferentGroup_WithValidEntity()
    {
        var term = "Name";
        var spec = new Specification<Customer>();
        spec.Query
            .Like(x => x.FirstName, $"%{term}%", 1)
            .Like(x => x.LastName, $"%{term}%", 2);

        var result = _validator.IsValid(_customer, spec);
        
        result.Should().BeTrue();
    }

    [Fact]
    public void ReturnsFalse_GivenSpecWithMultipleLikeDifferentGroup_WithInvalidEntity()
    {
        var term = "irst";
        var spec = new Specification<Customer>();
        spec.Query
            .Like(x => x.FirstName, $"%{term}%", 1)
            .Like(x => x.LastName, $"%{term}%", 2);

        var result = _validator.IsValid(_customer, spec);
        
        result.Should().BeFalse();
    }

    [Fact]
    public void ReturnsTrue_GivenSpecWithMultipleLikeSameGroup_WithNullProperty()
    {
        var term = "irst";
        var customer = new Customer(1, "FirstName1", null);
        var spec = new Specification<Customer>();
        spec.Query
            .Like(x => x.FirstName, $"%{term}%", 1)
            .Like(x => x.LastName, $"%{term}%", 1);

        var result = _validator.IsValid(_customer, spec);

        result.Should().BeTrue();
    }

    [Fact]
    public void ReturnsFalse_GivenSpecWithMultipleLikeDifferentGroup_WithNullProperty()
    {
        var term = "irst";
        var customer = new Customer(1, "FirstName1", null);
        var spec = new Specification<Customer>();
        spec.Query
            .Like(x => x.FirstName, $"%{term}%", 1)
            .Like(x => x.LastName, $"%{term}%", 2);

        var result = _validator.IsValid(_customer, spec);

        result.Should().BeFalse();
    }
}
