namespace Pozitron.QuerySpecification;

public interface ISpecificationBuilder<T, TResult> : ISpecificationBuilder<T>
{
    new SpecificationContext<T, TResult> Context { get; }
}

public interface ISpecificationBuilder<T>
{
    SpecificationContext<T> Context { get; }
}
