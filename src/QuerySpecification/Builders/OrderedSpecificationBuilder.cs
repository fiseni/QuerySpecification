namespace Pozitron.QuerySpecification;

public interface IOrderedSpecificationBuilder<T, TResult> : ISpecificationBuilder<T, TResult>
{
    internal bool IsChainDiscarded { get; set; }
}

public interface IOrderedSpecificationBuilder<T> : ISpecificationBuilder<T>
{
    internal bool IsChainDiscarded { get; set; }
}

internal class OrderedSpecificationBuilder<T, TResult> : IOrderedSpecificationBuilder<T, TResult>
{
    public Specification<T, TResult> Specification { get; }
    public bool IsChainDiscarded { get; set; }

    public OrderedSpecificationBuilder(Specification<T, TResult> specification)
        : this(specification, false)
    {
    }

    public OrderedSpecificationBuilder(Specification<T, TResult> specification, bool isChainDiscarded)
    {
        Specification = specification;
        IsChainDiscarded = isChainDiscarded;
    }
}

internal class OrderedSpecificationBuilder<T> : IOrderedSpecificationBuilder<T>
{
    public Specification<T> Specification { get; }
    public bool IsChainDiscarded { get; set; }

    public OrderedSpecificationBuilder(Specification<T> specification)
        : this(specification, false)
    {
    }

    public OrderedSpecificationBuilder(Specification<T> specification, bool isChainDiscarded)
    {
        Specification = specification;
        IsChainDiscarded = isChainDiscarded;
    }
}
