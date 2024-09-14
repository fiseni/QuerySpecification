using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace Pozitron.QuerySpecification;

public record Pagination
{
    private readonly PaginationSettings _paginationSettings;

    public int TotalItems { get; }
    public int TotalPages { get; }
    public int PageSize { get; }
    public int Page { get; }
    public int StartItem { get; }
    public int EndItem { get; }
    public bool HasPrevious { get; }
    public bool HasNext { get; }

    [JsonIgnore]
    public int Take { get; }
    [JsonIgnore]
    public int Skip { get; }

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

    public static Pagination Default { get; } = new Pagination(PaginationSettings.Default, 0, null, null);

    public Pagination(int itemsCount, PagingFilter filter)
        : this(PaginationSettings.Default, itemsCount, filter.PageSize, filter.Page)
    {
    }

    public Pagination(int itemsCount, int? pageSize, int? page)
        : this(PaginationSettings.Default, itemsCount, pageSize, page)
    {
    }

    public Pagination(PaginationSettings paginationSettings, int itemsCount, PagingFilter filter)
        : this(paginationSettings, itemsCount, filter.PageSize, filter.Page)
    {
    }

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
