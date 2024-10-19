namespace Pozitron.QuerySpecification;

public interface ISpecificationBuilder<T, TResult>
{
    internal Specification<T, TResult> Spec { get; }
}

public interface ISpecificationBuilder<T>
{
    internal Specification<T> Spec { get; }
}

internal class SpecificationBuilder<T, TResult> : ISpecificationBuilder<T, TResult>
{
    public Specification<T, TResult> Spec { get; }

    public SpecificationBuilder(Specification<T, TResult> specification)
    {
        Spec = specification;
    }
}

internal class SpecificationBuilder<T> : ISpecificationBuilder<T>
{
    public Specification<T> Spec { get; }

    public SpecificationBuilder(Specification<T> specification)
    {
        Spec = specification;
    }
}
