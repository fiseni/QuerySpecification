namespace Pozitron.QuerySpecification;

/// <summary>
/// Specifies the type of order operation in a specification.
/// </summary>
public enum OrderType
{
    /// <summary>
    /// Represents an order by operation.
    /// </summary>
    OrderBy = 1,

    /// <summary>
    /// Represents an order by descending operation.
    /// </summary>
    OrderByDescending = 2,

    /// <summary>
    /// Represents a then by operation.
    /// </summary>
    ThenBy = 3,

    /// <summary>
    /// Represents a then by descending operation.
    /// </summary>
    ThenByDescending = 4
}
