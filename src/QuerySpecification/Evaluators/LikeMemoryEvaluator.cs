﻿using System.Diagnostics;

namespace Pozitron.QuerySpecification;

/*
    public IEnumerable<T> Evaluate<T>(IEnumerable<T> source, Specification<T> specification)
    {
        foreach (var likeGroup in specification.LikeExpressions.GroupBy(x => x.Group))
        {
            source = source.Where(x => likeGroup.Any(c => c.KeySelectorFunc(x)?.Like(c.Pattern) ?? false));
        }
        return source;
    }
    This was the previous implementation.We're trying to avoid allocations of LikeExpressions, GroupBy and LINQ.
    The new implementation preserves the behavior and reduces allocations drastically.
    We've implemented a custom iterator. Also, instead of GroupBy, we have a single array sorted by group, and we slice it to get the groups.
    For 1000 items, the allocations are reduced from 257.872 bytes to only 64 bytes (the cost of the iterator instance). Refer to LikeInMemoryEvaluatorBenchmark results.
 */

public sealed class LikeMemoryEvaluator : IInMemoryEvaluator
{
    private LikeMemoryEvaluator() { }
    public static LikeMemoryEvaluator Instance = new();

    public IEnumerable<T> Evaluate<T>(IEnumerable<T> source, Specification<T> specification)
    {
        if (specification.IsEmpty) return source;

        var compiledItems = specification.GetCompiledItems();
        if (compiledItems.Length == 0) return source;

        int startIndexLikeItems = Array.FindIndex(compiledItems, item => item.Type == ItemType.Like);
        if (startIndexLikeItems == -1) return source;

        // The like items are contiguously placed as a last segment in the array and are already sorted by group.
        return new SpecLikeIterator<T>(source, compiledItems, startIndexLikeItems);
    }

    private sealed class SpecLikeIterator<TSource> : Iterator<TSource>
    {
        private readonly IEnumerable<TSource> _source;
        private readonly SpecItem[] _compiledItems;
        private readonly int _startIndex;

        private IEnumerator<TSource>? _enumerator;

        public SpecLikeIterator(IEnumerable<TSource> source, SpecItem[] compiledItems, int startIndex)
        {
            Debug.Assert(source != null);
            Debug.Assert(compiledItems != null);
            _source = source;
            _compiledItems = compiledItems;
            _startIndex = startIndex;
        }

        public override Iterator<TSource> Clone()
            => new SpecLikeIterator<TSource>(_source, _compiledItems, _startIndex);

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
                    var likeItems = _compiledItems.AsSpan()[_startIndex.._compiledItems.Length];
                    while (_enumerator.MoveNext())
                    {
                        TSource sourceItem = _enumerator.Current;
                        if (IsValid(sourceItem, likeItems))
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
            var valid = true;
            int start = 0;

            for (int i = 1; i <= span.Length; i++)
            {
                if (i == span.Length || span[i].Bag != span[start].Bag)
                {
                    var validOrGroup = IsValidInOrGroup(sourceItem, span[start..i]);
                    if ((valid = valid && validOrGroup) is false)
                    {
                        break;
                    }
                    start = i;
                }
            }

            return valid;

            static bool IsValidInOrGroup(T sourceItem, ReadOnlySpan<SpecItem> span)
            {
                var validOrGroup = false;
                foreach (var specItem in span)
                {
                    if (specItem.Reference is not SpecLikeCompiled<T> specLike) continue;

                    if (specLike.KeySelector(sourceItem)?.Like(specLike.Pattern) ?? false)
                    {
                        validOrGroup = true;
                        break;
                    }
                }
                return validOrGroup;
            }
        }
    }
}
