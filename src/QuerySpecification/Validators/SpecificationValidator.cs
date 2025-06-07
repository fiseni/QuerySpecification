namespace Pozitron.QuerySpecification;

/// <summary>
/// Validates specifications.
/// </summary>
public class SpecificationValidator
{
    /// <summary>
    /// Gets the default instance of the <see cref="SpecificationValidator"/> class.
    /// </summary>
    public static SpecificationValidator Default = new();

    /// <summary>
    /// Gets the list of validators.
    /// </summary>
    protected List<IValidator> Validators { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SpecificationValidator"/> class.
    /// </summary>
    public SpecificationValidator()
    {
        Validators = TypeDiscovery.IsAutoDiscoveryEnabled
            ? TypeDiscovery.GetValidators()
            :
            [
                WhereValidator.Instance,
                LikeValidator.Instance,
            ];
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SpecificationValidator"/> class with the specified validators.
    /// </summary>
    /// <param name="validators">The validators to use.</param>
    public SpecificationValidator(IEnumerable<IValidator> validators)
    {
        Validators = validators.ToList();
    }

    /// <summary>
    /// Determines whether the specified entity is valid according to the given specification.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="entity">The entity to validate.</param>
    /// <param name="specification">The specification to evaluate.</param>
    /// <returns>true if the entity is valid; otherwise, false.</returns>
    public virtual bool IsValid<T>(T entity, Specification<T> specification)
    {
        if (specification.IsEmpty) return true;

        foreach (var validator in Validators)
        {
            if (validator.IsValid(entity, specification) == false)
                return false;
        }

        return true;
    }
}
