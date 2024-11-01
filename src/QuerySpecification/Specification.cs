using System.Diagnostics.CodeAnalysis;

namespace Pozitron.QuerySpecification;

public class Specification<T, TResult> : Specification<T>
{
    public Specification() : base() { }
    public Specification(int initialCapacity) : base(initialCapacity) { }

    public new virtual IEnumerable<TResult> Evaluate(IEnumerable<T> entities, bool ignorePaging = false)
    {
        var evaluator = Evaluator;
        return evaluator.Evaluate(entities, this, ignorePaging);
    }

    public new ISpecificationBuilder<T, TResult> Query => new SpecificationBuilder<T, TResult>(this);

    public Expression<Func<T, TResult>>? Selector => FirstOrDefault<Expression<Func<T, TResult>>>(StateType.Select);
    public Expression<Func<T, IEnumerable<TResult>>>? SelectorMany => FirstOrDefault<Expression<Func<T, IEnumerable<TResult>>>>(StateType.Select);
}

public partial class Specification<T>
{
    private protected SpecState[]? _states;

    // It is utilized only during the building stage for the builder chains. Once the state is built, we don't care about it anymore.
    // The initial value is not important since the value is always initialized by the root of the chain. Therefore, we don't need ThreadLocal (it's more expensive).
    // With this we're saving 8 bytes per include builder, and we don't need an order builder at all (saving 32 bytes per order builder instance).
    [ThreadStatic]
    internal static bool IsChainDiscarded;

    public Specification() { }
    public Specification(int initialCapacity)
    {
        _states = new SpecState[initialCapacity];
    }

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

    protected virtual SpecificationInMemoryEvaluator Evaluator => SpecificationInMemoryEvaluator.Default;
    protected virtual SpecificationValidator Validator => SpecificationValidator.Default;

    public ISpecificationBuilder<T> Query => new SpecificationBuilder<T>(this);

    [MemberNotNullWhen(false, nameof(_states))]
    public bool IsEmpty => _states is null;

    public IEnumerable<WhereExpressionCompiled<T>> WhereExpressionsCompiled => _states is null
        ? Enumerable.Empty<WhereExpressionCompiled<T>>()
        : new SpecSelectIterator<Func<T, bool>, WhereExpressionCompiled<T>>(GetCompiledStates(), StateType.Where, (x, bag) => new(x));

    public IEnumerable<OrderExpressionCompiled<T>> OrderExpressionsCompiled => _states is null
        ? Enumerable.Empty<OrderExpressionCompiled<T>>()
        : new SpecSelectIterator<Func<T, object?>, OrderExpressionCompiled<T>>(GetCompiledStates(), StateType.Order, (x, bag) => new(x, (OrderType)bag));

    public IEnumerable<LikeExpressionCompiled<T>> LikeExpressionsCompiled => _states is null
        ? Enumerable.Empty<LikeExpressionCompiled<T>>()
        : new SpecSelectIterator<SpecLikeCompiled<T>, LikeExpressionCompiled<T>>(GetCompiledStates(), StateType.Like, (x, bag) => new(x.KeySelector, x.Pattern, bag));

    public IEnumerable<WhereExpression<T>> WhereExpressions => _states is null
        ? Enumerable.Empty<WhereExpression<T>>()
        : new SpecSelectIterator<Expression<Func<T, bool>>, WhereExpression<T>>(_states, StateType.Where, (x, bag) => new WhereExpression<T>(x));

    public IEnumerable<IncludeExpression<T>> IncludeExpressions => _states is null
        ? Enumerable.Empty<IncludeExpression<T>>()
        : new SpecSelectIterator<LambdaExpression, IncludeExpression<T>>(_states, StateType.Include, (x, bag) => new IncludeExpression<T>(x, (IncludeType)bag));

    public IEnumerable<OrderExpression<T>> OrderExpressions => _states is null
        ? Enumerable.Empty<OrderExpression<T>>()
        : new SpecSelectIterator<Expression<Func<T, object?>>, OrderExpression<T>>(_states, StateType.Order, (x, bag) => new OrderExpression<T>(x, (OrderType)bag));

