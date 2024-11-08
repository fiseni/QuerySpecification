namespace Pozitron.QuerySpecification;

/// <summary>
/// Represents an evaluator that processes a specification.
/// </summary>
public interface IEvaluator
{
    /// <summary>
    /// Evaluates the given specification on the provided queryable source.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="source">The queryable source.</param>
    /// <param name="specification">The specification to evaluate.</param>
    /// <returns>The evaluated queryable source.</returns>
    IQueryable<T> Evaluate<T>(IQueryable<T> source, Specification<T> specification) where T : class;
}
