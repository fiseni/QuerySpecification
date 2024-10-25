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
    public Expression<Func<T, TResult>>? Selector { get; internal set; }
    public Expression<Func<T, IEnumerable<TResult>>>? SelectorMany { get; internal set; }
}

public class Specification<T>
{
    internal SpecState[]? _state;

    public Specification()
    {
    }
    public Specification(int capacity)
    {
        _state = new SpecState[capacity];
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

    public ISpecificationBuilder<T> Query => new SpecificationBuilder<T>(this);

    protected virtual SpecificationInMemoryEvaluator Evaluator => SpecificationInMemoryEvaluator.Default;
    protected virtual SpecificationValidator Validator => SpecificationValidator.Default;

    public IEnumerable<WhereExpression<T>> WhereExpressions => _state?
        .Where(x => x.Type == StateType.Where && x.Reference is not null)
        .Select(x => new WhereExpression<T>((Expression<Func<T, bool>>)x.Reference!))
        ?? Enumerable.Empty<WhereExpression<T>>();

    public IEnumerable<IncludeExpression> IncludeExpressions => _state?
        .Where(x => x.Type == StateType.Include && x.Reference is not null)
        .Select(x => new IncludeExpression((LambdaExpression)x.Reference!, (IncludeTypeEnum)x.Bag))
        ?? Enumerable.Empty<IncludeExpression>();

    public IEnumerable<OrderExpression<T>> OrderExpressions => _state?
        .Where(x => x.Type == StateType.Order && x.Reference is not null)
        .Select(x => new OrderExpression<T>((Expression<Func<T, object?>>)x.Reference!, (OrderTypeEnum)x.Bag))
        ?? Enumerable.Empty<OrderExpression<T>>();

    public IEnumerable<LikeExpression<T>> LikeExpressions => _state?
        .Where(x => x.Type == StateType.Like && x.Reference is not null)
        .Select(x => (LikeExpression<T>)x.Reference!)
        ?? Enumerable.Empty<LikeExpression<T>>();

    public IEnumerable<string> IncludeStrings => _state?
        .Where(x => x.Type == StateType.IncludeString && x.Reference is not null)
        .Select(x => (string)x.Reference!)
        ?? Enumerable.Empty<string>();

    public int Take
    {
        get => GetFirstOrDefault<Paging>(StateType.Paging)?.Take ?? -1;
        internal set => GetOrCreate<Paging>(StateType.Paging).Take = value;
    }
    public int Skip
    {
        get => GetFirstOrDefault<Paging>(StateType.Paging)?.Skip ?? -1;
        internal set => GetOrCreate<Paging>(StateType.Paging).Skip = value;
    }

    public bool IgnoreQueryFilters => GetFlag(SpecFlag.IgnoreQueryFilters);
    public bool AsSplitQuery => GetFlag(SpecFlag.AsSplitQuery);
    public bool AsNoTracking => GetFlag(SpecFlag.AsNoTracking);
    public bool AsNoTrackingWithIdentityResolution => GetFlag(SpecFlag.AsNoTrackingWithIdentityResolution);

    [MemberNotNullWhen(false, nameof(_state))]
    public bool IsEmpty => _state is null;

    public bool Contains(int type)
    {
        if (_state is null) return false;

        for (int i = 0; i < _state.Length; i++)
        {
            if (_state[i].Type == type)
            {
                return true;
            }
        }

        return false;
    }

    public IEnumerable<TState> OfType<TState>(int type)
    {
        if (IsEmpty)
        {
            return Enumerable.Empty<TState>();
        }

        return OfTypeIterator<TState>(_state, type);
    }

    private static IEnumerable<TState> OfTypeIterator<TState>(SpecState[] array, int type)
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i].Type == type)
            {
                var output = array[i].Reference;
                if (output is not null)
                {
                    yield return (TState)output;
                }
            }
        }
    }

    public TState? GetFirstOrDefault<TState>(int type)
    {
        if (IsEmpty) return default;

        for (int i = 0; i < _state.Length; i++)
        {
            if (_state[i].Type == type)
            {
                return (TState?)_state[i].Reference;
            }
        }

        return default;
    }

    public TState GetOrCreate<TState>(int type) where TState : new()
    {
        return GetFirstOrDefault<TState>(type) ?? Create();
        TState Create()
        {
            var state = new SpecState();
            state.Type = type;
            state.Reference = new TState();
            Add(state);
            return (TState)state.Reference;
        }
    }

    public TState GetOrCreate<TState>(int type, Func<TState> create) where TState : notnull
    {
        return GetFirstOrDefault<TState>(type) ?? Create();
        TState Create()
        {
            var state = new SpecState();
            state.Type = type;
            state.Reference = create();
            Add(state);
            return (TState)state.Reference;
        }
    }

    public void Add(int type, object value)
    {
        ArgumentNullException.ThrowIfNull(value);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(type);

        var state = new SpecState();
        state.Type = type;
        state.Reference = value;
        Add(state);
    }

    internal void Add(SpecState state)
    {
        if (IsEmpty)
        {
            // Specs with two items are very common, we'll optimize for that.
            _state = new SpecState[2];
            _state[0] = state;
        }
        else
        {
            for (int i = 0; i < _state.Length; i++)
            {
                if (_state[i].Type == 0)
                {
                    _state[i] = state;
                    return;
                }
            }

            var originalLength = _state.Length;
            var newArray = new SpecState[originalLength + 4];
            Array.Copy(_state, newArray, _state.Length);
            newArray[originalLength] = state;
            _state = newArray;
        }
    }

    internal void SetEfFlag(SpecFlag flag)
        => ModifyEfFlag(flag, true);

    internal void RemoveEfFlag(SpecFlag flag)
        => ModifyEfFlag(flag, false);

    private void ModifyEfFlag(SpecFlag flag, bool set = true)
    {
        if (IsEmpty)
        {
            Create();
            return;
        }

        for (int i = 0; i < _state.Length; i++)
        {
            if (_state[i].Type == StateType.Flags)
            {
                var newValue = set
                    ? (SpecFlag)_state[i].Bag | flag
                    : (SpecFlag)_state[i].Bag & ~flag;

                _state[i].Bag = (int)newValue;
                return;
            }
        }

        Create();
        void Create()
        {
            if (set)
            {
                var state = new SpecState();
                state.Type = StateType.Flags;
                state.Bag = (int)flag;
                Add(state);
            }
        }
    }

    internal SpecFlag? GetFlag()
    {
        if (IsEmpty) return null;

        for (int i = 0; i < _state.Length; i++)
        {
            var state = _state[i];
            if (state.Type == StateType.Flags)
            {
                return (SpecFlag)state.Bag;
            }
        }

        return null;
    }

    internal bool GetFlag(SpecFlag flag)
    {
        var efFlag = GetFlag();

        if (efFlag is null) return false;

        return (efFlag & flag) == flag;
    }
}

internal class Paging
{
    public int Take = -1;
    public int Skip = -1;
}

internal class Flags
{
    public bool IgnoreQueryFilters = false;
    public bool AsNoTracking = false;
    public bool AsNoTrackingWithIdentityResolution = false;
    public bool AsSplitQuery = false;
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
    // 0-4 bytes (on x64)
    public int Type;

    // 4-8 bytes
    public int Bag;

    // 8-16 bytes
    public object? Reference;
}

internal static class StateType
{
    public const int Where = -1;
    public const int Order = -2;
    public const int Include = -3;
    public const int IncludeString = -4;
    public const int Like = -5;
    public const int Select = -6;
    public const int Paging = -7;
    public const int Flags = -8;
}
