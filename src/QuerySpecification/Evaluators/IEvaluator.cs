namespace Pozitron.QuerySpecification;

public interface IEvaluator
{
    IQueryable<T> Evaluate<T>(IQueryable<T> source, Specification<T> specification) where T : class;
}
