﻿using System.Diagnostics;

namespace Pozitron.QuerySpecification;

public sealed class OrderExpression<T>
{
    public Expression<Func<T, object?>> KeySelector { get; }
    public OrderType Type { get; }

    public OrderExpression(Expression<Func<T, object?>> keySelector, OrderType type)
    {
        Debug.Assert(keySelector is not null);
        KeySelector = keySelector;
        Type = type;
    }
}

public sealed class OrderExpressionCompiled<T>
{
    public Func<T, object?> KeySelector { get; }
    public OrderType Type { get; }

    public OrderExpressionCompiled(Func<T, object?> keySelector, OrderType type)
    {
        Debug.Assert(keySelector is not null);
        KeySelector = keySelector;
        Type = type;
    }
}
