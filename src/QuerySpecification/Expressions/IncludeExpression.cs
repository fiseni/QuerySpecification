namespace Pozitron.QuerySpecification;

public sealed class IncludeThenExpression : IncludeExpression
{
    public IncludeThenExpression(LambdaExpression expression)
        : base(expression)
    {
    }
}

public class IncludeExpression
{
    public LambdaExpression LambdaExpression { get; }

    public IncludeExpression(LambdaExpression expression)
    {
        ArgumentNullException.ThrowIfNull(expression);
        LambdaExpression = expression;
    }

    public IncludeTypeEnum Type => this switch
    {
        IncludeThenExpression => IncludeTypeEnum.ThenInclude,
        IncludeExpression => IncludeTypeEnum.Include,
        _ => throw new InvalidOperationException("Unknown IncludeExpression type.")
    };
}
