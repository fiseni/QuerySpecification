namespace Pozitron.QuerySpecification;

public class Specification<T, TResult> : Specification<T>
{
    public new SpecificationContext<T, TResult> Context { get; }
    public new ISpecificationBuilder<T, TResult> Query { get; }

    protected Specification()
        : this(InMemorySpecificationEvaluator.Default)
    {
    }

    protected Specification(InMemorySpecificationEvaluator inMemorySpecificationEvaluator)
        : base(inMemorySpecificationEvaluator)
    {
        Context = new SpecificationContext<T, TResult>();
        Query = new SpecificationBuilder<T, TResult>(Context);
    }
}

public class Specification<T>
{
    private readonly InMemorySpecificationEvaluator _evaluator;
    private readonly SpecificationValidator _validator;
    public SpecificationContext<T> Context { get; }
    public ISpecificationBuilder<T> Query { get; }

    protected Specification()
        : this(InMemorySpecificationEvaluator.Default, SpecificationValidator.Default)
    {
    }

    protected Specification(InMemorySpecificationEvaluator inMemorySpecificationEvaluator)
        : this(inMemorySpecificationEvaluator, SpecificationValidator.Default)
    {
    }

    protected Specification(SpecificationValidator specificationValidator)
        : this(InMemorySpecificationEvaluator.Default, specificationValidator)
    {
    }

    protected Specification(InMemorySpecificationEvaluator inMemorySpecificationEvaluator, SpecificationValidator specificationValidator)
    {
        _evaluator = inMemorySpecificationEvaluator;
        _validator = specificationValidator;
        Context = new SpecificationContext<T>();
        Query = new SpecificationBuilder<T>(Context);
    }

    public virtual IEnumerable<T> Evaluate(IEnumerable<T> entities)
    {
        return _evaluator.Evaluate(entities, this);
    }

    public virtual bool IsSatisfiedBy(T entity)
    {
        return _validator.IsValid(entity, this);
    }
}
