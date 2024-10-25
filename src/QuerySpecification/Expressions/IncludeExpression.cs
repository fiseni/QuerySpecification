namespace Pozitron.QuerySpecification;

public class IncludeExpression
{
    public LambdaExpression LambdaExpression { get; }
    public IncludeTypeEnum Type { get; }

    public IncludeExpression(LambdaExpression expression, IncludeTypeEnum type)
    {
        ArgumentNullException.ThrowIfNull(expression);

        LambdaExpression = expression;
        Type = type;
    }
}
