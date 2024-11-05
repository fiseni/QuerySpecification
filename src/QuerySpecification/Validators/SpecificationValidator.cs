namespace Pozitron.QuerySpecification;

public class SpecificationValidator
{
    public static SpecificationValidator Default = new();

    protected List<IValidator> Validators { get; }

    public SpecificationValidator()
    {
        Validators =
        [
            WhereValidator.Instance,
            LikeValidator.Instance
        ];
    }
    public SpecificationValidator(IEnumerable<IValidator> validators)
    {
        Validators = validators.ToList();
    }

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
