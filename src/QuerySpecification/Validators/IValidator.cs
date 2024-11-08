namespace Pozitron.QuerySpecification;

/// <summary>
/// Represents a validator for specifications.
/// </summary>
public interface IValidator
{
    /// <summary>
    /// Determines whether the specified entity is valid according to the given specification.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="entity">The entity to validate.</param>
    /// <param name="specification">The specification to evaluate.</param>
    /// <returns>true if the entity is valid; otherwise, false.</returns>
    bool IsValid<T>(T entity, Specification<T> specification);
}
