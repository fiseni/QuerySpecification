namespace Pozitron.QuerySpecification;

public interface IInMemoryEvaluator
{
    IEnumerable<T> Evaluate<T>(IEnumerable<T> source, Specification<T> specification);
}
