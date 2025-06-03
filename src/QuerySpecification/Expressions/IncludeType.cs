namespace Pozitron.QuerySpecification;

/// <summary>
/// Specifies the type of include operation in a specification.
/// </summary>
public enum IncludeType
{
    /// <summary>
    /// Represents an Include operation.
    /// </summary>
    Include = 1,

    /// <summary>
    /// Represents a ThenInclude operation after reference include.
    /// </summary>
    ThenIncludeAfterReference = 2,

    /// <summary>
    /// Represents a ThenInclude operation after collection include.
    /// </summary>
    ThenIncludeAfterCollection = 3
}
