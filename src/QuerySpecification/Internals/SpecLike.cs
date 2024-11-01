namespace Pozitron.QuerySpecification;

internal sealed class SpecLike<T>
{
    private Func<T, string?>? _keySelectorFunc;
    public Expression<Func<T, string?>> KeySelector { get; }
    public string Pattern { get; }

    public SpecLike(Expression<Func<T, string?>> keySelector, string pattern)
    {
        KeySelector = keySelector;
        Pattern = pattern;
    }

    // TODO: Temporary [fatii, 01/11/2024]
    public Func<T, string?> KeySelectorFunc => _keySelectorFunc ??= KeySelector.Compile();
}
