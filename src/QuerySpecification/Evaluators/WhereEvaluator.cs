using System.Diagnostics;

namespace Pozitron.QuerySpecification;

/// <summary>
/// Represents an evaluator for where expressions.
/// </summary>
[EvaluatorDiscovery(Order = 10)]
public sealed class WhereEvaluator : IEvaluator, IMemoryEvaluator, ISpecItemKind
{
    /// <summary>
    /// Gets the singleton instance of the <see cref="WhereEvaluator"/> class.
    /// </summary>
    public static readonly WhereEvaluator Instance = new();
    private WhereEvaluator() { }

    int ISpecItemKind.ItemType => Pozitron.QuerySpecification.ItemType.Where;

    /// <inheritdoc/>
    public IQueryable<T> Evaluate<T>(IQueryable<T> source, Specification<T> specification) where T : class
    {
        foreach (var item in specification.Items)
        {
            if (item.Type == ItemType.Where && item.Reference is Expression<Func<T, bool>> expr)
            {
                source = source.Where(expr);
            }
        }

        return source;
    }

    /// <inheritdoc/>
    public IEnumerable<T> Evaluate<T>(IEnumerable<T> source, Specification<T> specification)
    {
        var compiledItems = specification.GetCompiledItems();

        // The compiled Where items are placed contiguously at the start of the array.
        var whereCount = 0;
        while (whereCount < compiledItems.Length && compiledItems[whereCount].Type == ItemType.Where)
        {
            whereCount++;
        }
        if (whereCount == 0) return source;

        // A single iterator applies all predicates in one pass, instead of chaining one Where per predicate.
        return new SpecWhereIterator<T>(source, compiledItems, whereCount);
    }

    private sealed class SpecWhereIterator<TSource> : Iterator<TSource>
    {
        private readonly IEnumerable<TSource> _source;
        private readonly SpecItem[] _compiledItems;
        private readonly int _whereCount;

        private IEnumerator<TSource>? _enumerator;

        public SpecWhereIterator(IEnumerable<TSource> source, SpecItem[] compiledItems, int whereCount)
        {
            Debug.Assert(source != null);
            Debug.Assert(compiledItems != null && compiledItems.Length >= whereCount);
            _source = source;
            _compiledItems = compiledItems;
            _whereCount = whereCount;
        }

        public override Iterator<TSource> Clone()
            => new SpecWhereIterator<TSource>(_source, _compiledItems, _whereCount);

        public override void Dispose()
        {
            if (_enumerator is not null)
            {
                _enumerator.Dispose();
                _enumerator = null;
            }
            base.Dispose();
        }

        public override bool MoveNext()
        {
            switch (_state)
            {
                case 1:
                    _enumerator = _source.GetEnumerator();
                    _state = 2;
                    goto case 2;
                case 2:
                    Debug.Assert(_enumerator is not null);
                    var whereItems = _compiledItems.AsSpan()[.._whereCount];
                    while (_enumerator.MoveNext())
                    {
                        TSource sourceItem = _enumerator.Current;
                        if (IsValid(sourceItem, whereItems))
                        {
                            _current = sourceItem;
                            return true;
                        }
                    }

                    Dispose();
                    break;
            }

            return false;
        }

        private static bool IsValid<T>(T sourceItem, ReadOnlySpan<SpecItem> span)
        {
            foreach (var specItem in span)
            {
                if (specItem.Reference is not Func<T, bool> predicate) continue;

                if (!predicate(sourceItem))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
