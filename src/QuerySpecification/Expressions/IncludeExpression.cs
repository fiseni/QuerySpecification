using System.Diagnostics;

namespace Pozitron.QuerySpecification;

public sealed class IncludeExpression<T>
{
    public LambdaExpression LambdaExpression { get; }
    public IncludeType Type { get; }

    public IncludeExpression(LambdaExpression expression, IncludeType type)
    {
        Debug.Assert(expression is not null);
        LambdaExpression = expression;
        Type = type;
    }
}
