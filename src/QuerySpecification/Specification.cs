using System.Diagnostics.CodeAnalysis;

namespace Pozitron.QuerySpecification;

public class Specification<T, TResult> : Specification<T>, ISpecificationBuilder<T, TResult>
{
    //public Specification()
    //{
    //    Query = new SpecificationBuilder<T, TResult>(this);
    //}

    //public new ISpecificationBuilder<T, TResult> Query { get; }

    public new Specification<T, TResult> Spec => this;
    public new ISpecificationBuilder<T, TResult> Query => this;

    public Expression<Func<T, TResult>>? Selector
    {
        get => Get(StateType.Select) is Expression<Func<T, TResult>> selector ? selector : null;
        internal set => GetRef(StateType.Select) = value;
    }
    public Expression<Func<T, IEnumerable<TResult>>>? SelectorMany
    {
        get => Get(StateType.Select) is Expression<Func<T, IEnumerable<TResult>>> selector ? selector : null;
        internal set => GetRef(StateType.Select) = value;
    }

    public new virtual IEnumerable<TResult> Evaluate(IEnumerable<T> entities, bool ignorePaging = false)
    {
        var evaluator = Evaluator;
        return evaluator.Evaluate(entities, this, ignorePaging);
    }
}

public class Specification<T> : ISpecificationBuilder<T>
{
    //public Specification()
    //{
    //    Query = new SpecificationBuilder<T>(this);
    //}

    //public ISpecificationBuilder<T> Query { get; }
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


    // Up to here we're spending 48 bytes. It's 56 with the state reference

    private object?[]? _state;

    [MemberNotNull(nameof(_state))]
    private protected ref object? GetRef(StateType stateType)
    {
        var index = (int)stateType;
        var capacity = index switch
        {
            0 => 1,
            < 2 => 2, // Spec with Where and Include are most common
            < 5 => 5,
            _ => 8
        };

        if (_state is null)
        {
            _state = new object[capacity];
        }
        else if (_state.Length < capacity)
        {
            var newItems = new object[capacity];
            Array.Copy(_state, newItems, _state.Length);
            _state = newItems;
        }
        return ref _state[index];
    }

    private protected object? Get(StateType stateType)
    {
        var index = (int)stateType;
        return _state?.Length > index ? _state[index] : null;
    }

    private void AddToArray<TType>(StateType stateType, TType value)
    {
        ref var item = ref GetRef(stateType);
        if (item is TType[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] is null)
                {
                    array[i] = value;
                    return;
                }
            }

