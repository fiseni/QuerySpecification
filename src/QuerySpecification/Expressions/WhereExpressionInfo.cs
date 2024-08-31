using System.Linq.Expressions;

namespace Pozitron.QuerySpecification;

public class WhereExpressionInfo<T>
{
    private readonly Lazy<Func<T, bool>> _filterFunc;

    public WhereExpressionInfo(Expression<Func<T, bool>> filter)
    {
        _ = filter ?? throw new ArgumentNullException(nameof(filter));

        Filter = filter;

        _filterFunc = new Lazy<Func<T, bool>>(Filter.Compile);
    }

    public Expression<Func<T, bool>> Filter { get; }

    public Func<T, bool> FilterFunc => _filterFunc.Value;
}
