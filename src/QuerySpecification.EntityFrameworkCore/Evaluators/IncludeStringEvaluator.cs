namespace Pozitron.QuerySpecification;

/// <summary>
/// Evaluates a specification to include navigation properties specified by string paths.
/// </summary>
[EvaluatorDiscovery(Order = -90)]
public sealed class IncludeStringEvaluator : IEvaluator
{

    /// <summary>
    /// Gets the singleton instance of the <see cref="IncludeStringEvaluator"/> class.
    /// </summary>
    public static IncludeStringEvaluator Instance = new();
    private IncludeStringEvaluator() { }

    /// <inheritdoc/>
    public IQueryable<T> Evaluate<T>(IQueryable<T> source, Specification<T> specification) where T : class
    {
        foreach (var item in specification.Items)
        {
            if (item.Type == ItemType.IncludeString && item.Reference is string includeString)
            {
                source = source.Include(includeString);
            }
        }

        return source;
    }
}
