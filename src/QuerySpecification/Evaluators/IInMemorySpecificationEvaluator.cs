namespace Pozitron.QuerySpecification;

public interface IInMemorySpecificationEvaluator
{
    IEnumerable<TResult> Evaluate<T, TResult>(IEnumerable<T> source, Specification<T, TResult> specification);
    IEnumerable<T> Evaluate<T>(IEnumerable<T> source, Specification<T> specification);
}
