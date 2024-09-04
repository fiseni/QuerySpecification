namespace Pozitron.QuerySpecification;

public interface ISpecificationBuilder<T, TResult> : ISpecificationBuilder<T>
{
    internal new Specification<T, TResult> Specification { get; }
}

public interface ISpecificationBuilder<T>
{
    internal Specification<T> Specification { get; }
}
