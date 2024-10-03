namespace Pozitron.QuerySpecification;

public class LikeMemoryEvaluator : IInMemoryEvaluator
{
    private LikeMemoryEvaluator() { }
    public static LikeMemoryEvaluator Instance = new();

    public IEnumerable<T> Evaluate<T>(IEnumerable<T> source, Specification<T> specification)
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
