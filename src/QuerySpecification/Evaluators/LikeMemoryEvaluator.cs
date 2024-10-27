namespace Pozitron.QuerySpecification;

public sealed class LikeMemoryEvaluator : IInMemoryEvaluator
{
    private LikeMemoryEvaluator() { }
    public static LikeMemoryEvaluator Instance = new();

    public IEnumerable<T> Evaluate<T>(IEnumerable<T> source, Specification<T> specification)
    {
        if (!specification.Contains(StateType.Like)) return source;

        return LikeMemoryIterator<T>(source, specification);
    }

    // TODO: Refactor this. Create custom iterator (backed with state array) instead of using yield [fatii, 27/10/2024]
    // TODO: Refactor the Like state. Use the bag for grouping instead keeping it in the object state. We'll save 8 bytes. [fatii, 27/10/2024]
    private static IEnumerable<T> LikeMemoryIterator<T>(IEnumerable<T> source, Specification<T> specification)
    {
        // There are benchmarks in QuerySpecification.Benchmarks project.
        // This implementation was the most efficient one.

        var groups = specification.LikeExpressions.GroupBy(x => x.Group).ToList();

        foreach (var item in source)
        {
            var match = true;
            foreach (var group in groups)
            {
                var matchOrGroup = false;
                foreach (var like in group)
                {
                    if (like.KeySelectorFunc(item)?.Like(like.Pattern) ?? false)
                    {
                        matchOrGroup = true;
                        break;
                    }
                }

                if ((match = match && matchOrGroup) is false)
                {
                    break;
                }
            }

            if (match)
            {
                yield return item;
            }
        }
    }
}
