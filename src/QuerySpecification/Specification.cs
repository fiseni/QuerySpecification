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

    public Expression<Func<T, TResult>>? Selector => GetFirstOrDefault<Expression<Func<T, TResult>>>(StateType.Select);
    public Expression<Func<T, IEnumerable<TResult>>>? SelectorMany => GetFirstOrDefault<Expression<Func<T, IEnumerable<TResult>>>>(StateType.Select);
}

public class Specification<T>
{
    private protected SpecState[]? _states;

    // It is utilized only during the building stage for the builder chains. Once the state is built, we don't care about it anymore.
    // We also don't care about the initial value since the value is always initialized in the root chains. Therefore, we don't need ThreadLocal (it's more expensive).
    // With this we're saving 8 bytes per include builder, and we don't need order builder at all (saving 32 bytes per order builder).

    [ThreadStatic]
    internal static bool IsChainDiscarded;

    public Specification() { }
    public Specification(int capacity)
    {
        _states = new SpecState[capacity];
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

    internal ReadOnlySpan<SpecState> States => _states ?? ReadOnlySpan<SpecState>.Empty;

    protected virtual SpecificationInMemoryEvaluator Evaluator => SpecificationInMemoryEvaluator.Default;
    protected virtual SpecificationValidator Validator => SpecificationValidator.Default;

    public ISpecificationBuilder<T> Query => new SpecificationBuilder<T>(this);

    public IEnumerable<WhereExpression<T>> WhereExpressions => _states?
        .Where(x => x.Type == StateType.Where && x.Reference is not null)
        .Select(x => new WhereExpression<T>((Expression<Func<T, bool>>)x.Reference!))
        ?? Enumerable.Empty<WhereExpression<T>>();

    public IEnumerable<IncludeExpression> IncludeExpressions => _states?
        .Where(x => x.Type == StateType.Include && x.Reference is not null)
        .Select(x => new IncludeExpression((LambdaExpression)x.Reference!, (IncludeTypeEnum)x.Bag))
        ?? Enumerable.Empty<IncludeExpression>();

    public IEnumerable<OrderExpression<T>> OrderExpressions => _states?
        .Where(x => x.Type == StateType.Order && x.Reference is not null)
        .Select(x => new OrderExpression<T>((Expression<Func<T, object?>>)x.Reference!, (OrderTypeEnum)x.Bag))
        ?? Enumerable.Empty<OrderExpression<T>>();

    public IEnumerable<LikeExpression<T>> LikeExpressions => _states?
        .Where(x => x.Type == StateType.Like && x.Reference is not null)
        .Select(x => (LikeExpression<T>)x.Reference!)
        ?? Enumerable.Empty<LikeExpression<T>>();

    public IEnumerable<string> IncludeStrings => _states?
        .Where(x => x.Type == StateType.IncludeString && x.Reference is not null)
        .Select(x => (string)x.Reference!)
        ?? Enumerable.Empty<string>();

    public int Take => GetFirstOrDefault<Paging>(StateType.Paging)?.Take ?? -1;
    public int Skip => GetFirstOrDefault<Paging>(StateType.Paging)?.Skip ?? -1;
    public bool IgnoreQueryFilters => GetFlag(SpecFlag.IgnoreQueryFilters);
    public bool AsSplitQuery => GetFlag(SpecFlag.AsSplitQuery);
    public bool AsNoTracking => GetFlag(SpecFlag.AsNoTracking);
    public bool AsNoTrackingWithIdentityResolution => GetFlag(SpecFlag.AsNoTrackingWithIdentityResolution);

    [MemberNotNullWhen(false, nameof(_states))]
    public bool IsEmpty => _states is null;

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
    public IEnumerable<TObject> OfType<TObject>(int type)
    {
        if (IsEmpty) return Enumerable.Empty<TObject>();
        return OfTypeIterator<TObject>(_states, type);
    }
    private static IEnumerable<TObject> OfTypeIterator<TObject>(SpecState[] states, int type)
    {
        foreach (var state in states)
        {
            if (state.Type == type && state.Reference is TObject reference)
            {
                yield return reference;
            }
        }
    }
    public TObject? GetFirstOrDefault<TObject>(int type)
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
    public TObject GetOrCreate<TObject>(int type, Func<TObject> create) where TObject : notnull
    {
        return GetFirstOrDefault<TObject>(type) ?? Create();
        TObject Create()
        {
            var reference = create();
            Add(type, reference);
            return reference;
        }
    }
    public TObject GetOrCreate<TObject>(int type) where TObject : new()
    {
        return GetFirstOrDefault<TObject>(type) ?? Create();
        TObject Create()
        {
            var reference = new TObject();
            Add(type, reference);
            return reference;
        }
    }
    public void Add(int type, object value)
    {
        ArgumentNullException.ThrowIfNull(value);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(type);

        AddInternal(type, value);
    }

    internal TObject GetOrCreateInternal<TObject>(int type) where TObject : new()
    {
        return GetFirstOrDefault<TObject>(type) ?? Create();
        TObject Create()
        {
            var reference = new TObject();
            AddInternal(type, reference);
            return reference;
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
                        _states[i] = newState;
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

    internal bool GetFlag(SpecFlag flag)
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
    internal void SetFlag(SpecFlag flag)
        => SetRemoveFlag(flag, true);
    internal void RemoveFlag(SpecFlag flag)
        => SetRemoveFlag(flag, false);
    private void SetRemoveFlag(SpecFlag flag, bool set = true)
    {
        if (IsEmpty)
        {
            if (set)
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
                var newValue = set
                    ? (SpecFlag)states[i].Bag | flag
                    : (SpecFlag)states[i].Bag & ~flag;

                _states[i].Bag = (int)newValue;
                return;
            }
        }

        if (set)
        {
            AddInternal(StateType.Flags, null!, (int)flag);
        }
    }
}

internal class Paging
{
    public int Take = -1;
    public int Skip = -1;
}

[Flags]
internal enum SpecFlag
{
    IgnoreQueryFilters = 1,
    AsNoTracking = 2,
    AsNoTrackingWithIdentityResolution = 4,
    AsSplitQuery = 8
}

internal struct SpecState
{
    public int Type; // 0-4 bytes
    public int Bag; // 4-8 bytes
    public object? Reference; // 8-16 bytes (on x64)
}

internal static class StateType
{
    public const int Where = -1;
    public const int Order = -2;
    public const int Include = -3;
    public const int IncludeString = -4;
    public const int Like = -5;
    public const int Select = -6;
    public const int SelectMany = -7;

    // We can save 16  bytes (on x64) by storing both Flags and Paging in the same state.
    public const int Paging = -8; // Stored in the reference
    public const int Flags = -8; // Stored in the bag
}
