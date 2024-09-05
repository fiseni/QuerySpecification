﻿namespace Pozitron.QuerySpecification;

public class WhereEvaluator : IEvaluator, IInMemoryEvaluator
{
    private WhereEvaluator() { }
    public static WhereEvaluator Instance = new();

    public IQueryable<T> GetQuery<T>(IQueryable<T> query, Specification<T> specification) where T : class
    {
        foreach (var info in specification.WhereExpressions)
        {
            query = query.Where(info.Filter);
        }

        return query;
    }

    public IEnumerable<T> Evaluate<T>(IEnumerable<T> query, Specification<T> specification)
    {
        foreach (var info in specification.WhereExpressions)
        {
            query = query.Where(info.FilterFunc);
        }

        return query;
    }
}