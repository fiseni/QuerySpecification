using System.Diagnostics;

namespace Pozitron.QuerySpecification;

/// <summary>
/// Represents an order expression used in a specification.
/// </summary>
/// <typeparam name="T">The type of the entity.</typeparam>
public sealed class OrderExpression<T>
{
    /// <summary>
    /// Gets the key selector expression.
    /// </summary>
    public Expression<Func<T, object?>> KeySelector { get; }

    /// <summary>
    /// Gets the type of the order.
    /// </summary>
    public OrderType Type { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="OrderExpression{T}"/> class.
    /// </summary>
    /// <param name="keySelector">The key selector expression.</param>
    /// <param name="type">The type of the order.</param>
    public OrderExpression(Expression<Func<T, object?>> keySelector, OrderType type)
    {
        Debug.Assert(keySelector is not null);
        KeySelector = keySelector;
        Type = type;
    }
}

/// <summary>
/// Represents a compiled order expression used in a specification.
/// </summary>
/// <typeparam name="T">The type of the entity.</typeparam>
public sealed class OrderExpressionCompiled<T>
{
    /// <summary>
    /// Gets the compiled key selector function.
    /// </summary>
    public Func<T, object?> KeySelector { get; }

    /// <summary>
    /// Gets the type of the order.
    /// </summary>
    public OrderType Type { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="OrderExpressionCompiled{T}"/> class.
    /// </summary>
    /// <param name="keySelector">The compiled key selector function.</param>
    /// <param name="type">The type of the order.</param>
    public OrderExpressionCompiled(Func<T, object?> keySelector, OrderType type)
    {
        Debug.Assert(keySelector is not null);
        KeySelector = keySelector;
        Type = type;
    }
}
