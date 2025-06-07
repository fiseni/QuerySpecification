using System.Buffers;

namespace Pozitron.QuerySpecification;

/*
    public IQueryable<T> Evaluate<T>(IQueryable<T> source, Specification<T> specification) where T : class
    {
        foreach (var likeGroup in specification.LikeExpressions.GroupBy(x => x.Group))
        {
            source = source.Like(likeGroup);
        }
        return source;
    }
    This was the previous implementation. We're trying to avoid allocations of LikeExpressions and GroupBy.
    The new implementation preserves the behavior and has zero allocations.
    Instead of GroupBy, we have a single array, sorted by group, and we slice it to get the groups.
*/

/// <summary>
/// Evaluates a specification to apply "like" expressions for filtering.
/// </summary>
[EvaluatorDiscovery(Order = 20)]
public sealed class LikeEvaluator : IEvaluator
{
    private LikeEvaluator() { }

    /// <summary>
    /// Gets the singleton instance of the <see cref="LikeEvaluator"/> class.
    /// </summary>
    public static LikeEvaluator Instance = new();

    /// <inheritdoc/>
    public IQueryable<T> Evaluate<T>(IQueryable<T> source, Specification<T> specification) where T : class
    {
        var likeCount = GetLikeCount(specification);
        if (likeCount == 0) return source;

        if (likeCount == 1)
        {
            // Specs with a single Like are the most common. We can optimize for this case to avoid all the additional overhead.
            var items = specification.Items;
            for (var i = 0; i < items.Length; i++)
            {
                if (items[i].Type == ItemType.Like)
                {
                    return source.ApplyLikesAsOrGroup(items.Slice(i, 1));
                }
            }
        }

        SpecItem[] array = ArrayPool<SpecItem>.Shared.Rent(likeCount);

        try
        {
            var span = array.AsSpan()[..likeCount];
            FillSorted(specification, span);
            source = ApplyLike(source, span);
        }
        finally
        {
            ArrayPool<SpecItem>.Shared.Return(array);
        }

        return source;
    }

    private static IQueryable<T> ApplyLike<T>(IQueryable<T> source, ReadOnlySpan<SpecItem> span) where T : class
    {
        var groupStart = 0;
        for (var i = 1; i <= span.Length; i++)
        {
            // If we reached the end of the span or the group has changed, we slice and process the group.
            if (i == span.Length || span[i].Bag != span[groupStart].Bag)
            {
                source = source.ApplyLikesAsOrGroup(span[groupStart..i]);
                groupStart = i;
            }
        }
        return source;
    }

    private static int GetLikeCount<T>(Specification<T> specification)
    {
        var count = 0;
        foreach (var item in specification.Items)
        {
            if (item.Type == ItemType.Like)
                count++;
        }
        return count;
    }

    private static void FillSorted<T>(Specification<T> specification, Span<SpecItem> span)
    {
        var i = 0;
        foreach (var item in specification.Items)
        {
            if (item.Type == ItemType.Like)
            {
                // Find the correct insertion point
                var j = i;
                while (j > 0 && span[j - 1].Bag > item.Bag)
                {
                    span[j] = span[j - 1];
                    j--;
                }

                // Insert the current item in the sorted position
                span[j] = item;
                i++;
            }
        }
    }
}
