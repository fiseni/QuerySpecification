namespace Pozitron.QuerySpecification;

public static partial class SpecificationBuilderExtensions
{
    /// <summary>
    /// Sets the cache key for the specification.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <param name="cacheKey">The cache key to be used.</param>
    /// <returns>The updated specification builder.</returns>
    public static ISpecificationBuilder<T, TResult> WithCacheKey<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder,
        string cacheKey) where T : class
    {
        WithCacheKey(builder, cacheKey, true);
        return (SpecificationBuilder<T, TResult>)builder;
    }

    /// <summary>
    /// Sets the cache key for the specification.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <param name="cacheKey">The cache key to be used.</param>
    /// <param name="condition">The condition to evaluate.</param>
    /// <returns>The updated specification builder.</returns>
    public static ISpecificationBuilder<T, TResult> WithCacheKey<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder,
        string cacheKey,
        bool condition) where T : class
    {
        if (condition)
        {
            builder.Specification.AddOrUpdateInternal(ItemType.CacheKey, cacheKey);
        }

        return builder;
    }

    /// <summary>
    /// Sets the cache key for the specification.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <param name="cacheKey">The cache key to be used.</param>
    /// <returns>The updated specification builder.</returns>
    public static ISpecificationBuilder<T> WithCacheKey<T>(
        this ISpecificationBuilder<T> builder,
        string cacheKey) where T : class
        => WithCacheKey(builder, cacheKey, true);

    /// <summary>
    /// Sets the cache key for the specification.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <param name="cacheKey">The cache key to be used.</param>
    /// <param name="condition">The condition to evaluate.</param>
    /// <returns>The updated specification builder.</returns>
    public static ISpecificationBuilder<T> WithCacheKey<T>(
        this ISpecificationBuilder<T> builder,
        string cacheKey,
        bool condition) where T : class
    {
        if (condition)
        {
            builder.Specification.AddOrUpdateInternal(ItemType.CacheKey, cacheKey);
        }

        return builder;
    }
}
