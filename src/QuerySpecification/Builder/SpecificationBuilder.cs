namespace Pozitron.QuerySpecification;

public class SpecificationBuilder<T, TResult> : SpecificationBuilder<T>, ISpecificationBuilder<T, TResult>
{
    public new SpecificationContext<T, TResult> Context { get; }

    public SpecificationBuilder(SpecificationContext<T, TResult> context)
        : base(context)
    {
        Context = context;
    }
}

public class SpecificationBuilder<T> : ISpecificationBuilder<T>
{
    public SpecificationContext<T> Context { get; }

    public SpecificationBuilder(SpecificationContext<T> context)
    {
        Context = context;
    }
}
