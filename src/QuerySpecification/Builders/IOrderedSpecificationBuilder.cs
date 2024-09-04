namespace Pozitron.QuerySpecification;

public interface IOrderedSpecificationBuilder<T> : ISpecificationBuilder<T>
{
    internal bool IsChainDiscarded { get; set; }
}
