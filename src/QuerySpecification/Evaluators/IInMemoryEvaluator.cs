﻿namespace Pozitron.QuerySpecification;

/// <summary>
/// Represents an in-memory evaluator that processes a specification.
/// </summary>
public interface IInMemoryEvaluator
{
    /// <summary>
    /// Evaluates the given specification on the provided enumerable source.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="source">The enumerable source.</param>
    /// <param name="specification">The specification to evaluate.</param>
    /// <returns>The evaluated enumerable source.</returns>
    IEnumerable<T> Evaluate<T>(IEnumerable<T> source, Specification<T> specification);
}
