using System.Buffers;
using System.Diagnostics;

namespace Pozitron.QuerySpecification;

// public IEnumerable<T> Evaluate<T>(IEnumerable<T> source, Specification<T> specification)
// {
//     foreach (var likeGroup in specification.LikeExpressions.GroupBy(x => x.Group))
//     {
//         source = source.Where(x => likeGroup.Any(c => c.KeySelectorFunc(x)?.Like(c.Pattern) ?? false));
//     }
//     return source;
// }
// This was the previous implementation. We're trying to avoid allocations of LikeExpressions, GroupBy and LINQ.
// The new implementation preserves the behavior and reduces allocations drastically.
// We've implemented a custom iterator. Also, instead of GroupBy, we have a single array sorted by group, and we slice it to get the groups.
// For 1000 items, the allocations are reduced from 270.592 bytes to only 72 bytes (the cost of the iterator instance). Refer to LikeInMemoryBenchmark results.

public sealed class LikeMemoryEvaluator : IInMemoryEvaluator
{
    private LikeMemoryEvaluator() { }
    public static LikeMemoryEvaluator Instance = new();

    public IEnumerable<T> Evaluate<T>(IEnumerable<T> source, Specification<T> specification)
    {
        if (specification.IsEmpty) return source;

        var count = GetCount(specification);
        if (count == 0) return source;

        return new SpecLikeIterator<T>(source, specification, count);
    }

    private static int GetCount<T>(Specification<T> specification)
    {
        var count = 0;
        foreach (var state in specification.States)
        {
            if (state.Type == StateType.Like)
                count++;
        }
        return count;
    }

    private sealed class SpecLikeIterator<TSource> : Iterator<TSource>
    {
        private readonly IEnumerable<TSource> _source;
        private readonly Specification<TSource> _specification;
        private readonly int _count;
        private SpecState[]? _states;

        private IEnumerator<TSource>? _enumerator;

        public SpecLikeIterator(IEnumerable<TSource> source, Specification<TSource> specification, int count)
        {
            Debug.Assert(source != null);
            Debug.Assert(specification != null);
            _source = source;
            _specification = specification;
            _count = count;

            _states = ArrayPool<SpecState>.Shared.Rent(count);
            FillSorted(specification, _states.AsSpan().Slice(0, count));
        }

        public override Iterator<TSource> Clone()
            => new SpecLikeIterator<TSource>(_source, _specification, _count);

        public override void Dispose()
        {
            if (_states is not null)
            {
                ArrayPool<SpecState>.Shared.Return(_states);
                _states = null;
            }
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
                    Debug.Assert(_states is not null);
                    var states = _states.AsSpan().Slice(0, _count);
                    while (_enumerator.MoveNext())
                    {
                        TSource item = _enumerator.Current;
                        if (IsValid(item, states))
                        {
                            _current = item;
                            return true;
                        }
                    }

                    Dispose();
                    break;
            }

            return false;
        }

        private static void FillSorted<T>(Specification<T> specification, Span<SpecState> span)
        {
            var i = 0;
            foreach (var state in specification.States)
            {
                if (state.Type == StateType.Like)
                {
                    // Find the correct insertion point
                    var j = i;
                    while (j > 0 && span[j - 1].Bag > state.Bag)
                    {
                        span[j] = span[j - 1];
                        j--;
                    }

                    // Insert the current state in the sorted position
                    span[j] = state;
                    i++;
                }
            }
        }

        private static bool IsValid<T>(T item, Span<SpecState> span)
        {
            var valid = true;
            int start = 0;

            for (int i = 1; i <= span.Length; i++)
            {
                if (i == span.Length || span[i].Bag != span[start].Bag)
                {
                    var validOrGroup = IsValidInOrGroup(item, span[start..i]);
                    if ((valid = valid && validOrGroup) is false)
                    {
                        break;
                    }
                    start = i;
                }
            }

            return valid;

            static bool IsValidInOrGroup(T item, Span<SpecState> span)
            {
                var validOrGroup = false;
                foreach (var state in span)
                {
                    if (state.Reference is not SpecLike<T> specLike) continue;

                    if (specLike.KeySelectorFunc(item)?.Like(specLike.Pattern) ?? false)
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
