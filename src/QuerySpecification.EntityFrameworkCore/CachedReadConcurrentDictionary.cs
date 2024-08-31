using System.Collections;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Pozitron.QuerySpecification.EntityFrameworkCore;

internal class CachedReadConcurrentDictionary<TKey, TValue> : IDictionary<TKey, TValue> where TKey : notnull
{
    private const int _cacheMissesBeforeCaching = 10;
    private readonly ConcurrentDictionary<TKey, TValue> _dictionary;
    private readonly IEqualityComparer<TKey>? _comparer;

    private int _cacheMissReads;

    private Dictionary<TKey, TValue>? _readCache;

    public CachedReadConcurrentDictionary()
    {
        _dictionary = new ConcurrentDictionary<TKey, TValue>();
    }

    public CachedReadConcurrentDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection)
    {
        _dictionary = new ConcurrentDictionary<TKey, TValue>(collection);
    }

    public CachedReadConcurrentDictionary(IEqualityComparer<TKey> comparer)
    {
        _comparer = comparer;
        _dictionary = new ConcurrentDictionary<TKey, TValue>(comparer);
    }

    public CachedReadConcurrentDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection, IEqualityComparer<TKey> comparer)
    {
        _comparer = comparer;
        _dictionary = new ConcurrentDictionary<TKey, TValue>(collection, comparer);
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => GetReadDictionary().GetEnumerator();

    public void Add(KeyValuePair<TKey, TValue> item)
    {
        ((IDictionary<TKey, TValue>)_dictionary).Add(item);
        InvalidateCache();
    }

    public void Clear()
    {
        _dictionary.Clear();
        InvalidateCache();
    }

    public bool Contains(KeyValuePair<TKey, TValue> item) => GetReadDictionary().Contains(item);

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
        GetReadDictionary().CopyTo(array, arrayIndex);
    }

    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
        var result = ((IDictionary<TKey, TValue>)_dictionary).Remove(item);
        if (result) InvalidateCache();
        return result;
    }

    public int Count => GetReadDictionary().Count;

    public bool IsReadOnly => false;

    public void Add(TKey key, TValue value)
    {
        ((IDictionary<TKey, TValue>)_dictionary).Add(key, value);
        InvalidateCache();
    }

    public TValue GetOrAdd(TKey key, Func<TKey, TValue> valueFactory)
    {
        if (GetReadDictionary().TryGetValue(key, out var value))
        {
            return value;
        }

        value = _dictionary.GetOrAdd(key, valueFactory);
        InvalidateCache();

        return value;
    }

    public bool TryAdd(TKey key, TValue value)
    {
        if (_dictionary.TryAdd(key, value))
        {
            InvalidateCache();
            return true;
        }

        return false;
    }

    public bool ContainsKey(TKey key) => GetReadDictionary().ContainsKey(key);

    public bool Remove(TKey key)
    {
        var result = ((IDictionary<TKey, TValue>)_dictionary).Remove(key);
        if (result) InvalidateCache();
        return result;
    }

    public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value) => GetReadDictionary().TryGetValue(key, out value);

    public TValue this[TKey key]
    {
        get => GetReadDictionary()[key];
        set
        {
            _dictionary[key] = value;
            InvalidateCache();
        }
    }

    public ICollection<TKey> Keys => GetReadDictionary().Keys;

    public ICollection<TValue> Values => GetReadDictionary().Values;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private IDictionary<TKey, TValue> GetReadDictionary() => _readCache ?? GetWithoutCache();

    private IDictionary<TKey, TValue> GetWithoutCache()
    {
        if (Interlocked.Increment(ref _cacheMissReads) < _cacheMissesBeforeCaching)
        {
            return _dictionary;
        }

        _cacheMissReads = 0;
        return _readCache = new Dictionary<TKey, TValue>(_dictionary, _comparer);
    }

    private void InvalidateCache()
    {
        _cacheMissReads = 0;
        _readCache = null;
    }
}
