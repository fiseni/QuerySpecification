﻿namespace Pozitron.QuerySpecification;

public class SearchEvaluator : IInMemoryEvaluator
{
    private SearchEvaluator() { }
    public static SearchEvaluator Instance { get; } = new SearchEvaluator();

    public IEnumerable<T> Evaluate<T>(IEnumerable<T> query, Specification<T> specification)
    {
        foreach (var searchGroup in specification.Context.SearchCriterias.GroupBy(x => x.SearchGroup))
        {
            query = query.Where(x => searchGroup.Any(c => c.SelectorFunc(x).Like(c.SearchTerm)));
        }

        return query;
    }
}
