namespace Pozitron.QuerySpecification;

public class LikeValidator : IValidator
{
    private LikeValidator() { }
    public static LikeValidator Instance = new();

    public bool IsValid<T>(T entity, Specification<T> specification)
    {
        foreach (var likeGroup in specification.LikeExpressions.GroupBy(x => x.Group))
        {
            if (likeGroup.Any(c => c.KeySelectorFunc(entity).Like(c.Pattern)) == false) return false;
        }

        return true;
    }
}
