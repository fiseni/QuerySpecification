using System.Linq.Expressions;

namespace Pozitron.QuerySpecification;

public class OrderExpressionInfo<T>
{
    private readonly Lazy<Func<T, object?>> _keySelectorFunc;

    public OrderExpressionInfo(Expression<Func<T, object?>> keySelector, OrderTypeEnum orderType)
    {
        _ = keySelector ?? throw new ArgumentNullException(nameof(keySelector));

        KeySelector = keySelector;
        OrderType = orderType;

        _keySelectorFunc = new Lazy<Func<T, object?>>(KeySelector.Compile);
    }

    public Expression<Func<T, object?>> KeySelector { get; }

    public OrderTypeEnum OrderType { get; }

    public Func<T, object?> KeySelectorFunc => _keySelectorFunc.Value;
}
