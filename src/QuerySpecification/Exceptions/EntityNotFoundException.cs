namespace Pozitron.QuerySpecification;

/// <summary>
/// Exception thrown when an entity is not found.
/// </summary>
public class EntityNotFoundException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EntityNotFoundException"/> class.
    /// </summary>
    public EntityNotFoundException()
        : base($"The queried entity was not found!")
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EntityNotFoundException"/> class with a specified entity name.
    /// </summary>
    /// <param name="entityName">The name of the entity that was not found.</param>
    public EntityNotFoundException(string entityName)
        : base($"The queried entity: {entityName} was not found!")
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EntityNotFoundException"/> class with a specified entity name and inner exception.
    /// </summary>
    /// <param name="entityName">The name of the entity that was not found.</param>
    /// <param name="innerException">The exception that is the cause of this exception.</param>
    public EntityNotFoundException(string entityName, Exception innerException)
        : base($"The queried entity: {entityName} was not found!", innerException)
    {
    }
}
