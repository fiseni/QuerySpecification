using System.Diagnostics;

namespace Pozitron.QuerySpecification;

internal sealed class SpecLike<T>
{
    public Expression<Func<T, string?>> KeySelector { get; }
    public string Pattern { get; }

    public SpecLike(Expression<Func<T, string?>> keySelector, string pattern)
    {
        Debug.Assert(keySelector is not null);
        Debug.Assert(!string.IsNullOrEmpty(pattern));
        KeySelector = keySelector;
        Pattern = pattern;
    }
}

internal sealed class SpecLikeCompiled<T>
{
    public Func<T, string?> KeySelector { get; }
    public string Pattern { get; }

    // The pattern is immutable, so the matcher is built once (lazily) and reused for every row,
    // avoiding a cache lookup per match. A benign race may build it twice; both results are identical.
    private LikeMatcher? _matcher;

    public SpecLikeCompiled(Func<T, string?> keySelector, string pattern)
    {
        Debug.Assert(keySelector is not null);
        Debug.Assert(!string.IsNullOrEmpty(pattern));
        KeySelector = keySelector;
        Pattern = pattern;
    }

    public bool Matches(string? input)
    {
        if (input is null) return false;

        var matcher = _matcher ??= LikeMatcher.Create(Pattern);
        return matcher.IsMatch(input);
    }
}
