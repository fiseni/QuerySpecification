using System.Diagnostics;

namespace Pozitron.QuerySpecification;

/// <summary>
/// Represents an include expression used in a specification.
/// </summary>
/// <typeparam name="T">The type of the entity.</typeparam>
public sealed class IncludeExpression<T>
{
    /// <summary>
    /// Gets the lambda expression for the include.
    /// </summary>
    public LambdaExpression LambdaExpression { get; }

    /// <summary>
    /// Gets the type of the include.
    /// </summary>
    public IncludeType Type { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="IncludeExpression{T}"/> class.
    /// </summary>
    /// <param name="expression">The lambda expression for the include.</param>
    /// <param name="type">The type of the include.</param>
    public IncludeExpression(LambdaExpression expression, IncludeType type)
    {
        Debug.Assert(expression is not null);
        LambdaExpression = expression;
        Type = type;
    }
}
