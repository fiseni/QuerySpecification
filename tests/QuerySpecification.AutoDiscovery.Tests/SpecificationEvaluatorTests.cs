using System.Runtime.CompilerServices;

namespace Tests;

public class SpecificationEvaluatorTests
{
    [Fact]
    public void DefaultSingleton_ScansEvaluators_GivenAutoDiscoveryEnabled()
    {
        var evaluator = SpecificationEvaluator.Default;

        var result = EvaluatorsOf(evaluator);

        result.Should().HaveCountGreaterThan(1);
        result.Should().ContainSingle(x => x is TestEvaluator);
    }

    [Fact]
    public void Constructor_ScansEvaluators_GivenAutoDiscoveryEnabled()
    {
        var evaluator = new SpecificationEvaluator();

        var result = EvaluatorsOf(evaluator);

        result.Should().HaveCountGreaterThan(1);
        result.Should().ContainSingle(x => x is TestEvaluator);
    }

    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "<Evaluators>k__BackingField")]
    public static extern ref List<IEvaluator> EvaluatorsOf(SpecificationEvaluator @this);

    public class TestEvaluator : IEvaluator
    {
        public IQueryable<T> Evaluate<T>(IQueryable<T> source, Specification<T> specification) where T : class
        {
            return source;
        }
    }
}
