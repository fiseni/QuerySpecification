namespace Tests.Paging;

public class PaginationTests
{
    private const int _defaultPage = 1;
    private const int _defaultPageSize = 10;
    private const int _defaultPageSizeLimit = 50;
    private static readonly PaginationSettings _settings = new(_defaultPageSize, _defaultPageSizeLimit);

    public record Expected(int totalItems, int totalPages, int pageSize, int page, int startItem, int endItem, bool hasPrevious, bool hasNext, int take, int skip);
    public static IEnumerable<object?[]> TheoryData =>
        [
            [0, null, null,     new Expected(0, 1, 10, 1, 0, 0, false, false, 10, 0)],
            [50, 500, 500,      new Expected(50, 1, 50, 1, 1, 50, false, false, 50, 0)],
            [50, 10, 1,         new Expected(50, 5, 10, 1, 1, 10, false, true, 10, 0)],
            [50, 10, 3,         new Expected(50, 5, 10, 3, 21, 30, true, true, 10, 20)],
        ];

    private static void AssertPaginationValues(Pagination pagination, Expected expected)
    {
        pagination.TotalItems.Should().Be(expected.totalItems);
        pagination.TotalPages.Should().Be(expected.totalPages);
        pagination.PageSize.Should().Be(expected.pageSize);
        pagination.Page.Should().Be(expected.page);
        pagination.StartItem.Should().Be(expected.startItem);
        pagination.EndItem.Should().Be(expected.endItem);
        pagination.HasPrevious.Should().Be(expected.hasPrevious);
        pagination.HasNext.Should().Be(expected.hasNext);
        pagination.Take.Should().Be(expected.take);
        pagination.Skip.Should().Be(expected.skip);
    }

    [Theory]
    [MemberData(nameof(TheoryData))]
    public void CalculatesValues_GivenMostInnerConstructor(int itemsCountInput, int? pageSizeInput, int? pageInput, Expected expected)
    {
        var pagination = new Pagination(_settings, itemsCountInput, pageSizeInput, pageInput);
        AssertPaginationValues(pagination, expected);
    }

    [Theory]
    [MemberData(nameof(TheoryData))]
    public void CalculatesValues_GivenConstructorWithPagingFilter(int itemsCountInput, int? pageSizeInput, int? pageInput, Expected expected)
    {
        var filter = new PagingFilter { PageSize = pageSizeInput, Page = pageInput };
        var pagination = new Pagination(_settings, itemsCountInput, filter);
        AssertPaginationValues(pagination, expected);
    }

    [Theory]
    [MemberData(nameof(TheoryData))]
    public void CalculatesValues_GivenConstructorWithDefaultSettings(int itemsCountInput, int? pageSizeInput, int? pageInput, Expected expected)
    {
        var pagination = new Pagination(itemsCountInput, pageSizeInput, pageInput);
        AssertPaginationValues(pagination, expected);
    }

    [Theory]
    [MemberData(nameof(TheoryData))]
    public void CalculatesValues_GivenConstructorWithDefaultSettingsAndPagingFilter(int itemsCountInput, int? pageSizeInput, int? pageInput, Expected expected)
    {
        var filter = new PagingFilter { PageSize = pageSizeInput, Page = pageInput };
        var pagination = new Pagination(itemsCountInput, filter);
        AssertPaginationValues(pagination, expected);
    }

    [Theory]
    [InlineData(0, 1, 10, 1, 0, 0, false, false)]
    [InlineData(50, 1, 50, 1, 1, 50, true, false)]
    [InlineData(50, 5, 10, 1, 1, 10, false, true)]
    [InlineData(50, 5, 10, 3, 21, 30, true, true)]
    public void CalculatesValues_GivenJsonConstructor(int totalItems, int totalPages, int pageSize, int page, int startItem, int endItem, bool hasPrevious, bool hasNext)
    {
        var pagination = new Pagination(totalItems, totalPages, pageSize, page, startItem, endItem, hasPrevious, hasNext);
        AssertPaginationValues(pagination, new Expected(totalItems, totalPages, pageSize, page, startItem, endItem, hasPrevious, hasNext, 0, 0));
    }

