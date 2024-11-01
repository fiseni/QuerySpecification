namespace Pozitron.QuerySpecification;

public sealed class WhereExpression<T>
{
    public Expression<Func<T, bool>> Filter { get; }

    public WhereExpression(Expression<Func<T, bool>> filter)
    {
        Filter = filter;
    }
}

public sealed class WhereExpressionCompiled<T>
{
    public Func<T, bool> Filter { get; }

    public WhereExpressionCompiled(Func<T, bool> filter)
    {
        Filter = filter;
    }
}
