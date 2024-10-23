namespace Pozitron.QuerySpecification;

public sealed class OrderByExpression<T> : OrderExpression<T>
{
    public OrderByExpression(Expression<Func<T, object?>> keySelector)
        : base(keySelector)
    {
    }
}

public sealed class OrderByDescendingExpression<T> : OrderExpression<T>
{
    public OrderByDescendingExpression(Expression<Func<T, object?>> keySelector)
        : base(keySelector)
    {
    }
}

public sealed class OrderThenByExpression<T> : OrderExpression<T>
{
    public OrderThenByExpression(Expression<Func<T, object?>> keySelector)
        : base(keySelector)
    {
    }
}

public sealed class OrderThenByDescendingExpression<T> : OrderExpression<T>
{
    public OrderThenByDescendingExpression(Expression<Func<T, object?>> keySelector)
        : base(keySelector)
    {
    }
}

public abstract class OrderExpression<T>
{
    private Func<T, object?>? _keySelectorFunc;
    public Expression<Func<T, object?>> KeySelector { get; }

    public OrderExpression(Expression<Func<T, object?>> keySelector)
    {
        ArgumentNullException.ThrowIfNull(keySelector);
        KeySelector = keySelector;
    }

    public Func<T, object?> KeySelectorFunc => _keySelectorFunc ??= KeySelector.Compile();

    public OrderTypeEnum OrderType => this switch
    {
        OrderByExpression<T> => OrderTypeEnum.OrderBy,
        OrderByDescendingExpression<T> => OrderTypeEnum.OrderByDescending,
        OrderThenByExpression<T> => OrderTypeEnum.ThenBy,
        OrderThenByDescendingExpression<T> => OrderTypeEnum.ThenByDescending,
        _ => throw new InvalidOperationException("Unknown OrderExpression type.")
    };
}
