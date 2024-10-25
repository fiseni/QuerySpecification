namespace Pozitron.QuerySpecification;

public sealed class OrderEvaluator : IEvaluator, IInMemoryEvaluator
{
    private OrderEvaluator() { }
    public static OrderEvaluator Instance = new();

    public IQueryable<T> Evaluate<T>(IQueryable<T> source, Specification<T> specification) where T : class
    {
        if (specification.IsEmpty) return source;

        IOrderedQueryable<T>? orderedQuery = null;

        foreach (var state in specification.State)
        {
            if (state.Type == StateType.Order && state.Reference is not null)
            {
                var expr = (Expression<Func<T, object?>>)state.Reference;
                if (state.Bag == (int)OrderTypeEnum.OrderBy)
                {
                    orderedQuery = source.OrderBy(expr);
                }
                else if (state.Bag == (int)OrderTypeEnum.OrderByDescending)
                {
                    orderedQuery = source.OrderByDescending(expr);
                }
                else if (state.Bag == (int)OrderTypeEnum.ThenBy)
                {
                    orderedQuery = orderedQuery!.ThenBy(expr);
                }
                else if (state.Bag == (int)OrderTypeEnum.ThenByDescending)
                {
                    orderedQuery = orderedQuery!.ThenByDescending(expr);
                }
            }
        }

        if (orderedQuery is not null)
        {
            source = orderedQuery;
        }

        return source;
    }

    public IEnumerable<T> Evaluate<T>(IEnumerable<T> source, Specification<T> specification)
    {
        if (specification.IsEmpty) return source;

        IOrderedEnumerable<T>? orderedQuery = null;

        foreach (var state in specification.State)
        {
            if (state.Type == StateType.Order && state.Reference is not null)
            {
                var func = ((Expression<Func<T, object?>>)state.Reference).Compile();
                if (state.Bag == (int)OrderTypeEnum.OrderBy)
                {
                    orderedQuery = source.OrderBy(func);
                }
                else if (state.Bag == (int)OrderTypeEnum.OrderByDescending)
                {
                    orderedQuery = source.OrderByDescending(func);
                }
                else if (state.Bag == (int)OrderTypeEnum.ThenBy)
                {
                    orderedQuery = orderedQuery!.ThenBy(func);
                }
                else if (state.Bag == (int)OrderTypeEnum.ThenByDescending)
                {
                    orderedQuery = orderedQuery!.ThenByDescending(func);
                }
            }
        }

        if (orderedQuery is not null)
        {
            source = orderedQuery;
        }

        return source;
    }
}
