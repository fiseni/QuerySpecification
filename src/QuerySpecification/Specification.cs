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

    public Expression<Func<T, TResult>>? Selector => FirstOrDefault<Expression<Func<T, TResult>>>(StateType.Select);
    public Expression<Func<T, IEnumerable<TResult>>>? SelectorMany => FirstOrDefault<Expression<Func<T, IEnumerable<TResult>>>>(StateType.Select);
}

public partial class Specification<T>
{
    private protected SpecState[]? _states;

    // It is utilized only during the building stage for the builder chains. Once the state is built, we don't care about it anymore.
    // We also don't care about the initial value since the value is always initialized in the root chains. Therefore, we don't need ThreadLocal (it's more expensive).
    // With this we're saving 8 bytes per include builder, and we don't need order builder at all (saving 32 bytes per order builder).
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
        : new SpecSelectIterator<LikeExpression<T>, LikeExpression<T>>(_states, StateType.Like, (x, bag) => x);

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

    public bool Contains(int type)
    {
        if (IsEmpty) return false;

        foreach (var state in _states)
        {
            if (state.Type == type)
            {
                return true;
            }
        }
        return false;
    }

    public IEnumerable<TObject> OfType<TObject>(int type) => _states is null
        ? Enumerable.Empty<TObject>()
        : new SpecIterator<TObject>(_states, type);

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

    public void Add(int type, object value)
    {
        ArgumentNullException.ThrowIfNull(value);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(type);

        AddInternal(type, value);
    }

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
                    _states[i] = newState;
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
