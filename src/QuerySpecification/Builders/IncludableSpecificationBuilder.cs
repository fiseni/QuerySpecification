namespace Pozitron.QuerySpecification;

/// <summary>
/// Represents a specification builder that supports include operations.
/// </summary>
/// <typeparam name="T">The type of the entity.</typeparam>
/// <typeparam name="TResult">The type of the result.</typeparam>
/// <typeparam name="TProperty">The type of the property.</typeparam>
public interface IIncludableSpecificationBuilder<T, TResult, out TProperty> : ISpecificationBuilder<T, TResult> where T : class
{
}

/// <summary>
/// Represents a specification builder that supports include operations.
/// </summary>
/// <typeparam name="T">The type of the entity.</typeparam>
/// <typeparam name="TProperty">The type of the property.</typeparam>
public interface IIncludableSpecificationBuilder<T, out TProperty> : ISpecificationBuilder<T> where T : class
{
}

internal class IncludableSpecificationBuilder<T, TResult, TProperty>(Specification<T, TResult> specification)
    : SpecificationBuilder<T, TResult>(specification), IIncludableSpecificationBuilder<T, TResult, TProperty> where T : class
{
}

internal class IncludableSpecificationBuilder<T, TProperty>(Specification<T> specification)
    : SpecificationBuilder<T>(specification), IIncludableSpecificationBuilder<T, TProperty> where T : class
{
}
