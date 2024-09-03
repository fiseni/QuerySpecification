namespace Pozitron.QuerySpecification;

public interface IOrderedSpecificationBuilder<T> : ISpecificationBuilder<T>
{
    bool IsChainDiscarded { get; set; }
}
