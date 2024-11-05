﻿namespace Pozitron.QuerySpecification;

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
        var compiledItems = specification.GetCompiledItems();
        if (compiledItems.Length == 0) return true;

        var startIndexLikeItems = Array.FindIndex(compiledItems, item => item.Type == ItemType.Like);
        if (startIndexLikeItems == -1) return true;

        // The like items are contiguously placed as a last segment in the array and are already sorted by group.
        return IsValid(entity, compiledItems.AsSpan()[startIndexLikeItems..compiledItems.Length]);
    }

    private static bool IsValid<T>(T entity, ReadOnlySpan<SpecItem> span)
    {
        var start = 0;
        for (var i = 1; i <= span.Length; i++)
        {
            if (i == span.Length || span[i].Bag != span[start].Bag)
            {
                if (IsValidInOrGroup(entity, span[start..i]) is false)
                {
                    return false;
                }
                start = i;
            }
        }
        return true;

        static bool IsValidInOrGroup(T entity, ReadOnlySpan<SpecItem> span)
        {
            var validOrGroup = false;
            foreach (var specItem in span)
            {
                if (specItem.Reference is not SpecLikeCompiled<T> specLike) continue;

                if (specLike.KeySelector(entity)?.Like(specLike.Pattern) ?? false)
                {
                    validOrGroup = true;
                    break;
                }
            }
            return validOrGroup;
        }
    }
}
