namespace Pozitron.QuerySpecification;

public interface IInMemoryEvaluator
{
    bool IsCriteriaEvaluator { get; }
    IEnumerable<T> Evaluate<T>(IEnumerable<T> query, Specification<T> specification);
}
