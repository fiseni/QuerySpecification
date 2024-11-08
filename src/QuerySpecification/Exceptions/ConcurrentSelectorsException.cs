namespace Pozitron.QuerySpecification;

/// <summary>
/// Exception thrown when concurrent selectors are defined in the specification.
/// </summary>
public class ConcurrentSelectorsException : Exception
{
    private const string _message = "Concurrent specification selector transforms defined. Ensure only one of the Select() or SelectMany() transforms is used in the same specification!";

    /// <summary>
    /// Initializes a new instance of the <see cref="ConcurrentSelectorsException"/> class.
    /// </summary>
    public ConcurrentSelectorsException()
        : base(_message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ConcurrentSelectorsException"/> class with a specified inner exception.
    /// </summary>
    /// <param name="innerException">The exception that is the cause of this exception.</param>
    public ConcurrentSelectorsException(Exception innerException)
        : base(_message, innerException)
    {
    }
}
