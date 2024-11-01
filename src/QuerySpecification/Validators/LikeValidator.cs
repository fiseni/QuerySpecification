namespace Pozitron.QuerySpecification;

/*
    public bool IsValid<T>(T entity, Specification<T> specification)
    {
        foreach (var likeGroup in specification.LikeExpressions.GroupBy(x => x.Group))
        {
            if (likeGroup.Any(c => c.KeySelectorFunc(entity)?.Like(c.Pattern) ?? false) == false) return false;
        }
        return true;
    }
    This was the previous implementation.We're trying to avoid allocations of LikeExpressions, GroupBy and LINQ.
    Instead of GroupBy, we have a single array sorted by group, and we slice it to get the groups.
    The new implementation preserves the behavior and reduces allocations drastically.
    For 1000 items, the allocations are reduced from 651.160 bytes to ZERO bytes. Refer to LikeValidatorBenchmark results.
 */

public sealed class LikeValidator : IValidator
{
    private LikeValidator() { }
    public static LikeValidator Instance = new();

    public bool IsValid<T>(T entity, Specification<T> specification)
    {
        if (specification.IsEmpty) return true;

        var compiledStates = specification.GetCompiledStates();
        if (compiledStates.Length == 0) return true;

        int startIndexLikeStates = Array.FindIndex(compiledStates, state => state.Type == StateType.Like);
        if (startIndexLikeStates == -1) return true;

        // The like states are contiguous placed as last segment in the array and are already sorted by group.
        return IsValid(entity, compiledStates.AsSpan()[startIndexLikeStates..compiledStates.Length]);
    }

    private static bool IsValid<T>(T item, ReadOnlySpan<SpecState> span)
    {
        int start = 0;
        for (int i = 1; i <= span.Length; i++)
        {
            if (i == span.Length || span[i].Bag != span[start].Bag)
            {
                if (IsValidInOrGroup(item, span[start..i]) is false)
                {
                    return false;
                }
                start = i;
            }
        }
        return true;

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
