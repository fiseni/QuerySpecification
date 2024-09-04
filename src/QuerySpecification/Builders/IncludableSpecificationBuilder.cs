namespace Pozitron.QuerySpecification;

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
