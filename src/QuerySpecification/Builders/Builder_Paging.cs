namespace Pozitron.QuerySpecification;

public static partial class SpecificationBuilderExtensions
{
    /// <summary>
    /// Sets the number of items to take in the specification.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <param name="take">The number of items to take.</param>
    /// <returns>The updated specification builder.</returns>
    public static ISpecificationBuilder<T, TResult> Take<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder,
        int take)
        => Take(builder, take, true);

    /// <summary>
    /// Sets the number of items to take in the specification if the condition is true.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <param name="take">The number of items to take.</param>
    /// <param name="condition">The condition to evaluate.</param>
    /// <returns>The updated specification builder.</returns>
    public static ISpecificationBuilder<T, TResult> Take<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder,
        int take,
        bool condition)
    {
        if (condition)
        {
            builder.Specification.GetOrCreate<SpecPaging>(ItemType.Paging).Take = take;
        }
        return builder;
    }

    /// <summary>
    /// Sets the number of items to take in the specification.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <param name="take">The number of items to take.</param>
    /// <returns>The updated specification builder.</returns>
    public static ISpecificationBuilder<T> Take<T>(
        this ISpecificationBuilder<T> builder,
        int take)
        => Take(builder, take, true);

    /// <summary>
    /// Sets the number of items to take in the specification if the condition is true.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <param name="take">The number of items to take.</param>
    /// <param name="condition">The condition to evaluate.</param>
    /// <returns>The updated specification builder.</returns>
    public static ISpecificationBuilder<T> Take<T>(
        this ISpecificationBuilder<T> builder,
        int take,
        bool condition)
    {
        if (condition)
        {
            builder.Specification.GetOrCreate<SpecPaging>(ItemType.Paging).Take = take;
        }
        return builder;
    }

    /// <summary>
    /// Sets the number of items to skip in the specification.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <param name="skip">The number of items to skip.</param>
    /// <returns>The updated specification builder.</returns>
    public static ISpecificationBuilder<T, TResult> Skip<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder,
        int skip)
        => Skip(builder, skip, true);

    /// <summary>
    /// Sets the number of items to skip in the specification if the condition is true.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <param name="skip">The number of items to skip.</param>
    /// <param name="condition">The condition to evaluate.</param>
    /// <returns>The updated specification builder.</returns>
    public static ISpecificationBuilder<T, TResult> Skip<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder,
        int skip,
        bool condition)
    {
        if (condition)
        {
            builder.Specification.GetOrCreate<SpecPaging>(ItemType.Paging).Skip = skip;
        }
        return builder;
    }

    /// <summary>
    /// Sets the number of items to skip in the specification.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <param name="skip">The number of items to skip.</param>
    /// <returns>The updated specification builder.</returns>
    public static ISpecificationBuilder<T> Skip<T>(
        this ISpecificationBuilder<T> builder,
        int skip)
        => Skip(builder, skip, true);

    /// <summary>
    /// Sets the number of items to skip in the specification if the condition is true.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <param name="skip">The number of items to skip.</param>
    /// <param name="condition">The condition to evaluate.</param>
    /// <returns>The updated specification builder.</returns>
    public static ISpecificationBuilder<T> Skip<T>(
        this ISpecificationBuilder<T> builder,
        int skip,
        bool condition)
    {
        if (condition)
        {
            builder.Specification.GetOrCreate<SpecPaging>(ItemType.Paging).Skip = skip;
        }
        return builder;
    }
}
