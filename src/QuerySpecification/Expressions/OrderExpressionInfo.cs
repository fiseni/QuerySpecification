using System.Linq.Expressions;

namespace Pozitron.QuerySpecification;

public class OrderExpressionInfo<T>
{
    private Func<T, object?>? _keySelectorFunc;
    public Expression<Func<T, object?>> KeySelector { get; }
    public OrderTypeEnum OrderType { get; }

    public OrderExpressionInfo(Expression<Func<T, object?>> keySelector, OrderTypeEnum orderType)
    {
        ArgumentNullException.ThrowIfNull(keySelector);
        KeySelector = keySelector;
        OrderType = orderType;
    }

    public Func<T, object?> KeySelectorFunc => _keySelectorFunc ??= KeySelector.Compile();
}
