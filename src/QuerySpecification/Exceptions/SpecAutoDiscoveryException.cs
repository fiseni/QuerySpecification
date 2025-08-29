namespace Pozitron.QuerySpecification;

/// <summary>
/// Exception thrown when auto discovery fails to scan the loaded assemblies.
/// </summary>
public class SpecAutoDiscoveryException : Exception
{
    private const string _message = "The auto-discovery of evaluators failed while scanning the loaded assemblies. Disable the feature and contact the authors!";

    /// <summary>
    /// Initializes a new instance of the <see cref="SpecAutoDiscoveryException"/> class.
    /// </summary>
    public SpecAutoDiscoveryException()
        : base(_message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SpecAutoDiscoveryException"/> class with a specified inner exception.
    /// </summary>
    /// <param name="innerException">The exception that is the cause of this exception.</param>
    public SpecAutoDiscoveryException(Exception innerException)
        : base(_message, innerException)
    {
    }
}
