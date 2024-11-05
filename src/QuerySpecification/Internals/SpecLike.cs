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

    public SpecLikeCompiled(Func<T, string?> keySelector, string pattern)
    {
        Debug.Assert(keySelector is not null);
        Debug.Assert(!string.IsNullOrEmpty(pattern));
        KeySelector = keySelector;
        Pattern = pattern;
    }
}
