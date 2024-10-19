namespace Pozitron.QuerySpecification;

public interface ISpecificationBuilder<T, TResult>
{
    internal Specification<T, TResult> Spec { get; }
}

public interface ISpecificationBuilder<T>
{
    internal Specification<T> Spec { get; }
}
