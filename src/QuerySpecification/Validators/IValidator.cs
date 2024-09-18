namespace Pozitron.QuerySpecification;

public interface IValidator
{
    bool IsValid<T>(T entity, Specification<T> specification);
}
