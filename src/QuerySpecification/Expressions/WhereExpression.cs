namespace Pozitron.QuerySpecification;

public sealed class WhereExpression<T>
{
    private Func<T, bool>? _filterFunc;
    public Expression<Func<T, bool>> Filter { get; }

    public WhereExpression(Expression<Func<T, bool>> filter)
    {
        ArgumentNullException.ThrowIfNull(filter);
        Filter = filter;
    }

    public Func<T, bool> FilterFunc => _filterFunc ??= Filter.Compile();
}
