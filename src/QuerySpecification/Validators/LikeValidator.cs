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
    For 1000 entities, the allocations are reduced from 651.160 bytes to ZERO bytes. Refer to LikeValidatorBenchmark results.
 */

/// <summary>
/// Represents a validator for "like" expressions.
/// </summary>
[ValidatorDiscovery(Order = 20)]
public sealed class LikeValidator : IValidator
{
    /// <summary>
    /// Gets the singleton instance of the <see cref="LikeValidator"/> class.
    /// </summary>
    public static LikeValidator Instance = new();
    private LikeValidator() { }

    /// <inheritdoc/>
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
        var groupStart = 0;
        for (var i = 1; i <= span.Length; i++)
        {
            // If we reached the end of the span or the group has changed, we slice and process the group.
            if (i == span.Length || span[i].Bag != span[groupStart].Bag)
            {
                if (IsValidInOrGroup(entity, span[groupStart..i]) is false)
                {
                    return false;
                }
                groupStart = i;
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
