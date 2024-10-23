namespace Pozitron.QuerySpecification;

public interface IOrderedSpecificationBuilder<T, TResult> : ISpecificationBuilder<T, TResult>
{
}

public interface IOrderedSpecificationBuilder<T> : ISpecificationBuilder<T>
{
}
public interface ISpecificationBuilder<T, TResult>
{
    internal Specification<T, TResult> Specification { get; }
}

public interface ISpecificationBuilder<T>
{
    internal Specification<T> Specification { get; }
}

internal class SpecificationBuilder<T, TResult> : IOrderedSpecificationBuilder<T, TResult>, ISpecificationBuilder<T, TResult>
{
    public Specification<T, TResult> Specification { get; }
    public SpecificationBuilder(Specification<T, TResult> specification)
    {
        Specification = specification;
    }
}

internal class SpecificationBuilder<T> : IOrderedSpecificationBuilder<T>, ISpecificationBuilder<T>
{
    public Specification<T> Specification { get; }
    public SpecificationBuilder(Specification<T> specification)
    {
        Specification = specification;
    }
}

public interface IIncludableSpecificationBuilder<T, TResult, out TProperty> : ISpecificationBuilder<T, TResult> where T : class
{
}

public interface IIncludableSpecificationBuilder<T, out TProperty> : ISpecificationBuilder<T> where T : class
{
}

internal sealed class IncludableSpecificationBuilder<T, TResult, TProperty>(Specification<T, TResult> specification)
    : SpecificationBuilder<T, TResult>(specification), IIncludableSpecificationBuilder<T, TResult, TProperty> where T : class
{
}

internal sealed class IncludableSpecificationBuilder<T, TProperty>(Specification<T> specification)
    : SpecificationBuilder<T>(specification), IIncludableSpecificationBuilder<T, TProperty> where T : class
{
}
