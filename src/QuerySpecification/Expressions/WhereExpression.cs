using System.Diagnostics;

namespace Pozitron.QuerySpecification;

/// <summary>
/// Represents a where expression used in a specification.
/// </summary>
/// <typeparam name="T">The type of the entity.</typeparam>
public sealed class WhereExpression<T>
{
    /// <summary>
    /// Gets the filter expression.
    /// </summary>
    public Expression<Func<T, bool>> Filter { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="WhereExpression{T}"/> class.
    /// </summary>
    /// <param name="filter">The filter expression.</param>
    public WhereExpression(Expression<Func<T, bool>> filter)
    {
        Debug.Assert(filter is not null);
        Filter = filter;
    }
}

/// <summary>
/// Represents a compiled where expression used in a specification.
/// </summary>
/// <typeparam name="T">The type of the entity.</typeparam>
public sealed class WhereExpressionCompiled<T>
{
    /// <summary>
    /// Gets the compiled filter function.
    /// </summary>
    public Func<T, bool> Filter { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="WhereExpressionCompiled{T}"/> class.
    /// </summary>
    /// <param name="filter">The compiled filter function.</param>
    public WhereExpressionCompiled(Func<T, bool> filter)
    {
        Debug.Assert(filter is not null);
        Filter = filter;
    }
}
