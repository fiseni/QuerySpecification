using System.Linq.Expressions;

namespace Pozitron.QuerySpecification;

public class LikeExpression<T>
{
    private Func<T, string?>? _keySelectorFunc;
    public Expression<Func<T, string?>> KeySelector { get; }
    public string Pattern { get; }
    public int Group { get; }

    public LikeExpression(Expression<Func<T, string?>> keySelector, string pattern, int group = 1)
    {
        ArgumentNullException.ThrowIfNull(keySelector);
        ArgumentException.ThrowIfNullOrEmpty(pattern);

        KeySelector = keySelector;
        Pattern = pattern;
        Group = group;
    }

    public Func<T, string?> KeySelectorFunc => _keySelectorFunc ??= KeySelector.Compile();
}
