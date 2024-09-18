namespace Pozitron.QuerySpecification;

public class WhereEvaluator : IEvaluator, IInMemoryEvaluator
{
    private WhereEvaluator() { }
    public static WhereEvaluator Instance = new();

    public IQueryable<T> GetQuery<T>(IQueryable<T> query, Specification<T> specification) where T : class
    {
        foreach (var whereExpression in specification.WhereExpressions)
        {
            query = query.Where(whereExpression.Filter);
        }

        return query;
    }

    public IEnumerable<T> Evaluate<T>(IEnumerable<T> query, Specification<T> specification)
    {
        foreach (var whereExpression in specification.WhereExpressions)
        {
            query = query.Where(whereExpression.FilterFunc);
        }

        return query;
    }
}
