namespace Pozitron.QuerySpecification;

public sealed class LikeMemoryEvaluator : IInMemoryEvaluator
{
    private LikeMemoryEvaluator() { }
    public static LikeMemoryEvaluator Instance = new();

    // public IEnumerable<T> Evaluate<T>(IEnumerable<T> source, Specification<T> specification)
    // {
    //     foreach (var likeGroup in specification.LikeExpressions.GroupBy(x => x.Group))
    //     {
    //         source = source.Where(x => likeGroup.Any(c => c.KeySelectorFunc(x)?.Like(c.Pattern) ?? false));
    //     }
    //     return source;
    // }
    // This was the previous implementation. We're trying to avoid allocations of LikeExpressions, GroupBy and LINQ.
    // The new implementation preserves the behavior and reduces allocations.
    // Instead of GroupBy, we have a single array, sorted by group, and we slice it to get the groups.

    public IEnumerable<T> Evaluate<T>(IEnumerable<T> source, Specification<T> specification)
    {
        var count = GetCount(specification);
        if (count == 0) return source;

        var array = new SpecState[count];
        FillSorted(specification, array);

        // TODO: We can't use ArrayPool with yield return. We'll have to create a custom enumerator. [fatii, 30/10/2024]
        // In the custom enumerator perhaps we can return the array in the Dispose method.
        return LikeMemoryIterator<T>(source, array);
    }

    private static IEnumerable<T> LikeMemoryIterator<T>(IEnumerable<T> source, SpecState[] array)
    {
        foreach (var item in source)
        {
            if (IsMatch(array, item))
            {
                yield return item;
            }
        }
    }

    private static bool IsMatch<T>(Span<SpecState> span, T item)
    {
        var match = true;

        int start = 0;
        for (int i = 1; i <= span.Length; i++)
        {
            if (i == span.Length || span[i].Bag != span[start].Bag)
            {
                var matchOrGroup = IsMatchInOrGroup(span[start..i], item);
                if ((match = match && matchOrGroup) is false)
                {
                    break;
                }
                start = i;
            }
        }

        return match;

        static bool IsMatchInOrGroup(Span<SpecState> likeStates, T item)
        {
            var matchOrGroup = false;
            foreach (var state in likeStates)
            {
                if (state.Reference is not SpecLike<T> specLike) continue;

                if (specLike.KeySelector.Compile()(item)?.Like(specLike.Pattern) ?? false)
                {
                    matchOrGroup = true;
                    break;
                }
            }
            return matchOrGroup;
        }
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
}
