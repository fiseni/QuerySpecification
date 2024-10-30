namespace Pozitron.QuerySpecification;

internal sealed class SpecLike<T>
{
    public Expression<Func<T, string?>> KeySelector { get; }
    public string Pattern { get; }

    public SpecLike(Expression<Func<T, string?>> keySelector, string pattern)
    {
        KeySelector = keySelector;
        Pattern = pattern;
    }
}
