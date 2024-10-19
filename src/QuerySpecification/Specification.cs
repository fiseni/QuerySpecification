namespace Pozitron.QuerySpecification;

public class Specification<T, TResult> : Specification<T>, ISpecificationBuilder<T, TResult>
{
    public new Specification<T, TResult> Spec => this;
    public new ISpecificationBuilder<T, TResult> Query => this;

    public Expression<Func<T, TResult>>? Selector
    {
        get => Get<SelectExpression<T, TResult>>()?.Selector ?? null;
        internal set => GetOrCreate<SelectExpression<T, TResult>>().Selector = value;
    }
    public Expression<Func<T, IEnumerable<TResult>>>? SelectorMany
    {
        get => Get<SelectExpression<T, TResult>>()?.SelectorMany ?? null;
        internal set => GetOrCreate<SelectExpression<T, TResult>>().SelectorMany = value;
    }

    public new virtual IEnumerable<TResult> Evaluate(IEnumerable<T> entities, bool ignorePaging = false)
    {
        var evaluator = Evaluator;
        return evaluator.Evaluate(entities, this, ignorePaging);
    }
}

public class Specification<T> : ISpecificationBuilder<T>
{
    public ISpecificationBuilder<T> Query => this;
    public Specification<T> Spec => this;

    protected virtual SpecificationInMemoryEvaluator Evaluator => SpecificationInMemoryEvaluator.Default;
    protected virtual SpecificationValidator Validator => SpecificationValidator.Default;

    public virtual IEnumerable<T> Evaluate(IEnumerable<T> entities, bool ignorePaging = false)
    {
        var evaluator = Evaluator;
        return evaluator.Evaluate(entities, this, ignorePaging);
    }

    public virtual bool IsSatisfiedBy(T entity)
    {
        var validator = Validator;
        return validator.IsValid(entity, this);
    }

    private protected object?[]? _state;

    private void AddItem(object value)
    {
        if (_state is null)
        {
            _state = new object[2];
            _state[0] = value;
        }
        else
        {
            for (int i = 0; i < _state.Length; i++)
            {
                if (_state[i] is null)
                {
                    _state[i] = value;
                    return;
                }
            }

            var originalLength = _state.Length;
            var newArray = new object[originalLength * 2];
            Array.Copy(_state, newArray, _state.Length);
            newArray[originalLength] = value;
            _state = newArray;
        }
    }

    private protected TType? Get<TType>() where TType : class, new()
    {
        for (int i = 0; i < _state?.Length; i++)
        {
            if (_state[i] is TType value)
            {
                return value;
            }
        }

        return null;
    }

    private protected TType GetOrCreate<TType>() where TType : new()
    {
        for (int i = 0; i < _state?.Length; i++)
        {
            if (_state[i] is TType value)
            {
                return value;
            }
        }

        var newValue = new TType();
        AddItem(newValue);
        return newValue;
    }

    internal void Add(WhereExpression<T> whereExpression) => AddItem(whereExpression);
    internal void Add(IncludeExpression includeExpression) => AddItem(includeExpression);
    internal void Add(OrderExpression<T> orderExpression) => AddItem(orderExpression);
    internal void Add(LikeExpression<T> likeExpression) => AddItem(likeExpression);
    internal void Add(string includeString) => AddItem(includeString);

    public IEnumerable<WhereExpression<T>> WhereExpressions => _state?.OfType<WhereExpression<T>>() ?? Enumerable.Empty<WhereExpression<T>>();
    public IEnumerable<IncludeExpression> IncludeExpressions => _state?.OfType<IncludeExpression>() ?? Enumerable.Empty<IncludeExpression>();
    public IEnumerable<OrderExpression<T>> OrderExpressions => _state?.OfType<OrderExpression<T>>() ?? Enumerable.Empty<OrderExpression<T>>();
    public IEnumerable<LikeExpression<T>> LikeExpressions => _state?.OfType<LikeExpression<T>>() ?? Enumerable.Empty<LikeExpression<T>>();
    public IEnumerable<string> IncludeStrings => _state?.OfType<string>() ?? Enumerable.Empty<string>();

    public int Take
    {
        get => Get<SpecFlags>()?.Take ?? -1;
        internal set => GetOrCreate<SpecFlags>().Take = value;
    }
    public int Skip
    {
        get => Get<SpecFlags>()?.Skip ?? -1;
        internal set => GetOrCreate<SpecFlags>().Skip = value;
    }

    public bool IgnoreQueryFilters
    {
        get => Get<SpecFlags>()?.IgnoreQueryFilters ?? false;
        internal set => GetOrCreate<SpecFlags>().IgnoreQueryFilters = value;
    }
    public bool AsSplitQuery
    {
        get => Get<SpecFlags>()?.AsSplitQuery ?? false;
        internal set => GetOrCreate<SpecFlags>().AsSplitQuery = value;
    }
    public bool AsNoTracking
    {
        get => Get<SpecFlags>()?.AsNoTracking ?? false;
        internal set => GetOrCreate<SpecFlags>().AsNoTracking = value;
    }
    public bool AsNoTrackingWithIdentityResolution
    {
        get => Get<SpecFlags>()?.AsNoTrackingWithIdentityResolution ?? false;
        internal set => GetOrCreate<SpecFlags>().AsNoTrackingWithIdentityResolution = value;
    }

    public Dictionary<string, object> Items => GetOrCreate<Dictionary<string, object>>();

    public class SpecFlags
    {
        public int Take = -1;
        public int Skip = -1;
        public bool IgnoreQueryFilters = false;
        public bool AsSplitQuery = false;
        public bool AsNoTracking = false;
        public bool AsNoTrackingWithIdentityResolution = false;
    }
}

public enum StateType
{
    Where,
    Include,
    Order,
    Flags,
    Select,
    Like,
    IncludeString,
    Items,
}
