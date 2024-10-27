﻿namespace Pozitron.QuerySpecification;

public sealed class WhereEvaluator : IEvaluator, IInMemoryEvaluator
{
    private WhereEvaluator() { }
    public static WhereEvaluator Instance = new();

    public IQueryable<T> Evaluate<T>(IQueryable<T> source, Specification<T> specification) where T : class
    {
        if (specification.IsEmpty) return source;

        foreach (var state in specification.States)
        {
            if (state.Type == StateType.Where && state.Reference is Expression<Func<T, bool>> expr)
            {
                source = source.Where(expr);
            }
        }

        return source;
    }

    public IEnumerable<T> Evaluate<T>(IEnumerable<T> source, Specification<T> specification)
    {
        if (specification.IsEmpty) return source;

        foreach (var state in specification.States)
        {
            if (state.Type == StateType.Where && state.Reference is Expression<Func<T, bool>> expr)
            {
                var func = expr.Compile();
                source = source.Where(func);
            }
        }

        return source;
    }
}

