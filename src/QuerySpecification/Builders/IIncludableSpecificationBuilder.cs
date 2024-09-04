namespace Pozitron.QuerySpecification;

public interface IIncludableSpecificationBuilder<T, out TProperty> : ISpecificationBuilder<T> where T : class
{
    internal bool IsChainDiscarded { get; set; }
}
