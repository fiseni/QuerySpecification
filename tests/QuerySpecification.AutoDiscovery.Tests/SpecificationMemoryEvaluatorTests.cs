using System.Runtime.CompilerServices;

namespace Tests;

public class SpecificationMemoryEvaluatorTests
{
    [Fact]
    public void DefaultSingleton_ScansEvaluators_GivenAutoDiscoveryEnabled()
    {
        var evaluator = SpecificationMemoryEvaluator.Default;

        var result = EvaluatorsOf(evaluator);

        result.Should().HaveCountGreaterThan(1);
        result.Should().ContainSingle(x => x is TestMemoryEvaluator);
    }

    [Fact]
    public void Constructor_ScansEvaluators_GivenAutoDiscoveryEnabled()
    {
        var evaluator = new SpecificationMemoryEvaluator();

        var result = EvaluatorsOf(evaluator);

        result.Should().HaveCountGreaterThan(1);
        result.Should().ContainSingle(x => x is TestMemoryEvaluator);
    }

    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "<Evaluators>k__BackingField")]
    public static extern ref List<IMemoryEvaluator> EvaluatorsOf(SpecificationMemoryEvaluator @this);

    public class TestMemoryEvaluator : IMemoryEvaluator
    {
        public IEnumerable<T> Evaluate<T>(IEnumerable<T> source, Specification<T> specification)
        {
            return source;
        }
    }
}
