namespace Pozitron.QuerySpecification;

public sealed class OrderExpression<T>
{
    private Func<T, object?>? _keySelectorFunc;
    public Expression<Func<T, object?>> KeySelector { get; }
    public OrderTypeEnum OrderType { get; }

    public OrderExpression(Expression<Func<T, object?>> keySelector, OrderTypeEnum type)
    {
        ArgumentNullException.ThrowIfNull(keySelector);

        KeySelector = keySelector;
        OrderType = type;
    }

    public Func<T, object?> KeySelectorFunc => _keySelectorFunc ??= KeySelector.Compile();
}
