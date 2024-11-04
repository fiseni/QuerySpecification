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

    public Expression<Func<T, TResult>>? Selector => FirstOrDefault<Expression<Func<T, TResult>>>(ItemType.Select);
    public Expression<Func<T, IEnumerable<TResult>>>? SelectorMany => FirstOrDefault<Expression<Func<T, IEnumerable<TResult>>>>(ItemType.Select);
}

public partial class Specification<T>
{
    private protected SpecItem[]? _items;

    // It is utilized only during the building stage for the builder chains. Once the state is built, we don't care about it anymore.
    // The initial value is not important since the value is always initialized by the root of the chain. Therefore, we don't need ThreadLocal (it's more expensive).
    // With this we're saving 8 bytes per include builder, and we don't need an order builder at all (saving 32 bytes per order builder instance).
    [ThreadStatic]
    internal static bool IsChainDiscarded;

    public Specification() { }
    public Specification(int initialCapacity)
    {
        _items = new SpecItem[initialCapacity];
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

    [MemberNotNullWhen(false, nameof(_items))]
    public bool IsEmpty => _items is null;

    public IEnumerable<WhereExpressionCompiled<T>> WhereExpressionsCompiled => _items is null
        ? Enumerable.Empty<WhereExpressionCompiled<T>>()
        : new SpecSelectIterator<Func<T, bool>, WhereExpressionCompiled<T>>(GetCompiledItems(), ItemType.Where, (x, bag) => new(x));

    public IEnumerable<OrderExpressionCompiled<T>> OrderExpressionsCompiled => _items is null
        ? Enumerable.Empty<OrderExpressionCompiled<T>>()
        : new SpecSelectIterator<Func<T, object?>, OrderExpressionCompiled<T>>(GetCompiledItems(), ItemType.Order, (x, bag) => new(x, (OrderType)bag));

    public IEnumerable<LikeExpressionCompiled<T>> LikeExpressionsCompiled => _items is null
        ? Enumerable.Empty<LikeExpressionCompiled<T>>()
        : new SpecSelectIterator<SpecLikeCompiled<T>, LikeExpressionCompiled<T>>(GetCompiledItems(), ItemType.Like, (x, bag) => new(x.KeySelector, x.Pattern, bag));

    public IEnumerable<WhereExpression<T>> WhereExpressions => _items is null
        ? Enumerable.Empty<WhereExpression<T>>()
        : new SpecSelectIterator<Expression<Func<T, bool>>, WhereExpression<T>>(_items, ItemType.Where, (x, bag) => new WhereExpression<T>(x));

    public IEnumerable<IncludeExpression<T>> IncludeExpressions => _items is null
        ? Enumerable.Empty<IncludeExpression<T>>()
        : new SpecSelectIterator<LambdaExpression, IncludeExpression<T>>(_items, ItemType.Include, (x, bag) => new IncludeExpression<T>(x, (IncludeType)bag));

    public IEnumerable<OrderExpression<T>> OrderExpressions => _items is null
        ? Enumerable.Empty<OrderExpression<T>>()
        : new SpecSelectIterator<Expression<Func<T, object?>>, OrderExpression<T>>(_items, ItemType.Order, (x, bag) => new OrderExpression<T>(x, (OrderType)bag));

    public IEnumerable<LikeExpression<T>> LikeExpressions => _items is null
        ? Enumerable.Empty<LikeExpression<T>>()
        : new SpecSelectIterator<SpecLike<T>, LikeExpression<T>>(_items, ItemType.Like, (x, bag) => new LikeExpression<T>(x.KeySelector, x.Pattern, bag));

    public IEnumerable<string> IncludeStrings => _items is null
        ? Enumerable.Empty<string>()
        : new SpecSelectIterator<string, string>(_items, ItemType.IncludeString, (x, bag) => x);

    public int Take
    {
        get => FirstOrDefault<SpecPaging>(ItemType.Paging)?.Take ?? -1;
        set => GetOrCreate<SpecPaging>(ItemType.Paging).Take = value;
    }
    public int Skip
    {
        get => FirstOrDefault<SpecPaging>(ItemType.Paging)?.Skip ?? -1;
        set => GetOrCreate<SpecPaging>(ItemType.Paging).Skip = value;
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

        foreach (var item in _items)
        {
            if (item.Type == type && item.Reference is TObject reference)
            {
                return reference;
            }
        }
        return default;
    }
    public IEnumerable<TObject> OfType<TObject>(int type) => _items is null
        ? Enumerable.Empty<TObject>()
        : new SpecIterator<TObject>(_items, type);

    internal ReadOnlySpan<SpecItem> Items => _items ?? ReadOnlySpan<SpecItem>.Empty;

    [MemberNotNull(nameof(_items))]
    internal void AddInternal(int type, object value, int bag = 0)
    {
        var newItem = new SpecItem();
        newItem.Type = type;
        newItem.Reference = value;
        newItem.Bag = bag;

        if (IsEmpty)
        {
            // Specs with two items are very common, we'll optimize for that.
            _items = new SpecItem[2];
            _items[0] = newItem;
        }
        else
        {
            var items = _items;

            // We have a special case for Paging, we're storing it in the same item with Flags.
            if (type == ItemType.Paging)
            {
                for (int i = 0; i < items.Length; i++)
                {
                    if (items[i].Type == ItemType.Paging)
                    {
                        _items[i].Reference = newItem.Reference;
                        return;
                    }
                }
            }

            for (int i = 0; i < items.Length; i++)
            {
                if (items[i].Type == 0)
                {
                    items[i] = newItem;
                    return;
                }
            }

            var originalLength = items.Length;
            var newArray = new SpecItem[originalLength + 4];
            Array.Copy(items, newArray, originalLength);
            newArray[originalLength] = newItem;
            _items = newArray;
        }
    }
    internal void AddOrUpdateInternal(int type, object value, int bag = 0)
    {
        if (IsEmpty)
        {
            AddInternal(type, value, bag);
            return;
        }
        var items = _items;
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].Type == type)
            {
                _items[i].Reference = value;
                _items[i].Bag = bag;
                return;
            }
        }
        AddInternal(type, value, bag);
    }
    internal SpecItem[] GetCompiledItems()
    {
        if (IsEmpty) return Array.Empty<SpecItem>();

        var compilableItemsCount = CountCompilableItems(_items);
        if (compilableItemsCount == 0) return Array.Empty<SpecItem>();

        var compiledItems = GetCompiledItems(_items);

        // If the count of compilable items is equal to the count of compiled items, we don't need to recompile.
        if (compiledItems.Length == compilableItemsCount) return compiledItems;

        compiledItems = GenerateCompiledItems(_items, compilableItemsCount);
        AddOrUpdateInternal(ItemType.Compiled, compiledItems);
        return compiledItems;

        static SpecItem[] GetCompiledItems(SpecItem[] items)
        {
            foreach (var item in items)
            {
                if (item.Type == ItemType.Compiled && item.Reference is SpecItem[] compiledItems)
                {
                    return compiledItems;
                }
            }
            return Array.Empty<SpecItem>();
        }

        static int CountCompilableItems(SpecItem[] items)
        {
            var count = 0;
            foreach (var item in items)
            {
                if (item.Type == ItemType.Where || item.Type == ItemType.Like || item.Type == ItemType.Order)
                    count++;
            }
            return count;
        }

        static SpecItem[] GenerateCompiledItems(SpecItem[] items, int count)
        {
            var compiledItems = new SpecItem[count];

            // We want to place the items contiguously by type. Sorting is more expensive than looping per type.
            var index = 0;
            foreach (var item in items)
            {
                if (item.Type == ItemType.Where && item.Reference is Expression<Func<T, bool>> expr)
                {
                    compiledItems[index++] = new SpecItem
                    {
                        Type = item.Type,
                        Reference = expr.Compile(),
                        Bag = item.Bag
                    };
                }
            }
            if (index == count) return compiledItems;

            foreach (var item in items)
            {
                if (item.Type == ItemType.Order && item.Reference is Expression<Func<T, object?>> expr)
                {
                    compiledItems[index++] = new SpecItem
                    {
                        Type = item.Type,
                        Reference = expr.Compile(),
                        Bag = item.Bag
                    };
                }
            }
            if (index == count) return compiledItems;

            var likeStartIndex = index;
            foreach (var item in items)
            {
                if (item.Type == ItemType.Like && item.Reference is SpecLike<T> like)
                {
                    compiledItems[index++] = new SpecItem
                    {
                        Type = item.Type,
                        Reference = new SpecLikeCompiled<T>(like.KeySelector.Compile(), like.Pattern),
                        Bag = item.Bag
                    };
                }
            }

            // Sort Like items by the group, so we do it only once and not repeatedly in the Like evaluator).
            compiledItems.AsSpan()[likeStartIndex..count].Sort((x, y) => x.Bag.CompareTo(y.Bag));

            return compiledItems;
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

        foreach (var item in _items)
        {
            if (item.Type == ItemType.Flags)
            {
                return ((SpecFlag)item.Bag & flag) == flag;
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
                AddInternal(ItemType.Flags, null!, (int)flag);
            }
            return;
        }

        var items = _items;
        for (var i = 0; i < items.Length; i++)
        {
            if (items[i].Type == ItemType.Flags)
            {
                var newValue = value
                    ? (SpecFlag)items[i].Bag | flag
                    : (SpecFlag)items[i].Bag & ~flag;

                _items[i].Bag = (int)newValue;
                return;
            }
        }

        if (value)
        {
            AddInternal(ItemType.Flags, null!, (int)flag);
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
