namespace Tests.Paging;

public class PagedResponseTests
{
    [Fact]
    public void Constructor_SetDataAndPagination()
    {
        var data = new List<int> { 1, 2, 3 };
        var pagination = new Pagination(1, 10, 3);

        var pagedResponse = new PagedResponse<int>(data, pagination);

        pagedResponse.Data.Should().Equal(data);
        pagedResponse.Pagination.Should().Be(pagination);
    }
}
