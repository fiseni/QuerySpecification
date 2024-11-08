namespace Pozitron.QuerySpecification;

public static partial class SpecificationBuilderExtensions
{
    /// <summary>
    /// Adds a Like clause to the specification.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <param name="keySelector">The key selector expression.</param>
    /// <param name="pattern">The pattern to match.</param>
    /// <param name="group">The group number. Like clauses within the same group are evaluated using OR logic.</param>
    /// <returns>The updated specification builder.</returns>
    public static ISpecificationBuilder<T, TResult> Like<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder,
        Expression<Func<T, string?>> keySelector,
        string pattern,
        int group = 1) where T : class
        => Like(builder, keySelector, pattern, true, group);

    /// <summary>
    /// Adds a Like clause to the specification if the condition is true.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <param name="keySelector">The key selector expression.</param>
    /// <param name="pattern">The pattern to match.</param>
    /// <param name="condition">The condition to evaluate.</param>
    /// <param name="group">The group number. Like clauses within the same group are evaluated using OR logic.</param>
    /// <returns>The updated specification builder.</returns>
    public static ISpecificationBuilder<T, TResult> Like<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder,
        Expression<Func<T, string?>> keySelector,
        string pattern,
        bool condition,
        int group = 1) where T : class
    {
        if (condition)
        {
            var like = new SpecLike<T>(keySelector, pattern);
            builder.Specification.AddInternal(ItemType.Like, like, group);
        }
        return builder;
    }

    /// <summary>
    /// Adds a Like clause to the specification.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <param name="keySelector">The key selector expression.</param>
    /// <param name="pattern">The pattern to match.</param>
    /// <param name="group">The group number. Like clauses within the same group are evaluated using OR logic.</param>
    /// <returns>The updated specification builder.</returns>
    public static ISpecificationBuilder<T> Like<T>(
        this ISpecificationBuilder<T> builder,
        Expression<Func<T, string?>> keySelector,
        string pattern,
        int group = 1) where T : class
        => Like(builder, keySelector, pattern, true, group);

    /// <summary>
    /// Adds a Like clause to the specification if the condition is true.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <param name="keySelector">The key selector expression.</param>
    /// <param name="pattern">The pattern to match.</param>
    /// <param name="condition">The condition to evaluate.</param>
    /// <param name="group">The group number. Like clauses within the same group are evaluated using OR logic.</param>
    /// <returns>The updated specification builder.</returns>
    public static ISpecificationBuilder<T> Like<T>(
        this ISpecificationBuilder<T> builder,
        Expression<Func<T, string?>> keySelector,
        string pattern,
        bool condition,
        int group = 1) where T : class
    {
        if (condition)
        {
            var like = new SpecLike<T>(keySelector, pattern);
            builder.Specification.AddInternal(ItemType.Like, like, group);
        }
        return builder;
    }
}
