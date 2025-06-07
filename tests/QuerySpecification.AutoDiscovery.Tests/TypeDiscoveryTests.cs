[assembly: SpecAutoDiscovery]

namespace Tests;

public class TypeDiscoveryTests
{
    [Fact]
    public void GetMemoryEvaluators_IncludesCustom()
    {
        var allEvaluators = TypeDiscovery.GetMemoryEvaluators();
        allEvaluators.Should().ContainSingle(x => x is TestMemoryEvaluator);
    }

    [Fact]
    public void GetEvaluators_IncludesCustom()
    {
        var allEvaluators = TypeDiscovery.GetEvaluators();
        allEvaluators.Should().ContainSingle(x => x is TestEvaluator);
    }

    [Fact]
    public void GetValidators_IncludesCustom()
    {
        var allValidators = TypeDiscovery.GetValidators();
        allValidators.Should().ContainSingle(x => x is TestValidator);
    }

    // Custom user evaluators and validators
    public class TestEvaluator : IEvaluator
    {
        public IQueryable<T> Evaluate<T>(IQueryable<T> source, Specification<T> specification) where T : class
        {
            return source;
        }
    }

    public class TestMemoryEvaluator : IMemoryEvaluator
    {
        public IEnumerable<T> Evaluate<T>(IEnumerable<T> source, Specification<T> specification)
        {
            return source;
        }
    }

    public class TestValidator : IValidator
    {
        public bool IsValid<T>(T entity, Specification<T> specification)
        {
            return true;
        }
    }
}

