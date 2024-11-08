using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace Pozitron.QuerySpecification;

/// <summary>
/// Represents pagination information.
/// </summary>
public record Pagination
{
    private readonly PaginationSettings _paginationSettings;

    /// <summary>
    /// Gets the total number of items.
    /// </summary>
    public int TotalItems { get; }

    /// <summary>
    /// Gets the total number of pages.
    /// </summary>
    public int TotalPages { get; }

    /// <summary>
    /// Gets the page size.
    /// </summary>
    public int PageSize { get; }

    /// <summary>
    /// Gets the current page number.
    /// </summary>
    public int Page { get; }

    /// <summary>
    /// Gets the start item number.
    /// </summary>
    public int StartItem { get; }

    /// <summary>
    /// Gets the end item number.
    /// </summary>
    public int EndItem { get; }

    /// <summary>
    /// Gets a value indicating whether there is a previous page.
    /// </summary>
    public bool HasPrevious { get; }

    /// <summary>
    /// Gets a value indicating whether there is a next page.
    /// </summary>
    public bool HasNext { get; }

    /// <summary>
    /// Gets the number of items to take.
    /// </summary>
    [JsonIgnore]
    public int Take { get; }

    /// <summary>
    /// Gets the number of items to skip.
    /// </summary>
    [JsonIgnore]
    public int Skip { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Pagination"/> class.
    /// </summary>
    /// <param name="totalItems">The total number of items.</param>
    /// <param name="totalPages">The total number of pages.</param>
    /// <param name="pageSize">The page size.</param>
    /// <param name="page">The current page number.</param>
    /// <param name="startItem">The start item number.</param>
    /// <param name="endItem">The end item number.</param>
    /// <param name="hasPrevious">A value indicating whether there is a previous page.</param>
    /// <param name="hasNext">A value indicating whether there is a next page.</param>
    [JsonConstructor]
    public Pagination(int totalItems, int totalPages, int pageSize, int page, int startItem, int endItem, bool hasPrevious, bool hasNext)
    {
        _paginationSettings = default!;
        TotalItems = totalItems;
        TotalPages = totalPages;
        PageSize = pageSize;
        Page = page;
        StartItem = startItem;
        EndItem = endItem;
        HasPrevious = hasPrevious;
        HasNext = hasNext;
    }

    /// <summary>
    /// Gets an pagination instance with zero items.
    /// </summary>
    public static Pagination Empty { get; } = new Pagination(PaginationSettings.Default, 0, null, null);

    /// <summary>
    /// Initializes a new instance of the <see cref="Pagination"/> class with the specified items count and filter.
    /// </summary>
    /// <param name="itemsCount">The total number of items.</param>
    /// <param name="filter">The paging filter.</param>
    public Pagination(int itemsCount, PagingFilter filter)
        : this(PaginationSettings.Default, itemsCount, filter.PageSize, filter.Page)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Pagination"/> class with the specified items count, page size, and page number.
    /// </summary>
    /// <param name="itemsCount">The total number of items.</param>
    /// <param name="pageSize">The page size.</param>
    /// <param name="page">The current page number.</param>
    public Pagination(int itemsCount, int? pageSize, int? page)
        : this(PaginationSettings.Default, itemsCount, pageSize, page)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Pagination"/> class with the specified pagination settings, items count, and filter.
    /// </summary>
    /// <param name="paginationSettings">The pagination settings.</param>
    /// <param name="itemsCount">The total number of items.</param>
    /// <param name="filter">The paging filter.</param>
    public Pagination(PaginationSettings paginationSettings, int itemsCount, PagingFilter filter)
        : this(paginationSettings, itemsCount, filter.PageSize, filter.Page)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Pagination"/> class with the specified pagination settings, items count, page size, and page number.
    /// </summary>
    /// <param name="paginationSettings">The pagination settings.</param>
    /// <param name="itemsCount">The total number of items.</param>
    /// <param name="pageSize">The page size.</param>
    /// <param name="page">The current page number.</param>
    public Pagination(PaginationSettings paginationSettings, int itemsCount, int? pageSize, int? page)
    {
        _paginationSettings = paginationSettings;

        // The order of actions is important
        TotalItems = GetHandledTotalItems(itemsCount);
        PageSize = GetHandledPageSize(_paginationSettings, pageSize);
        TotalPages = GetHandledTotalPages(TotalItems, PageSize);
        Page = GetHandledPage(_paginationSettings, TotalPages, page);

        HasNext = Page != TotalPages;
        HasPrevious = Page != 1;

        StartItem = TotalItems == 0 ? 0 : (PageSize * (Page - 1)) + 1;
        EndItem = (PageSize * Page) > TotalItems ? TotalItems : PageSize * Page;

        Take = PageSize;
        Skip = PageSize * (Page - 1);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int GetHandledTotalItems(int itemsCount)
    {
        return itemsCount < 0 ? 0 : itemsCount;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int GetHandledPageSize(PaginationSettings settings, int? pageSize)
    {
        if (!pageSize.HasValue || pageSize <= 0) return settings.DefaultPageSize;

        if (pageSize > settings.DefaultPageSizeLimit) return settings.DefaultPageSizeLimit;

        return pageSize.Value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int GetHandledTotalPages(int handledTotalItems, int handledPageSize)
    {
        return handledTotalItems == 0 ? 1 : (int)Math.Ceiling((decimal)handledTotalItems / handledPageSize);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int GetHandledPage(PaginationSettings settings, int handledTotalPages, int? page)
    {
        if (!page.HasValue || page <= 0) return settings.DefaultPage;

        if (page.Value > handledTotalPages) return handledTotalPages;

        return page.Value;
    }
}
