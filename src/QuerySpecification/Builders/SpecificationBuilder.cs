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

    void Add(int type, object value);
    void AddOrUpdate(int type, object value);
}

public interface ISpecificationBuilder<T>
{
    internal Specification<T> Specification { get; }

    void Add(int type, object value);
    void AddOrUpdate(int type, object value);
}

internal class SpecificationBuilder<T, TResult> : IOrderedSpecificationBuilder<T, TResult>, ISpecificationBuilder<T, TResult>
{
    public SpecificationBuilder(Specification<T, TResult> specification)
        => Specification = specification;

    public Specification<T, TResult> Specification { get; }

    public void Add(int type, object value)
        => Specification.Add(type, value);
    public void AddOrUpdate(int type, object value)
        => Specification.AddOrUpdate(type, value);
}

internal class SpecificationBuilder<T> : IOrderedSpecificationBuilder<T>, ISpecificationBuilder<T>
{
    public SpecificationBuilder(Specification<T> specification)
        => Specification = specification;

    public Specification<T> Specification { get; }

    public void Add(int type, object value)
        => Specification.Add(type, value);
    public void AddOrUpdate(int type, object value)
        => Specification.AddOrUpdate(type, value);
}
