namespace Pozitron.QuerySpecification;

/// <summary>
/// Exception thrown when a selector is not found in the specification.
/// </summary>
public class SelectorNotFoundException : Exception
{
    private const string _message = "The specification must have a selector transform defined. Ensure either Select() or SelectMany() is used in the specification!";

    /// <summary>
    /// Initializes a new instance of the <see cref="SelectorNotFoundException"/> class.
    /// </summary>
    public SelectorNotFoundException()
        : base(_message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SelectorNotFoundException"/> class with a specified inner exception.
    /// </summary>
    /// <param name="innerException">The exception that is the cause of this exception.</param>
    public SelectorNotFoundException(Exception innerException)
        : base(_message, innerException)
    {
    }
}
