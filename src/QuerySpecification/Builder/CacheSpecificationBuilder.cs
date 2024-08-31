namespace Pozitron.QuerySpecification;

public class CacheSpecificationBuilder<T> : ICacheSpecificationBuilder<T> where T : class
{
    public SpecificationContext<T> Context { get; }
    public bool IsChainDiscarded { get; set; }

    public CacheSpecificationBuilder(SpecificationContext<T> context)
        : this(context, false)
    {
    }

    public CacheSpecificationBuilder(SpecificationContext<T> context, bool isChainDiscarded)
    {
        Context = context;
        IsChainDiscarded = isChainDiscarded;
    }
}
