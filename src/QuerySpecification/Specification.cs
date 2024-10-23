using System.Diagnostics.CodeAnalysis;

namespace Pozitron.QuerySpecification;

public class Specification<T, TResult> : Specification<T>
{
    public new virtual IEnumerable<TResult> Evaluate(IEnumerable<T> entities, bool ignorePaging = false)
    {
        var evaluator = Evaluator;
        return evaluator.Evaluate(entities, this, ignorePaging);
    }

    public new ISpecificationBuilder<T, TResult> Query => new SpecificationBuilder<T, TResult>(this);
    public SelectExpression<T, TResult>? SelectExpression => GetFirstOrDefault<SelectExpression<T, TResult>>();

    internal void Add(Expression<Func<T, TResult>> selector) => GetOrCreate<SelectExpression<T, TResult>>().Selector = selector;
    internal void Add(Expression<Func<T, IEnumerable<TResult>>> selectorMany) => GetOrCreate<SelectExpression<T, TResult>>().SelectorMany = selectorMany;
}

public class Specification<T>
{
    [ThreadStatic]
    internal static bool _isChainDiscarded;

    internal object?[]? _state;

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

    public ISpecificationBuilder<T> Query => new SpecificationBuilder<T>(this);

    protected virtual SpecificationInMemoryEvaluator Evaluator => SpecificationInMemoryEvaluator.Default;
    protected virtual SpecificationValidator Validator => SpecificationValidator.Default;

    public IEnumerable<WhereExpression<T>> WhereExpressions => OfType<WhereExpression<T>>();
    public IEnumerable<IncludeExpression> IncludeExpressions => OfType<IncludeExpression>();
    public IEnumerable<OrderExpression<T>> OrderExpressions => OfType<OrderExpression<T>>();
    public IEnumerable<LikeExpression<T>> LikeExpressions => OfType<LikeExpression<T>>();
    public IEnumerable<string> IncludeStrings => OfType<string>();

    public int Take
    {
        get => GetFirstOrDefault<Paging>()?.Take ?? -1;
        internal set => GetOrCreate<Paging>().Take = value;
    }
    public int Skip
    {
        get => GetFirstOrDefault<Paging>()?.Skip ?? -1;
        internal set => GetOrCreate<Paging>().Skip = value;
    }

    public bool IgnoreQueryFilters
    {
        get => GetFirstOrDefault<Flags>()?.IgnoreQueryFilters ?? false;
        internal set => GetOrCreate<Flags>().IgnoreQueryFilters = value;
    }
    public bool AsSplitQuery
    {
        get => GetFirstOrDefault<Flags>()?.AsSplitQuery ?? false;
        internal set => GetOrCreate<Flags>().AsSplitQuery = value;
    }
    public bool AsNoTracking
    {
        get => GetFirstOrDefault<Flags>()?.AsNoTracking ?? false;
        internal set => GetOrCreate<Flags>().AsNoTracking = value;
    }
    public bool AsNoTrackingWithIdentityResolution
    {
        get => GetFirstOrDefault<Flags>()?.AsNoTrackingWithIdentityResolution ?? false;
        internal set => GetOrCreate<Flags>().AsNoTrackingWithIdentityResolution = value;
    }

    [MemberNotNullWhen(false, nameof(_state))]
    public bool IsEmpty => _state is null;

    public bool Contains<TState>()
    {
        for (int i = 0; i < _state?.Length; i++)
        {
            if (_state[i] is TState)
            {
                return true;
            }
        }

        return false;
    }

    public IEnumerable<TState> OfType<TState>()
    {
        if (_state is null)
        {
            return Enumerable.Empty<TState>();
        }

        return OfTypeIterator<TState>(_state);
    }

    private static IEnumerable<TState> OfTypeIterator<TState>(object?[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] is TState item)
            {
                yield return item;
            }
        }
    }

    public TState? GetFirstOrDefault<TState>()
    {
        for (int i = 0; i < _state?.Length; i++)
        {
            if (_state[i] is TState item)
            {
                return item;
            }
        }

        return default;
    }

    public TState GetOrCreate<TState>() where TState : new()
    {
        return GetFirstOrDefault<TState>() ?? Create();
        TState Create()
        {
            var item = new TState();
            Add(item);
            return item;
        }
    }

    public TState GetOrCreate<TState>(Func<TState> create) where TState : notnull
    {
        return GetFirstOrDefault<TState>() ?? Create();
        TState Create()
        {
            var item = create();
            Add(item);
            return item;
        }
    }

    public void Add(object value)
    {
        if (_state is null)
        {
            // Specs with two items are very common, we'll optimize for that.
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
            var newArray = new object[originalLength + 4];
            Array.Copy(_state, newArray, _state.Length);
            newArray[originalLength] = value;
            _state = newArray;
        }
    }

    public class Paging
    {
        public int Take = -1;
        public int Skip = -1;
    }

    public class Flags
    {
        public bool IgnoreQueryFilters = false;
        public bool AsNoTracking = false;
        public bool AsNoTrackingWithIdentityResolution = false;
        public bool AsSplitQuery = false;
    }
}
