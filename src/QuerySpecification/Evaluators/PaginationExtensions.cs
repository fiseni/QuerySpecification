namespace Pozitron.QuerySpecification;

/// <summary>
/// Provides extension methods for applying pagination to queryable and enumerable sources.
/// </summary>
public static class PaginationExtensions
{
    /// <summary>
    /// Applies pagination to the queryable source based on the specification.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="source">The queryable source.</param>
    /// <param name="specification">The specification containing pagination settings.</param>
    /// <returns>The paginated queryable source.</returns>
    public static IQueryable<TResult> ApplyPaging<T, TResult>(this IQueryable<TResult> source, Specification<T, TResult> specification)
    {
        var paging = specification.FirstOrDefault<SpecPaging>(ItemType.Paging);
        if (paging is null) return source;

        return ApplyPaging(source, paging.Skip, paging.Take);
    }

    /// <summary>
    /// Applies pagination to the enumerable source based on the specification.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="source">The enumerable source.</param>
    /// <param name="specification">The specification containing pagination settings.</param>
    /// <returns>The paginated enumerable source.</returns>
    public static IEnumerable<TResult> ApplyPaging<T, TResult>(this IEnumerable<TResult> source, Specification<T, TResult> specification)
    {
        var paging = specification.FirstOrDefault<SpecPaging>(ItemType.Paging);
        if (paging is null) return source;

        return ApplyPaging(source, paging.Skip, paging.Take);
    }

    /// <summary>
    /// Applies pagination to the queryable source based on the pagination settings.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="source">The queryable source.</param>
    /// <param name="pagination">The pagination settings.</param>
    /// <returns>The paginated queryable source.</returns>
    public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> source, Pagination pagination)
        => ApplyPaging(source, pagination.Skip, pagination.Take);

    /// <summary>
    /// Applies pagination to the queryable source based on the specification.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="source">The queryable source.</param>
    /// <param name="specification">The specification containing pagination settings.</param>
    /// <returns>The paginated queryable source.</returns>
    public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> source, Specification<T> specification)
    {
        var paging = specification.FirstOrDefault<SpecPaging>(ItemType.Paging);
        if (paging is null) return source;

        return ApplyPaging(source, paging.Skip, paging.Take);
    }

    /// <summary>
    /// Applies pagination to the enumerable source based on the specification.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="source">The enumerable source.</param>
    /// <param name="specification">The specification containing pagination settings.</param>
    /// <returns>The paginated enumerable source.</returns>
    public static IEnumerable<T> ApplyPaging<T>(this IEnumerable<T> source, Specification<T> specification)
    {
        var paging = specification.FirstOrDefault<SpecPaging>(ItemType.Paging);
        if (paging is null) return source;

        return ApplyPaging(source, paging.Skip, paging.Take);
    }

    private static IQueryable<T> ApplyPaging<T>(this IQueryable<T> source, int skip, int take)
    {
        // If skip is 0, avoid adding to the IQueryable. It will generate more optimized SQL that way.
        if (skip > 0)
        {
            source = source.Skip(skip);
        }

        if (take >= 0)
        {
            source = source.Take(take);
        }

        return source;
    }

    private static IEnumerable<T> ApplyPaging<T>(this IEnumerable<T> source, int skip, int take)
    {
        if (skip > 0)
        {
            source = source.Skip(skip);
        }

        if (take >= 0)
        {
            source = source.Take(take);
        }

        return source;
    }
}
