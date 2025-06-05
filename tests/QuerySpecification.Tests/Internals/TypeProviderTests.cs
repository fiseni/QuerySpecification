namespace Tests.Internals;

public class TypeProviderTests
{
    [Fact]
    public void EvaluatorProvider_GetAllMemoryEvaluators_IncludesCustom()
    {
        var allEvaluators = EvaluatorProvider.GetAllMemoryEvaluators();
        allEvaluators.Should().ContainSingle(x => x is TestMemoryEvaluator);
    }

    [Fact]
    public void EvaluatorProvider_GetBuiltInMemoryEvaluators_ExcludesCustom()
    {
        var builtInEvaluators = EvaluatorProvider.GetBuiltInMemoryEvaluators();
        builtInEvaluators.Should().NotContain(x => x is TestMemoryEvaluator);
    }

    [Fact]
    public void EvaluatorProvider_GetAllEvaluators_IncludesCustom()
    {
        var allEvaluators = EvaluatorProvider.GetAllEvaluators();
        allEvaluators.Should().ContainSingle(x => x is TestEvaluator);
    }

    [Fact]
    public void EvaluatorProvider_GetBuiltInEvaluators_ExcludesCustom()
    {
        var builtInEvaluators = EvaluatorProvider.GetBuiltInEvaluators();
        builtInEvaluators.Should().NotContain(x => x is TestEvaluator);
    }

    [Fact]
    public void ValidatorProvider_GetAllValidators_IncludesCustom()
    {
        var allValidators = ValidatorProvider.GetAllValidators();
        allValidators.Should().ContainSingle(x => x is TestValidator);
    }

    [Fact]
    public void ValidatorProvider_GetBuiltInValidators_ExcludesCustom()
    {
        var builtInValidators = ValidatorProvider.GetBuiltInValidators();
        builtInValidators.Should().NotContain(x => x is TestValidator);
    }

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
