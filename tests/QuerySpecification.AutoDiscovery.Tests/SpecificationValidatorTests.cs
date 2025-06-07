using System.Runtime.CompilerServices;

namespace Tests;

public class SpecificationValidatorTests
{
    [Fact]
    public void DefaultSingleton_ScansValidators_GivenAutoDiscoveryEnabled()
    {
        var validators = SpecificationValidator.Default;

        var result = ValidatorsOf(validators);

        result.Should().HaveCountGreaterThan(1);
        result.Should().ContainSingle(x => x is TestValidator);
    }

    [Fact]
    public void Constructor_ScansValidators_GivenAutoDiscoveryEnabled()
    {
        var validators = new SpecificationValidator();

        var result = ValidatorsOf(validators);

        result.Should().HaveCountGreaterThan(1);
        result.Should().ContainSingle(x => x is TestValidator);
    }

    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "<Validators>k__BackingField")]
    public static extern ref List<IValidator> ValidatorsOf(SpecificationValidator @this);

    public class TestValidator : IValidator
    {
        public bool IsValid<T>(T entity, Specification<T> specification)
        {
            return true;
        }
    }
}
