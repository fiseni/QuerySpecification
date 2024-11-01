using System.Diagnostics;

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

        var compiledStates = specification.GetCompiledStates();
        if (compiledStates.Length == 0) return source;

        int startIndexLikeStates = Array.FindIndex(compiledStates, state => state.Type == StateType.Like);
        if (startIndexLikeStates == -1) return source;

        // The like states are contiguous placed as last segment in the array and are already sorted by group.
        return new SpecLikeIterator<T>(source, compiledStates, startIndexLikeStates);
    }

    private sealed class SpecLikeIterator<TSource> : Iterator<TSource>
    {
        private readonly IEnumerable<TSource> _source;
        private readonly SpecState[] _likeStates;
        private readonly int _startIndex;

        private IEnumerator<TSource>? _enumerator;

        public SpecLikeIterator(IEnumerable<TSource> source, SpecState[] compiledStates, int startIndex)
        {
            Debug.Assert(source != null);
            Debug.Assert(compiledStates != null);
            _source = source;
            _likeStates = compiledStates;
            _startIndex = startIndex;
        }

        public override Iterator<TSource> Clone()
            => new SpecLikeIterator<TSource>(_source, _likeStates, _startIndex);

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
                    var likeStates = _likeStates.AsSpan()[_startIndex.._likeStates.Length];
                    while (_enumerator.MoveNext())
                    {
                        TSource item = _enumerator.Current;
                        if (IsValid(item, likeStates))
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

        private static bool IsValid<T>(T item, ReadOnlySpan<SpecState> span)
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

            static bool IsValidInOrGroup(T item, ReadOnlySpan<SpecState> span)
            {
                var validOrGroup = false;
                foreach (var state in span)
                {
                    if (state.Reference is not SpecLikeCompiled<T> specLike) continue;

                    if (specLike.KeySelector(item)?.Like(specLike.Pattern) ?? false)
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
