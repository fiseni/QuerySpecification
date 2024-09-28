namespace Pozitron.QuerySpecification;

public class LikeEvaluator : IEvaluator
{
    private LikeEvaluator() { }
    public static LikeEvaluator Instance = new();

    public IQueryable<T> Evaluate<T>(IQueryable<T> source, Specification<T> specification) where T : class
    {
        foreach (var likeGroup in specification.LikeExpressions.GroupBy(x => x.Group))
        {
            source = source.Like(likeGroup);
        }

        return source;
    }
}
