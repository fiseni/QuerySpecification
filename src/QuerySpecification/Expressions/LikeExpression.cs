namespace Pozitron.QuerySpecification;

public sealed class LikeExpression<T>
{
    public Expression<Func<T, string?>> KeySelector { get; }
    public string Pattern { get; }
    public int Group { get; }

    public LikeExpression(Expression<Func<T, string?>> keySelector, string pattern, int group = 1)
    {
        KeySelector = keySelector;
        Pattern = pattern;
        Group = group;
    }
}

public sealed class LikeExpressionCompiled<T>
{
    public Func<T, string?> KeySelector { get; }
    public string Pattern { get; }
    public int Group { get; }

    public LikeExpressionCompiled(Func<T, string?> keySelector, string pattern, int group = 1)
    {
        KeySelector = keySelector;
        Pattern = pattern;
        Group = group;
    }
}