    public IEnumerable<LikeExpression<T>> LikeExpressions => _states is null
        ? Enumerable.Empty<LikeExpression<T>>()
        : new SpecSelectIterator<SpecLike<T>, LikeExpression<T>>(_states, StateType.Like, (x, bag) => new LikeExpression<T>(x.KeySelector, x.Pattern, bag));

    public IEnumerable<string> IncludeStrings => _states is null
        ? Enumerable.Empty<string>()
        : new SpecSelectIterator<string, string>(_states, StateType.IncludeString, (x, bag) => x);

    public int Take
    {
        get => FirstOrDefault<SpecPaging>(StateType.Paging)?.Take ?? -1;
        set => GetOrCreate<SpecPaging>(StateType.Paging).Take = value;
    }
    public int Skip
    {
        get => FirstOrDefault<SpecPaging>(StateType.Paging)?.Skip ?? -1;
        set => GetOrCreate<SpecPaging>(StateType.Paging).Skip = value;
    }
    public bool IgnoreQueryFilters
    {
        get => GetFlag(SpecFlag.IgnoreQueryFilters);
        set => UpdateFlag(SpecFlag.IgnoreQueryFilters, value);
    }
    public bool AsSplitQuery
    {
        get => GetFlag(SpecFlag.AsSplitQuery);
        set => UpdateFlag(SpecFlag.AsSplitQuery, value);
    }
    public bool AsNoTracking
    {
        get => GetFlag(SpecFlag.AsNoTracking);
        set => UpdateFlag(SpecFlag.AsNoTracking, value);
    }
    public bool AsNoTrackingWithIdentityResolution
    {
        get => GetFlag(SpecFlag.AsNoTrackingWithIdentityResolution);
        set => UpdateFlag(SpecFlag.AsNoTrackingWithIdentityResolution, value);
    }

    public void Add(int type, object value)
    {
        ArgumentNullException.ThrowIfNull(value);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(type);

        AddInternal(type, value);
    }
    public TObject? FirstOrDefault<TObject>(int type)
    {
        if (IsEmpty) return default;

        foreach (var state in _states)
        {
            if (state.Type == type && state.Reference is TObject reference)
            {
                return reference;
            }
        }
        return default;
    }
    public IEnumerable<TObject> OfType<TObject>(int type) => _states is null
        ? Enumerable.Empty<TObject>()
        : new SpecIterator<TObject>(_states, type);

    internal ReadOnlySpan<SpecState> States => _states ?? ReadOnlySpan<SpecState>.Empty;

