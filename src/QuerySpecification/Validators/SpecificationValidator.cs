namespace Pozitron.QuerySpecification;

public class SpecificationValidator
{
    // Will use singleton for default configuration. Yet, it can be instantiated if necessary, with default or provided validators.
    public static SpecificationValidator Default = new();

    private readonly List<IValidator> _validators;

    public SpecificationValidator()
    {
        _validators =
        [
            WhereValidator.Instance,
            SearchValidator.Instance
        ];
    }
    public SpecificationValidator(IEnumerable<IValidator> validators)
    {
        _validators = validators.ToList();
    }

    public virtual bool IsValid<T>(T entity, Specification<T> specification)
    {
        foreach (var partialValidator in _validators)
        {
            if (partialValidator.IsValid(entity, specification) == false) return false;
        }

        return true;
    }
}
