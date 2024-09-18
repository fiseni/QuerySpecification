namespace Pozitron.QuerySpecification;

public interface IEvaluator
{
    IQueryable<T> GetQuery<T>(IQueryable<T> query, Specification<T> specification) where T : class;
}
