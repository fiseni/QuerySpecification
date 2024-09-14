using System.Runtime.CompilerServices;

namespace Pozitron.QuerySpecification.Tests.Validators;

public class SpecificationValidatorTests
{
    private static readonly SpecificationValidator _validatorDefault = SpecificationValidator.Default;
    private static readonly Customer _customer = new(1, "FirstName1", "LastName1");

    public record Customer(int Id, string FirstName, string LastName);

    [Fact]
    public void ReturnTrue_GivenAllValidatorsPass()
    {
        var id = 1;
        var likeTerm = "irst";
        var spec = new Specification<Customer>();
        spec.Query
            .Where(x => x.Id == id)
            .Like(x => x.FirstName, $"%{likeTerm}%");

        var result = _validatorDefault.IsValid(_customer, spec);
        var resultFromSpec = spec.IsSatisfiedBy(_customer);

        result.Should().Be(resultFromSpec);
        result.Should().BeTrue();
    }

    [Fact]
    public void ReturnFalse_GivenOneValidatorFails()
    {
        var id = 1;
        var likeTerm = "irst";
        var spec = new Specification<Customer>();
        spec.Query
            .Where(x => x.Id == id)
            .Like(x => x.FirstName, $"%{likeTerm}%");

        var result = _validatorDefault.IsValid(_customer, spec);
        var resultFromSpec = spec.IsSatisfiedBy(_customer);

        result.Should().Be(resultFromSpec);
        result.Should().BeTrue();
    }

    [Fact]
    public void ReturnFalse_GivenAllValidatorsFail()
    {
        var id = 2;
        var likeTerm = "irstt";
        var spec = new Specification<Customer>();
        spec.Query
            .Where(x => x.Id == id)
            .Like(x => x.FirstName, $"%{likeTerm}%");

        var result = _validatorDefault.IsValid(_customer, spec);
        var resultFromSpec = spec.IsSatisfiedBy(_customer);

        result.Should().Be(resultFromSpec);
        result.Should().BeFalse();
    }

    [Fact]
    public void ConstructorSetsProvidedValidators()
    {
        var validators = new List<IValidator>
        {
            WhereValidator.Instance,
            LikeValidator.Instance,
            WhereValidator.Instance,
        };

        var validator = new SpecificationValidator(validators);

        var state = ValidatorsOf(validator);
        state.Should().HaveSameCount(validators);
        state.Should().Equal(validators);
    }

    [Fact]
    public void DerivedSpecificationValidatorCanAlterDefaultValidators()
    {
        var validator = new SpecificationValidatorDerived();

        var state = ValidatorsOf(validator);
        state.Should().HaveCount(4);
        state[0].Should().BeOfType<LikeValidator>();
        state[1].Should().BeOfType<WhereValidator>();
        state[2].Should().BeOfType<LikeValidator>();
        state[3].Should().BeOfType<WhereValidator>();
    }

    private class SpecificationValidatorDerived : SpecificationValidator
    {
        public SpecificationValidatorDerived()
        {
            Validators.Add(WhereValidator.Instance);
            Validators.Insert(0, LikeValidator.Instance);
        }
    }

    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "<Validators>k__BackingField")]
    public static extern ref List<IValidator> ValidatorsOf(SpecificationValidator @this);
}
