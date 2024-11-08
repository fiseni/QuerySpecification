namespace Pozitron.QuerySpecification;

/// <summary>
/// Exception thrown when an invalid like pattern is encountered.
/// </summary>
public class InvalidLikePatternException : Exception
{
    private const string _message = "Invalid like pattern: ";

    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidLikePatternException"/> class with a specified pattern.
    /// </summary>
    /// <param name="pattern">The invalid like pattern.</param>
    public InvalidLikePatternException(string pattern)
        : base($"{_message}{pattern}")
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidLikePatternException"/> class with a specified pattern and inner exception.
    /// </summary>
    /// <param name="pattern">The invalid like pattern.</param>
    /// <param name="innerException">The exception that is the cause of this exception.</param>
    public InvalidLikePatternException(string pattern, Exception innerException)
        : base($"{_message}{pattern}", innerException)
    {
    }
}
