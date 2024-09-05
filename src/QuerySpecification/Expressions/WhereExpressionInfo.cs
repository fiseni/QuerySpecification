using System.Linq.Expressions;

namespace Pozitron.QuerySpecification;

public class WhereExpressionInfo<T>
{
    private Func<T, bool>? _filterFunc;
    public Expression<Func<T, bool>> Filter { get; }

    public WhereExpressionInfo(Expression<Func<T, bool>> filter)
    {
        ArgumentNullException.ThrowIfNull(filter);
        Filter = filter;
    }

    public Func<T, bool> FilterFunc => _filterFunc ??= Filter.Compile();
}
