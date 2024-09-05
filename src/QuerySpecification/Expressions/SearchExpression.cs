using System.Linq.Expressions;

namespace Pozitron.QuerySpecification;

public class SearchExpression<T>
{
    private Func<T, string>? _selectorFunc;
    public Expression<Func<T, string>> Selector { get; }
    public string SearchTerm { get; }
    public int SearchGroup { get; }

    public SearchExpression(Expression<Func<T, string>> selector, string searchTerm, int searchGroup = 1)
    {
        ArgumentNullException.ThrowIfNull(selector);
        ArgumentException.ThrowIfNullOrEmpty(searchTerm);

        Selector = selector;
        SearchTerm = searchTerm;
        SearchGroup = searchGroup;
    }

    public Func<T, string> SelectorFunc => _selectorFunc ??= Selector.Compile();
}
