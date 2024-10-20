﻿namespace Pozitron.QuerySpecification;

public class Specification<T, TResult> : Specification<T>
{
    public new virtual IEnumerable<TResult> Evaluate(IEnumerable<T> entities, bool ignorePaging = false)
    {
        var evaluator = Evaluator;
        return evaluator.Evaluate(entities, this, ignorePaging);
    }

    public new ISpecificationBuilder<T, TResult> Query => new SpecificationBuilder<T, TResult>(this);
    public SelectExpression<T, TResult>? SelectExpression => Get<SelectExpression<T, TResult>>();

    internal void Add(Expression<Func<T, TResult>> selector) => GetOrCreate<SelectExpression<T, TResult>>().Selector = selector;
    internal void Add(Expression<Func<T, IEnumerable<TResult>>> selectorMany) => GetOrCreate<SelectExpression<T, TResult>>().SelectorMany = selectorMany;
}

public class Specification<T>
{
    private protected object?[]? _state;

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

    internal void Add(object value)
    {
        if (_state is null)
        {
            // Specs with two items are very common.
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
            if (_state[i] is TType item)
            {
                return item;
            }
        }

        return null;
    }

    private protected TType GetOrCreate<TType>() where TType : class, new()
    {
        return Get<TType>() ?? Create();
        TType Create()
        {
            var item = new TType();
            Add(item);
            return item;
        }
    }

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
