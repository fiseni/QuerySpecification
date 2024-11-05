using System.Diagnostics;

namespace Pozitron.QuerySpecification;

public sealed class WhereExpression<T>
{
    public Expression<Func<T, bool>> Filter { get; }

    public WhereExpression(Expression<Func<T, bool>> filter)
    {
        Debug.Assert(filter is not null);
        Filter = filter;
    }
}

public sealed class WhereExpressionCompiled<T>
{
    public Func<T, bool> Filter { get; }

    public WhereExpressionCompiled(Func<T, bool> filter)
    {
        Debug.Assert(filter is not null);
        Filter = filter;
    }
}
