namespace Pozitron.QuerySpecification;

public sealed class WhereEvaluator : IEvaluator, IInMemoryEvaluator
{
    private WhereEvaluator() { }
    public static WhereEvaluator Instance = new();

    public IQueryable<T> Evaluate<T>(IQueryable<T> source, Specification<T> specification) where T : class
    {
        foreach (var item in specification.Items)
        {
            if (item.Type == ItemType.Where && item.Reference is Expression<Func<T, bool>> expr)
            {
                source = source.Where(expr);
            }
        }

        return source;
    }

    public IEnumerable<T> Evaluate<T>(IEnumerable<T> source, Specification<T> specification)
    {
        var compiledItems = specification.GetCompiledItems();

        foreach (var item in compiledItems)
        {
            if (item.Type == ItemType.Where && item.Reference is Func<T, bool> compiledExpr)
            {
                source = source.Where(compiledExpr);
            }
        }

        return source;
    }
}

