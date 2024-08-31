namespace Pozitron.QuerySpecification;

public interface ISpecificationEvaluator
{
    IQueryable<TResult> GetQuery<T, TResult>(IQueryable<T> inputQuery, Specification<T, TResult> specification) where T : class;
    IQueryable<T> GetQuery<T>(IQueryable<T> inputQuery, Specification<T> specification, bool evaluateCriteriaOnly = false) where T : class;
}
