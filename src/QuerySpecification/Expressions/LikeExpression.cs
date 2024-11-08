using System.Diagnostics;

namespace Pozitron.QuerySpecification;

/// <summary>
/// Represents a like expression used in a specification.
/// </summary>
/// <typeparam name="T">The type of the entity.</typeparam>
public sealed class LikeExpression<T>
{
    /// <summary>
    /// Gets the key selector expression.
    /// </summary>
    public Expression<Func<T, string?>> KeySelector { get; }

    /// <summary>
    /// Gets the pattern to match.
    /// </summary>
    public string Pattern { get; }

    /// <summary>
    /// Gets the group number.
    /// </summary>
    public int Group { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="LikeExpression{T}"/> class.
    /// </summary>
    /// <param name="keySelector">The key selector expression.</param>
    /// <param name="pattern">The pattern to match.</param>
    /// <param name="group">The group number.</param>
    public LikeExpression(Expression<Func<T, string?>> keySelector, string pattern, int group = 1)
    {
        Debug.Assert(keySelector is not null);
        Debug.Assert(!string.IsNullOrEmpty(pattern));
        KeySelector = keySelector;
        Pattern = pattern;
        Group = group;
    }
}

/// <summary>
/// Represents a compiled like expression used in a specification.
/// </summary>
/// <typeparam name="T">The type of the entity.</typeparam>
public sealed class LikeExpressionCompiled<T>
{
    /// <summary>
    /// Gets the compiled key selector function.
    /// </summary>
    public Func<T, string?> KeySelector { get; }

    /// <summary>
    /// Gets the pattern to match.
    /// </summary>
    public string Pattern { get; }

    /// <summary>
    /// Gets the group number.
    /// </summary>
    public int Group { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="LikeExpressionCompiled{T}"/> class.
    /// </summary>
    /// <param name="keySelector">The compiled key selector function.</param>
    /// <param name="pattern">The pattern to match.</param>
    /// <param name="group">The group number.</param>
    public LikeExpressionCompiled(Func<T, string?> keySelector, string pattern, int group = 1)
    {
        Debug.Assert(keySelector is not null);
        Debug.Assert(!string.IsNullOrEmpty(pattern));
        KeySelector = keySelector;
        Pattern = pattern;
        Group = group;
    }
}
