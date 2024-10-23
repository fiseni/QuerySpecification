namespace Pozitron.QuerySpecification;

public sealed class LikeValidator : IValidator
{
    private LikeValidator() { }
    public static LikeValidator Instance = new();

    public bool IsValid<T>(T entity, Specification<T> specification)
    {
        // There are benchmarks in QuerySpecification.Benchmarks project.
        // This implementation was the most efficient one.

        var groups = specification.LikeExpressions.GroupBy(x => x.Group);

        foreach (var group in groups)
        {
            var match = false;
            foreach (var like in group)
            {
                if (like.KeySelectorFunc(entity)?.Like(like.Pattern) ?? false)
                {
                    match = true;
                    break;
                }
            }

            if (match is false)
                return false;
        }

        return true;
    }
}
