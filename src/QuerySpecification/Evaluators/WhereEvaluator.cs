namespace Pozitron.QuerySpecification;

public class WhereEvaluator : IEvaluator, IInMemoryEvaluator
{
    private WhereEvaluator() { }
    public static WhereEvaluator Instance = new();

    public IQueryable<T> Evaluate<T>(IQueryable<T> source, Specification<T> specification) where T : class
    {
        foreach (var whereExpression in specification.WhereExpressions)
        {
            source = source.Where(whereExpression.Filter);
        }

        return source;
    }

    public IEnumerable<T> Evaluate<T>(IEnumerable<T> source, Specification<T> specification)
    {
        foreach (var whereExpression in specification.WhereExpressions)
        {
            source = source.Where(whereExpression.FilterFunc);
        }

        return source;
    }
}
