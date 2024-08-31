namespace Pozitron.QuerySpecification;

public class OrderedSpecificationBuilder<T> : IOrderedSpecificationBuilder<T>
{
    public SpecificationContext<T> Context { get; }
    public bool IsChainDiscarded { get; set; }

    public OrderedSpecificationBuilder(SpecificationContext<T> context)
        : this(context, false)
    {
    }

    public OrderedSpecificationBuilder(SpecificationContext<T> context, bool isChainDiscarded)
    {
        Context = context;
        IsChainDiscarded = isChainDiscarded;
    }
}