            var originalLength = array.Length;
            var newArray = new TType[originalLength * 2];
            Array.Copy(array, newArray, originalLength);
            newArray[originalLength] = value;
            item = newArray;
        }
        else
        {
            var newArray = new TType[2];
            newArray[0] = value;
            item = newArray;
        }
    }

    internal void Add(WhereExpression<T> whereExpression) => AddToArray(StateType.Where, whereExpression);
    internal void Add(IncludeExpression includeExpression) => AddToArray(StateType.Include, includeExpression);
    internal void Add(OrderExpression<T> orderExpression) => AddToArray(StateType.Order, orderExpression);
    internal void Add(LikeExpression<T> likeExpression) => AddToArray(StateType.Like, likeExpression);
    internal void Add(string includeString) => AddToArray(StateType.IncludeString, includeString);

    public IEnumerable<WhereExpression<T>> WhereExpressions => Get(StateType.Where) is WhereExpression<T>[] array ? array.Where(x => x is not null) : Enumerable.Empty<WhereExpression<T>>();
    public IEnumerable<IncludeExpression> IncludeExpressions => Get(StateType.Include) is IncludeExpression[] array ? array.Where(x => x is not null) : Enumerable.Empty<IncludeExpression>();
    public IEnumerable<OrderExpression<T>> OrderExpressions => Get(StateType.Order) is OrderExpression<T>[] array ? array.Where(x => x is not null) : Enumerable.Empty<OrderExpression<T>>();
    public IEnumerable<LikeExpression<T>> LikeExpressions => Get(StateType.Like) is LikeExpression<T>[] array ? array.Where(x => x is not null) : Enumerable.Empty<LikeExpression<T>>();
    public IEnumerable<string> IncludeStrings => Get(StateType.IncludeString) is string[] array ? array.Where(x => x is not null) : Enumerable.Empty<string>();

    //private void AddToList<TType>(StateType stateType, TType value)
    //    => ((List<TType>)(GetRef(stateType) ??= new List<TType>(2))).Add(value);

    //internal void Add(WhereExpression<T> whereExpression) => AddToList(StateType.Where, whereExpression);
    //internal void Add(IncludeExpression includeExpression) => AddToList(StateType.Include, includeExpression);
    //internal void Add(OrderExpression<T> orderExpression) => AddToList(StateType.Order, orderExpression);
    //internal void Add(LikeExpression<T> likeExpression) => AddToList(StateType.Like, likeExpression);
    //internal void Add(string includeString) => AddToList(StateType.IncludeString, includeString);

    //public IEnumerable<WhereExpression<T>> WhereExpressions => Get(StateType.Where) is List<WhereExpression<T>> list ? list : Enumerable.Empty<WhereExpression<T>>();
    //public IEnumerable<IncludeExpression> IncludeExpressions => Get(StateType.Include) is List<IncludeExpression> list ? list : Enumerable.Empty<IncludeExpression>();
    //public IEnumerable<OrderExpression<T>> OrderExpressions => Get(StateType.Order) is List<OrderExpression<T>> list ? list : Enumerable.Empty<OrderExpression<T>>();
    //public IEnumerable<LikeExpression<T>> LikeExpressions => Get(StateType.Like) is List<LikeExpression<T>> list ? list : Enumerable.Empty<LikeExpression<T>>();
    //public IEnumerable<string> IncludeStrings => Get(StateType.IncludeString) is List<string> list ? list : Enumerable.Empty<string>();

    public int Take
    {
        get => Get(StateType.Flags) is SpecFlags flags ? flags.Take : -1;
        internal set => ((SpecFlags)(GetRef(StateType.Flags) ??= new SpecFlags())).Take = value;
    }
    public int Skip
    {
        get => Get(StateType.Flags) is SpecFlags flags ? flags.Skip : -1;
        internal set => ((SpecFlags)(GetRef(StateType.Flags) ??= new SpecFlags())).Skip = value;
    }

    public bool IgnoreQueryFilters
    {
        get => Get(StateType.Flags) is SpecFlags flags && flags.IgnoreQueryFilters;
        internal set => ((SpecFlags)(GetRef(StateType.Flags) ??= new SpecFlags())).IgnoreQueryFilters = value;
    }
    public bool AsSplitQuery
    {
        get => Get(StateType.Flags) is SpecFlags flags && flags.AsSplitQuery;
        internal set => ((SpecFlags)(GetRef(StateType.Flags) ??= new SpecFlags())).AsSplitQuery = value;
    }
    public bool AsNoTracking
    {
        get => Get(StateType.Flags) is SpecFlags flags && flags.AsNoTracking;
        internal set => ((SpecFlags)(GetRef(StateType.Flags) ??= new SpecFlags())).AsNoTracking = value;
    }
    public bool AsNoTrackingWithIdentityResolution
    {
        get => Get(StateType.Flags) is SpecFlags flags && flags.AsNoTrackingWithIdentityResolution;
        internal set => ((SpecFlags)(GetRef(StateType.Flags) ??= new SpecFlags())).AsNoTrackingWithIdentityResolution = value;
    }

    public Dictionary<string, object> Items => (Dictionary<string, object>)(GetRef(StateType.Items) ??= new Dictionary<string, object>());

    private class SpecFlags
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
