namespace Pozitron.QuerySpecification;

public static class PaginationExtensions
{
    public static IQueryable<TResult> ApplyPaging<T, TResult>(this IQueryable<TResult> source, Specification<T, TResult> specification)
    {
        var paging = specification.FirstOrDefault<SpecPaging>(ItemType.Paging);
        if (paging is null) return source;

        return ApplyPaging(source, paging.Skip, paging.Take);
    }

    public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> source, Specification<T> specification)
    {
        var paging = specification.FirstOrDefault<SpecPaging>(ItemType.Paging);
        if (paging is null) return source;

        return ApplyPaging(source, paging.Skip, paging.Take);
    }

    public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> source, Pagination pagination)
        => ApplyPaging(source, pagination.Skip, pagination.Take);

    public static IEnumerable<TResult> ApplyPaging<T, TResult>(this IEnumerable<TResult> source, Specification<T, TResult> specification)
    {
        var paging = specification.FirstOrDefault<SpecPaging>(ItemType.Paging);
        if (paging is null) return source;

        return ApplyPaging(source, paging.Skip, paging.Take);
    }

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

