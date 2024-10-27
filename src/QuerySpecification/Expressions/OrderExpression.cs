namespace Pozitron.QuerySpecification;

public sealed class OrderExpression<T>
{
    private Func<T, object?>? _keySelectorFunc;
    public Expression<Func<T, object?>> KeySelector { get; }
    public OrderType Type { get; }

    public OrderExpression(Expression<Func<T, object?>> keySelector, OrderType type)
    {
        ArgumentNullException.ThrowIfNull(keySelector);

        KeySelector = keySelector;
        Type = type;
    }

    public Func<T, object?> KeySelectorFunc => _keySelectorFunc ??= KeySelector.Compile();
}
