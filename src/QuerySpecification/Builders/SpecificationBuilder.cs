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

internal class SpecificationBuilder<T, TResult>(Specification<T, TResult> specification)
    : IOrderedSpecificationBuilder<T, TResult>, ISpecificationBuilder<T, TResult>
{
    public Specification<T, TResult> Specification { get; } = specification;
    public void Add(int type, object value) => Specification.Add(type, value);
    public void AddOrUpdate(int type, object value) => Specification.AddOrUpdate(type, value);
}

internal class SpecificationBuilder<T>(Specification<T> specification)
    : IOrderedSpecificationBuilder<T>, ISpecificationBuilder<T>
{
    public Specification<T> Specification { get; } = specification;
    public void Add(int type, object value) => Specification.Add(type, value);
    public void AddOrUpdate(int type, object value) => Specification.AddOrUpdate(type, value);
}
