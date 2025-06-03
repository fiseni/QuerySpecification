using System.Diagnostics.CodeAnalysis;

namespace Pozitron.QuerySpecification;

/// <summary>
/// Represents a specification with a result type.
/// </summary>
/// <typeparam name="T">The type of the entity.</typeparam>
/// <typeparam name="TResult">The type of the result.</typeparam>
public class Specification<T, TResult> : Specification<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Specification{T, TResult}"/> class.
    /// </summary>
    public Specification() : base() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Specification{T, TResult}"/> class with the specified initial capacity.
    /// </summary>
    /// <param name="initialCapacity">The initial capacity of the specification.</param>
    public Specification(int initialCapacity) : base(initialCapacity) { }

    /// <summary>
    /// Evaluates the given entities according to the specification and returns the result.
    /// </summary>
    /// <param name="entities">The entities to evaluate.</param>
    /// <param name="ignorePaging">Whether to ignore paging settings (Take/Skip) defined in the specification.</param>
    /// <returns>The evaluated result.</returns>
    public new virtual IEnumerable<TResult> Evaluate(IEnumerable<T> entities, bool ignorePaging = false)
    {
        var evaluator = Evaluator;
        return evaluator.Evaluate(entities, this, ignorePaging);
    }

    /// <summary>
    /// Gets the specification builder.
    /// </summary>
    public new ISpecificationBuilder<T, TResult> Query => new SpecificationBuilder<T, TResult>(this);

    /// <summary>
    /// Gets the Select expression.
    /// </summary>
    public Expression<Func<T, TResult>>? Selector => FirstOrDefault<Expression<Func<T, TResult>>>(ItemType.Select);

    /// <summary>
    /// Gets the SelectMany expression.
    /// </summary>
    public Expression<Func<T, IEnumerable<TResult>>>? SelectorMany => FirstOrDefault<Expression<Func<T, IEnumerable<TResult>>>>(ItemType.Select);
}

/// <summary>
/// Represents a specification.
/// </summary>
/// <typeparam name="T">The type of the entity.</typeparam>
public partial class Specification<T>
{
    private SpecItem[]? _items;

    // It is utilized only during the building stage for the builder chains. Once the state is built, we don't care about it anymore.
    // The initial value is not important since the value is always initialized by the root of the chain. Therefore, we don't need ThreadLocal (it's more expensive).
    // With this we're saving 8 bytes per include builder, and we don't need an order builder at all (saving 32 bytes per order builder instance).
    [ThreadStatic]
    internal static bool IsChainDiscarded;

    /// <summary>
    /// Initializes a new instance of the <see cref="Specification{T}"/> class.
    /// </summary>
    public Specification() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Specification{T}"/> class with the specified initial capacity.
    /// </summary>
    /// <param name="initialCapacity">The initial capacity of the specification.</param>
    public Specification(int initialCapacity)
    {
        _items = new SpecItem[initialCapacity];
    }

    /// <summary>
    /// Evaluates the given entities according to the specification and returns the result.
    /// </summary>
    /// <param name="entities">The entities to evaluate.</param>
    /// <param name="ignorePaging">Whether to ignore paging settings (Take/Skip) defined in the specification.</param>
    /// <returns>The evaluated result.</returns>
    public virtual IEnumerable<T> Evaluate(IEnumerable<T> entities, bool ignorePaging = false)
    {
        var evaluator = Evaluator;
        return evaluator.Evaluate(entities, this, ignorePaging);
    }

    /// <summary>
    /// Determines whether the specified entity satisfies the specification.
    /// </summary>
    /// <param name="entity">The entity to evaluate.</param>
    /// <returns>true if the entity satisfies the specification; otherwise, false.</returns>
    public virtual bool IsSatisfiedBy(T entity)
    {
        var validator = Validator;
        return validator.IsValid(entity, this);
    }

    /// <summary>
    /// Gets the evaluator.
    /// </summary>
    protected virtual SpecificationInMemoryEvaluator Evaluator => SpecificationInMemoryEvaluator.Default;

    /// <summary>
    /// Gets the validator.
    /// </summary>
    protected virtual SpecificationValidator Validator => SpecificationValidator.Default;

    /// <summary>
    /// Gets the specification builder.
    /// </summary>
    public ISpecificationBuilder<T> Query => new SpecificationBuilder<T>(this);

    /// <summary>
    /// Gets a value indicating whether the specification is empty.
    /// </summary>
    [MemberNotNullWhen(false, nameof(_items))]
    public bool IsEmpty => _items is null;

