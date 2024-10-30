using System.Buffers;

namespace Pozitron.QuerySpecification;

public sealed class LikeEvaluator : IEvaluator
{
    private LikeEvaluator() { }
    public static LikeEvaluator Instance = new();

    // public IQueryable<T> Evaluate<T>(IQueryable<T> source, Specification<T> specification) where T : class
    // {
    //     foreach (var likeGroup in specification.LikeExpressions.GroupBy(x => x.Group))
    //     {
    //         source = source.Like(likeGroup);
    //     }
    //     return source;
    // }
    // This was the previous implementation. We're trying to avoid allocations of LikeExpressions and GroupBy.
    // The new implementation preserves the behavior and has zero allocations.
    // Instead of GroupBy, we have a single array, sorted by group, and we slice it to get the groups.

    public IQueryable<T> Evaluate<T>(IQueryable<T> source, Specification<T> specification) where T : class
    {
        var count = GetCount(specification);
        if (count == 0) return source;

        SpecState[]? array = ArrayPool<SpecState>.Shared.Rent(count);

        try
        {
            // The ArrayPool may return an array with a larger size than requested.
            // We'll create a span with the exact size.
            var span = array.AsSpan()[..count];

            FillSorted(specification, span);
            source = ApplyLikeExpressions(source, span);
        }
        finally
        {
            ArrayPool<SpecState>.Shared.Return(array);
        }

        return source;
    }

    private static IQueryable<T> ApplyLikeExpressions<T>(IQueryable<T> source, Span<SpecState> span) where T : class
    {
        int start = 0;

        for (int i = 1; i <= span.Length; i++)
        {
            if (i == span.Length || span[i].Bag != span[start].Bag)
            {
                source = source.Like(span[start..i]);
                start = i;
            }
        }

        return source;
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

