namespace Pozitron.QuerySpecification;

public class IncludableSpecificationBuilder<T, TProperty> : IIncludableSpecificationBuilder<T, TProperty> where T : class
{
    public SpecificationContext<T> Context { get; }
    public bool IsChainDiscarded { get; set; }

    public IncludableSpecificationBuilder(SpecificationContext<T> context)
        : this(context, false)
    {
    }

    public IncludableSpecificationBuilder(SpecificationContext<T> context, bool isChainDiscarded)
    {
        Context = context;
        IsChainDiscarded = isChainDiscarded;
    }
}