    [MemberNotNull(nameof(_states))]
    internal void AddInternal(int type, object value, int bag = 0)
    {
        var newState = new SpecState();
        newState.Type = type;
        newState.Reference = value;
        newState.Bag = bag;

        if (IsEmpty)
        {
            // Specs with two items are very common, we'll optimize for that.
            _states = new SpecState[2];
            _states[0] = newState;
        }
        else
        {
            var states = _states;

            // We have a special case for Paging, we're storing it in the same state with Flags.
            if (type == StateType.Paging)
            {
                for (int i = 0; i < states.Length; i++)
                {
                    if (states[i].Type == StateType.Paging)
                    {
                        _states[i].Reference = newState.Reference;
                        return;
                    }
                }
            }

            for (int i = 0; i < states.Length; i++)
            {
                if (states[i].Type == 0)
                {
                    states[i] = newState;
                    return;
                }
            }

            var originalLength = states.Length;
            var newArray = new SpecState[originalLength + 4];
            Array.Copy(states, newArray, originalLength);
            newArray[originalLength] = newState;
            _states = newArray;
        }
    }
    internal void AddOrUpdateInternal(int type, object value, int bag = 0)
    {
        if (IsEmpty)
        {
            AddInternal(type, value, bag);
            return;
        }
        var states = _states;
        for (int i = 0; i < states.Length; i++)
        {
            if (states[i].Type == type)
            {
                _states[i].Reference = value;
                _states[i].Bag = bag;
                return;
            }
        }
        AddInternal(type, value, bag);
    }
    internal SpecState[] GetCompiledStates()
    {
        if (IsEmpty) return Array.Empty<SpecState>();

        var compilableStatesCount = CountCompilableStates(_states);
        if (compilableStatesCount == 0) return Array.Empty<SpecState>();

        var compiledStates = GetCompiledStates(_states);

        // If the count of compilable states is equal to the count of compiled states, we don't need to recompile.
        if (compiledStates.Length == compilableStatesCount) return compiledStates;

        compiledStates = GenerateCompiledStates(_states, compilableStatesCount);
        AddOrUpdateInternal(StateType.Compiled, compiledStates);
        return compiledStates;

        static SpecState[] GetCompiledStates(SpecState[] states)
        {
            foreach (var state in states)
            {
                if (state.Type == StateType.Compiled && state.Reference is SpecState[] compiledStates)
                {
                    return compiledStates;
                }
            }
            return Array.Empty<SpecState>();
        }

        static int CountCompilableStates(SpecState[] states)
        {
            var count = 0;
            foreach (var item in states)
            {
                if (item.Type == StateType.Where || item.Type == StateType.Like || item.Type == StateType.Order)
                    count++;
            }
            return count;
        }

        static SpecState[] GenerateCompiledStates(SpecState[] states, int count)
        {
            var compiledStates = new SpecState[count];

            // We want to place the states contiguously by type. Sorting is more expensive than looping per type.
            var index = 0;
            foreach (var item in states)
            {
                if (item.Type == StateType.Where && item.Reference is Expression<Func<T, bool>> expr)
                {
                    compiledStates[index++] = new SpecState
                    {
                        Type = item.Type,
                        Reference = expr.Compile(),
                        Bag = item.Bag
                    };
                }
            }
            if (index == count) return compiledStates;

            foreach (var item in states)
            {
                if (item.Type == StateType.Order && item.Reference is Expression<Func<T, object?>> expr)
                {
                    compiledStates[index++] = new SpecState
                    {
                        Type = item.Type,
                        Reference = expr.Compile(),
                        Bag = item.Bag
                    };
                }
            }
            if (index == count) return compiledStates;

            var likeStartIndex = index;
            foreach (var item in states)
            {
                if (item.Type == StateType.Like && item.Reference is SpecLike<T> like)
                {
                    compiledStates[index++] = new SpecState
                    {
                        Type = item.Type,
                        Reference = new SpecLikeCompiled<T>(like.KeySelector.Compile(), like.Pattern),
                        Bag = item.Bag
                    };
                }
            }

            // Sort Like states by the group, so we do it only once and not repeatedly in the Like evaluator).
            compiledStates.AsSpan()[likeStartIndex..count].Sort((x, y) => x.Bag.CompareTo(y.Bag));

            return compiledStates;
        }
    }

    private TObject GetOrCreate<TObject>(int type) where TObject : new()
    {
        return FirstOrDefault<TObject>(type) ?? Create();
        TObject Create()
        {
            var reference = new TObject();
            AddInternal(type, reference);
            return reference;
        }
    }
    private bool GetFlag(SpecFlag flag)
    {
        if (IsEmpty) return false;

        foreach (var state in _states)
        {
            if (state.Type == StateType.Flags)
            {
                return ((SpecFlag)state.Bag & flag) == flag;
            }
        }
        return false;
    }
    private void UpdateFlag(SpecFlag flag, bool value)
    {
        if (IsEmpty)
        {
            if (value)
            {
                AddInternal(StateType.Flags, null!, (int)flag);
            }
            return;
        }

        var states = _states;
        for (var i = 0; i < states.Length; i++)
        {
            if (states[i].Type == StateType.Flags)
            {
                var newValue = value
                    ? (SpecFlag)states[i].Bag | flag
                    : (SpecFlag)states[i].Bag & ~flag;

                _states[i].Bag = (int)newValue;
                return;
            }
        }

        if (value)
        {
            AddInternal(StateType.Flags, null!, (int)flag);
        }
    }

    [Flags]
    private enum SpecFlag
    {
        IgnoreQueryFilters = 1,
        AsNoTracking = 2,
        AsNoTrackingWithIdentityResolution = 4,
        AsSplitQuery = 8
    }
}
