namespace Pozitron.QuerySpecification;

public sealed class WhereEvaluator : IEvaluator, IInMemoryEvaluator
{
    private WhereEvaluator() { }
    public static WhereEvaluator Instance = new();

    public IQueryable<T> Evaluate<T>(IQueryable<T> source, Specification<T> specification) where T : class
    {
        if (specification.IsEmpty) return source;

        foreach (var item in specification._state)
        {
            if (item is WhereExpression<T> whereExpression)
            {
                source = source.Where(whereExpression.Filter);
            }
        }

        return source;
    }

    public IEnumerable<T> Evaluate<T>(IEnumerable<T> source, Specification<T> specification)
    {
        if (specification.IsEmpty) return source;

        foreach (var item in specification._state)
        {
            if (item is WhereExpression<T> whereExpression)
            {
                source = source.Where(whereExpression.FilterFunc);
            }
        }

        return source;
    }
}
