namespace Pozitron.QuerySpecification;

/// <summary>
/// Evaluator to apply IgnoreAutoIncludes to the query if the specification has IgnoreAutoIncludes set to true.
/// </summary>
[EvaluatorDiscovery(Order = 70)]
public sealed class IgnoreAutoIncludesEvaluator : IEvaluator, ISpecItemKind
{

    /// <summary>
    /// Gets the singleton instance of the <see cref="IgnoreAutoIncludesEvaluator"/> class.
    /// </summary>
    public static IgnoreAutoIncludesEvaluator Instance = new();
    private IgnoreAutoIncludesEvaluator() { }

    int ISpecItemKind.ItemType => Pozitron.QuerySpecification.ItemType.Flags;

    /// <inheritdoc/>
    public IQueryable<T> Evaluate<T>(IQueryable<T> source, Specification<T> specification) where T : class
    {
        if (specification.IgnoreAutoIncludes)
        {
            source = source.IgnoreAutoIncludes();
        }

        return source;
    }
}
