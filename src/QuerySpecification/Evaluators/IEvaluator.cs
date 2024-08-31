namespace Pozitron.QuerySpecification;

public interface IEvaluator
{
    bool IsCriteriaEvaluator { get; }

    IQueryable<T> GetQuery<T>(IQueryable<T> query, Specification<T> specification) where T : class;
}
