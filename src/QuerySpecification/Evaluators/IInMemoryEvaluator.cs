namespace Pozitron.QuerySpecification;

public interface IInMemoryEvaluator
{
    IEnumerable<T> Evaluate<T>(IEnumerable<T> query, Specification<T> specification);
}