    /// <summary>
    /// Gets the compiled where expressions.
    /// </summary>
    public IEnumerable<WhereExpressionCompiled<T>> WhereExpressionsCompiled => _items is null
        ? Enumerable.Empty<WhereExpressionCompiled<T>>()
        : new SpecSelectIterator<Func<T, bool>, WhereExpressionCompiled<T>>(GetCompiledItems(), ItemType.Where, (x, bag) => new(x));

    /// <summary>
    /// Gets the compiled order expressions.
    /// </summary>
    public IEnumerable<OrderExpressionCompiled<T>> OrderExpressionsCompiled => _items is null
        ? Enumerable.Empty<OrderExpressionCompiled<T>>()
        : new SpecSelectIterator<Func<T, object?>, OrderExpressionCompiled<T>>(GetCompiledItems(), ItemType.Order, (x, bag) => new(x, (OrderType)bag));

    /// <summary>
    /// Gets the compiled like expressions.
    /// </summary>
    public IEnumerable<LikeExpressionCompiled<T>> LikeExpressionsCompiled => _items is null
        ? Enumerable.Empty<LikeExpressionCompiled<T>>()
        : new SpecSelectIterator<SpecLikeCompiled<T>, LikeExpressionCompiled<T>>(GetCompiledItems(), ItemType.Like, (x, bag) => new(x.KeySelector, x.Pattern, bag));

    /// <summary>
    /// Gets the where expressions.
    /// </summary>
    public IEnumerable<WhereExpression<T>> WhereExpressions => _items is null
        ? Enumerable.Empty<WhereExpression<T>>()
        : new SpecSelectIterator<Expression<Func<T, bool>>, WhereExpression<T>>(_items, ItemType.Where, (x, bag) => new WhereExpression<T>(x));

    /// <summary>
    /// Gets the include expressions.
    /// </summary>
    public IEnumerable<IncludeExpression<T>> IncludeExpressions => _items is null
        ? Enumerable.Empty<IncludeExpression<T>>()
        : new SpecSelectIterator<LambdaExpression, IncludeExpression<T>>(_items, ItemType.Include, (x, bag) => new IncludeExpression<T>(x, (IncludeType)bag));

    /// <summary>
    /// Gets the order expressions.
    /// </summary>
    public IEnumerable<OrderExpression<T>> OrderExpressions => _items is null
        ? Enumerable.Empty<OrderExpression<T>>()
        : new SpecSelectIterator<Expression<Func<T, object?>>, OrderExpression<T>>(_items, ItemType.Order, (x, bag) => new OrderExpression<T>(x, (OrderType)bag));

    /// <summary>
    /// Gets the like expressions.
    /// </summary>
    public IEnumerable<LikeExpression<T>> LikeExpressions => _items is null
        ? Enumerable.Empty<LikeExpression<T>>()
        : new SpecSelectIterator<SpecLike<T>, LikeExpression<T>>(_items, ItemType.Like, (x, bag) => new LikeExpression<T>(x.KeySelector, x.Pattern, bag));

    /// <summary>
    /// Gets the include strings.
    /// </summary>
    public IEnumerable<string> IncludeStrings => _items is null
        ? Enumerable.Empty<string>()
        : new SpecSelectIterator<string, string>(_items, ItemType.IncludeString, (x, bag) => x);

    /// <summary>
    /// Gets the number of items to take.
    /// </summary>
    public int Take => FirstOrDefault<SpecPaging>(ItemType.Paging)?.Take ?? -1;

    /// <summary>
    /// Gets the number of items to skip.
    /// </summary>
    public int Skip => FirstOrDefault<SpecPaging>(ItemType.Paging)?.Skip ?? -1;

    /// <summary>
    /// Gets a value indicating whether IgnoreQueryFilters is applied.
    /// </summary>
    public bool IgnoreQueryFilters => GetFlag(SpecFlags.IgnoreQueryFilters);

    /// <summary>
    /// Gets a value indicating whether IgnoreAutoIncludes is applied.
    /// </summary>
    public bool IgnoreAutoIncludes => GetFlag(SpecFlags.IgnoreAutoIncludes);

    /// <summary>
    /// Gets a value indicating whether AsSplitQuery is applied.
    /// </summary>
    public bool AsSplitQuery => GetFlag(SpecFlags.AsSplitQuery);

    /// <summary>
    /// Gets a value indicating whether AsNoTracking is applied.
    /// </summary>
    public bool AsNoTracking => GetFlag(SpecFlags.AsNoTracking);

