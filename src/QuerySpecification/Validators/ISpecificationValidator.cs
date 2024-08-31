namespace Pozitron.QuerySpecification;

public interface ISpecificationValidator
{
    bool IsValid<T>(T entity, Specification<T> specification);
}
