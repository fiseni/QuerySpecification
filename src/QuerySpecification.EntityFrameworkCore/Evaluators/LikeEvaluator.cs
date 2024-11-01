using System.Buffers;

namespace Pozitron.QuerySpecification;

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

public sealed class LikeEvaluator : IEvaluator
{
    private LikeEvaluator() { }
    public static LikeEvaluator Instance = new();

    public IQueryable<T> Evaluate<T>(IQueryable<T> source, Specification<T> specification) where T : class
    {
        if (specification.IsEmpty) return source;

        var count = GetCount(specification);
        if (count == 0) return source;

        if (count == 1)
        {
            // Specs with a single Like are the most common. We can optimize for this case to avoid all the additional overhead.
            source = ApplySingleLike(source, specification);
            return source;
        }

        SpecState[]? array = ArrayPool<SpecState>.Shared.Rent(count);

        try
        {
            var span = array.AsSpan()[..count];
            FillSorted(specification, span);
            source = ApplyLike(source, span);
        }
        finally
        {
            ArrayPool<SpecState>.Shared.Return(array);
        }

        return source;
    }

    private static IQueryable<T> ApplySingleLike<T>(IQueryable<T> source, Specification<T> specification) where T : class
    {
        var states = specification.States;
        for (int i = 0; i < states.Length; i++)
        {
            if (states[i].Type == StateType.Like)
            {
                return source.ApplyLikesAsOrGroup(states.Slice(i, 1));
            }
        }
        return source;
    }

    private static IQueryable<T> ApplyLike<T>(IQueryable<T> source, Span<SpecState> span) where T : class
    {
        int start = 0;

        for (int i = 1; i <= span.Length; i++)
        {
            if (i == span.Length || span[i].Bag != span[start].Bag)
            {
                source = source.ApplyLikesAsOrGroup(span[start..i]);
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

