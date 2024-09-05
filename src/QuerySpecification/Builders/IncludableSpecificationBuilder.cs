namespace Pozitron.QuerySpecification;

public interface IIncludableSpecificationBuilder<T, TResult, out TProperty> : ISpecificationBuilder<T, TResult> where T : class
{
    internal bool IsChainDiscarded { get; set; }
}

public interface IIncludableSpecificationBuilder<T, out TProperty> : ISpecificationBuilder<T> where T : class
{
    internal bool IsChainDiscarded { get; set; }
}

internal class IncludableSpecificationBuilder<T, TResult, TProperty> : IIncludableSpecificationBuilder<T, TResult, TProperty> where T : class
{
    public Specification<T, TResult> Specification { get; }
    public bool IsChainDiscarded { get; set; }

    public IncludableSpecificationBuilder(Specification<T, TResult> specification)
        : this(specification, false)
    {
    }

    public IncludableSpecificationBuilder(Specification<T, TResult> specification, bool isChainDiscarded)
    {
        Specification = specification;
        IsChainDiscarded = isChainDiscarded;
    }
}

internal class IncludableSpecificationBuilder<T, TProperty> : IIncludableSpecificationBuilder<T, TProperty> where T : class
{
    public Specification<T> Specification { get; }
    public bool IsChainDiscarded { get; set; }

    public IncludableSpecificationBuilder(Specification<T> specification)
        : this(specification, false)
    {
    }

    public IncludableSpecificationBuilder(Specification<T> specification, bool isChainDiscarded)
    {
        Specification = specification;
        IsChainDiscarded = isChainDiscarded;
    }
}