    [Fact]
    public void DefaultPagination_SetsDefaultValues()
    {
        var pagination = Pagination.Default;
        var pageSize = PaginationSettings.Default.DefaultPageSize;
        AssertPaginationValues(pagination, new Expected( 0, 1, pageSize, 1, 0, 0, false, false, pageSize, 0));
    }

    [Fact]
    public void TotalItems_ReturnsZero_GivenItemsCountZero()
    {
        var itemsCount = 0;

        var pagination = new Pagination(_settings, itemsCount, 1, 1);

        pagination.TotalItems.Should().Be(0);
    }

    [Fact]
    public void TotalItems_ReturnsZero_GivenItemsCountNegative()
    {
        var itemsCount = -1;

        var pagination = new Pagination(_settings, itemsCount, 1, 1);

        pagination.TotalItems.Should().Be(0);
    }

    [Fact]
    public void TotalItems_ReturnsInput_GivenItemsCountPositive()
    {
        var itemsCount = 1;

        var pagination = new Pagination(_settings, itemsCount, 1, 1);

        pagination.TotalItems.Should().Be(itemsCount);
    }

    [Fact]
    public void PageSize_ReturnsDefault_GivenPageSizeNull()
    {
        int? pageSize = null;

        var pagination = new Pagination(_settings, 100, pageSize, 1);

        pagination.PageSize.Should().Be(_defaultPageSize);
    }

    [Fact]
    public void PageSize_ReturnsDefault_GivenPageSizeZero()
    {
        int? pageSize = 0;

        var pagination = new Pagination(_settings, 100, pageSize, 1);

        pagination.PageSize.Should().Be(_defaultPageSize);
    }

    [Fact]
    public void PageSize_ReturnsDefault_GivenPageSizeNegative()
    {
        int? pageSize = -1;

        var pagination = new Pagination(_settings, 100, pageSize, 1);

        pagination.PageSize.Should().Be(_defaultPageSize);
    }

    [Fact]
    public void PageSize_ReturnsDefaultLimit_GivenPageSizeOverLimit()
    {
        int? pageSize = 500;

        var pagination = new Pagination(_settings, 100, pageSize, 1);

        pagination.PageSize.Should().Be(_defaultPageSizeLimit);
    }

    [Fact]
    public void PageSize_ReturnsInput_GivenPageSizePositive()
    {
        int? pageSize = 2;

        var pagination = new Pagination(_settings, 100, pageSize, 1);

        pagination.PageSize.Should().Be(pageSize);
    }

    [Theory]
    [InlineData(0, 10, 1)]
    [InlineData(50, 10, 5)]
    [InlineData(55, 10, 6)]
    [InlineData(50, 100, 1)]
    [InlineData(9, 4, 3)]
    [InlineData(5, 2, 3)]
    [InlineData(8, 3, 3)]
    public void TotalPages_GivenItemsCountAndPageSize(int itemsCountInput, int? pageSizeInput, int totalPages)
    {
        var pagination = new Pagination(_settings, itemsCountInput, pageSizeInput, 1);

        pagination.TotalPages.Should().Be(totalPages);
    }

    [Theory]
    [InlineData(50, 10, null, _defaultPage)]
    [InlineData(50, 10, 0, _defaultPage)]
    [InlineData(50, 10, -1, _defaultPage)]
    [InlineData(50, 10, 100, 5)]
    [InlineData(50, 10, 2, 2)]
    public void Page_GivenInputs(int itemsCountInput, int? pageSizeInput, int? pageInput, int page)
    {
        var pagination = new Pagination(_settings, itemsCountInput, pageSizeInput, pageInput);

        pagination.Page.Should().Be(page);
    }

    [Theory]
    [InlineData(10, 10)]
    [InlineData(-1, _defaultPageSize)]
    public void Take_GivenPageSize(int? pageSize, int take)
    {
        var pagination = new Pagination(_settings, 100, pageSize, 1);

        pagination.Take.Should().Be(take);
    }

    [Theory]
    [InlineData(50, 10, 3, 20)]
    [InlineData(50, 10, 1, 0)]
    public void Skip_GivenPageSize(int itemsCountInput, int? pageSizeInput, int? pageInput, int skip)
    {
        var pagination = new Pagination(_settings, itemsCountInput, pageSizeInput, pageInput);

        pagination.Skip.Should().Be(skip);
    }
}
