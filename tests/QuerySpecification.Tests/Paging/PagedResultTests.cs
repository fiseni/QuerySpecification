namespace Tests.Paging;

public class PagedResultTests
{
    [Fact]
    public void Constructor_SetDataAndPagination()
    {
        var data = new List<int> { 1, 2, 3 };
        var pagination = new Pagination(1, 10, 3);

        var pagedResult = new PagedResult<int>(data, pagination);

        pagedResult.Data.Should().Equal(data);
        pagedResult.Pagination.Should().Be(pagination);
    }
}
