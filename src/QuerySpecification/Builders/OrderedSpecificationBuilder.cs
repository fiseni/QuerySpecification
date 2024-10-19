namespace Pozitron.QuerySpecification;

public interface IOrderedSpecificationBuilder<T, TResult> : ISpecificationBuilder<T, TResult>
{
    internal bool IsChainDiscarded { get; set; }
}

public interface IOrderedSpecificationBuilder<T> : ISpecificationBuilder<T>
{
    internal bool IsChainDiscarded { get; set; }
}

internal struct OrderedSpecificationBuilder<T, TResult> : IOrderedSpecificationBuilder<T, TResult>
{
    public Specification<T, TResult> Spec { get; }
    public bool IsChainDiscarded { get; set; }

    public OrderedSpecificationBuilder(Specification<T, TResult> specification, bool isChainDiscarded)
    {
        Spec = specification;
        IsChainDiscarded = isChainDiscarded;
    }
}

internal struct OrderedSpecificationBuilder<T> : IOrderedSpecificationBuilder<T>
{
    public Specification<T> Spec { get; }
    public bool IsChainDiscarded { get; set; }

    public OrderedSpecificationBuilder(Specification<T> specification, bool isChainDiscarded)
    {
        Spec = specification;
        IsChainDiscarded = isChainDiscarded;
    }
}
