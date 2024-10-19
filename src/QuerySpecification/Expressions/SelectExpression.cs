namespace Pozitron.QuerySpecification;

public class SelectExpression<T, TResult>
{
    public Expression<Func<T, TResult>>? Selector { get; internal set; }
    public Expression<Func<T, IEnumerable<TResult>>>? SelectorMany { get; internal set; }
}