    /// <summary>
    /// Gets a value indicating whether AsNoTrackingWithIdentityResolution is applied.
    /// </summary>
    public bool AsNoTrackingWithIdentityResolution => GetFlag(SpecFlags.AsNoTrackingWithIdentityResolution);

    /// <summary>
    /// Gets a value indicating whether AsTracking is applied.
    /// </summary>
    public bool AsTracking => GetFlag(SpecFlags.AsTracking);

    /// <summary>
    /// Adds an item to the specification.
    /// </summary>
    /// <param name="type">The type of the item. It must be a positive number.</param>
    /// <param name="value">The object to be stored in the item.</param>
    /// <exception cref="ArgumentNullException">If value is null</exception>
    /// <exception cref="ArgumentOutOfRangeException">If type is zero or negative.</exception>
    public void Add(int type, object value)
    {
        ArgumentNullException.ThrowIfNull(value);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(type);

        AddInternal(type, value);
    }

    /// <summary>
    /// Adds or updates an item in the specification.
    /// </summary>
    /// <param name="type">The type of the item.</param>
    /// <param name="value">The object to be stored in the item.</param>
    /// <exception cref="ArgumentNullException">Thrown if value is null</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if type is zero or negative.</exception>
    public void AddOrUpdate(int type, object value)
    {
        ArgumentNullException.ThrowIfNull(value);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(type);

        AddOrUpdateInternal(type, value);
    }

    /// <summary>
    /// Gets the first item of the specified type or the default value if no item is found.
    /// </summary>
    /// <typeparam name="TObject">The type of the object stored in the item.</typeparam>
    /// <param name="type">The type of the item.</param>
    /// <returns>The first item of the specified type or the default value if no item is found.</returns>
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

    /// <summary>
    /// Gets the first item of the specified type.
    /// </summary>
    /// <typeparam name="TObject">The type of the object stored in the item.</typeparam>
    /// <param name="type">The type of the item.</param>
    /// <returns>The first item of the specified type.</returns>
    /// <exception cref="InvalidOperationException">Thrown if no matching item is found.</exception>
    public TObject First<TObject>(int type)
    {
        if (IsEmpty) throw new InvalidOperationException("Specification contains no items");

        foreach (var item in _items)
        {
            if (item.Type == type && item.Reference is TObject reference)
            {
                return reference;
            }
        }
        throw new InvalidOperationException("Specification contains no matching item");
    }

    /// <summary>
    /// Gets all items of the specified type.
    /// </summary>
    /// <typeparam name="TObject">The type of the object stored in the item.</typeparam>
    /// <param name="type">The type of the items.</param>
    /// <returns>An enumerable of items of the specified type.</returns>
    public IEnumerable<TObject> OfType<TObject>(int type) => _items is null
        ? Enumerable.Empty<TObject>()
        : new SpecIterator<TObject>(_items, type);

    internal ReadOnlySpan<SpecItem> Items => _items ?? ReadOnlySpan<SpecItem>.Empty;

    [MemberNotNull(nameof(_items))]
    internal void AddInternal(int type, object value, int bag = 0)
    {
        var newItem = new SpecItem
        {
            Type = type,
            Reference = value,
            Bag = bag
        };

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
                for (var i = 0; i < items.Length; i++)
                {
                    if (items[i].Type == ItemType.Paging)
                    {
                        _items[i].Reference = newItem.Reference;
                        return;
                    }
                }
            }

            for (var i = 0; i < items.Length; i++)
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
        for (var i = 0; i < items.Length; i++)
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

    internal TObject GetOrCreate<TObject>(int type) where TObject : new()
    {
        return FirstOrDefault<TObject>(type) ?? Create();
        TObject Create()
        {
            var reference = new TObject();
            AddInternal(type, reference);
            return reference;
        }
    }
    internal bool GetFlag(SpecFlags flag)
    {
        if (IsEmpty) return false;

        foreach (var item in _items)
        {
            if (item.Type == ItemType.Flags)
            {
                return ((SpecFlags)item.Bag & flag) == flag;
            }
        }
        return false;
    }
    internal void AddOrUpdateFlag(SpecFlags flag, bool value)
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
                    ? (SpecFlags)items[i].Bag | flag
                    : (SpecFlags)items[i].Bag & ~flag;

                _items[i].Bag = (int)newValue;
                return;
            }
        }

        if (value)
        {
            AddInternal(ItemType.Flags, null!, (int)flag);
        }
    }
}
