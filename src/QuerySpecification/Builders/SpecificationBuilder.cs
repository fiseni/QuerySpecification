namespace Pozitron.QuerySpecification;

/// <summary>
/// Represents a specification builder that supports order operations.
/// </summary>
/// <typeparam name="T">The type of the entity.</typeparam>
/// <typeparam name="TResult">The type of the result.</typeparam>
public interface IOrderedSpecificationBuilder<T, TResult> : ISpecificationBuilder<T, TResult>
{
}

/// <summary>
/// Represents a specification builder that supports order operations.
/// </summary>
/// <typeparam name="T">The type of the entity.</typeparam>
public interface IOrderedSpecificationBuilder<T> : ISpecificationBuilder<T>
{
}

/// <summary>
/// Represents a specification builder.
/// </summary>
/// <typeparam name="T">The type of the entity.</typeparam>
/// <typeparam name="TResult">The type of the result.</typeparam>
public interface ISpecificationBuilder<T, TResult>
{
    internal Specification<T, TResult> Specification { get; }

    /// <summary>
    /// Adds an item to the specification.
    /// </summary>
    /// <param name="type">The type of the item.</param>
    /// <param name="value">The object to be stored in the item.</param>
    /// <exception cref="ArgumentNullException">Thrown if value is null</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if type is zero or negative.</exception>
    void Add(int type, object value);

    /// <summary>
    /// Adds or updates an item in the specification.
    /// </summary>
    /// <param name="type">The type of the item.</param>
    /// <param name="value">The object to be stored in the item.</param>
    /// <exception cref="ArgumentNullException">Thrown if value is null</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if type is zero or negative.</exception>
    void AddOrUpdate(int type, object value);
}

/// <summary>
/// Represents a specification builder.
/// </summary>
/// <typeparam name="T">The type of the entity.</typeparam>
public interface ISpecificationBuilder<T>
{
    internal Specification<T> Specification { get; }

    /// <summary>
    /// Adds an item to the specification.
    /// </summary>
    /// <param name="type">The type of the item.</param>
    /// <param name="value">The object to be stored in the item.</param>
    /// <exception cref="ArgumentNullException">Thrown if value is null</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if type is zero or negative.</exception>
    void Add(int type, object value);

    /// <summary>
    /// Adds or updates an item in the specification.
    /// </summary>
    /// <param name="type">The type of the item.</param>
    /// <param name="value">The object to be stored in the item.</param>
    /// <exception cref="ArgumentNullException">Thrown if value is null</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if type is zero or negative.</exception>
    void AddOrUpdate(int type, object value);
}

internal class SpecificationBuilder<T, TResult>(Specification<T, TResult> specification)
    : IOrderedSpecificationBuilder<T, TResult>, ISpecificationBuilder<T, TResult>
{
    public Specification<T, TResult> Specification { get; } = specification;
    public void Add(int type, object value) => Specification.Add(type, value);
    public void AddOrUpdate(int type, object value) => Specification.AddOrUpdate(type, value);
}

internal class SpecificationBuilder<T>(Specification<T> specification)
    : IOrderedSpecificationBuilder<T>, ISpecificationBuilder<T>
{
    public Specification<T> Specification { get; } = specification;
    public void Add(int type, object value) => Specification.Add(type, value);
    public void AddOrUpdate(int type, object value) => Specification.AddOrUpdate(type, value);
}
