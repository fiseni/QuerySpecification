﻿namespace Pozitron.QuerySpecification;

public class LikeEvaluator : IInMemoryEvaluator
{
    private LikeEvaluator() { }
    public static LikeEvaluator Instance = new();

    public IEnumerable<T> Evaluate<T>(IEnumerable<T> query, Specification<T> specification)
    {
        foreach (var likeGroup in specification.LikeExpressions.GroupBy(x => x.Group))
        {
            query = query.Where(x => likeGroup.Any(c => c.KeySelectorFunc(x).Like(c.Pattern)));
        }

        return query;
    }
}
