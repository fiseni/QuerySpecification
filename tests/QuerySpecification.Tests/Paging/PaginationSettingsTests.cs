namespace QuerySpecification.Tests.Paging;

public class PaginationSettingsTests
{
    [Fact]
    public void Default_Values()
    {
        var settings = PaginationSettings.Default;

        settings.DefaultPage.Should().Be(1);
        settings.DefaultPageSize.Should().Be(10);
        settings.DefaultPageSizeLimit.Should().Be(50);
    }

    [Fact]
    public void Custom_Values()
    {
        var settings = new PaginationSettings(5, 100);

        settings.DefaultPage.Should().Be(1);
        settings.DefaultPageSize.Should().Be(5);
        settings.DefaultPageSizeLimit.Should().Be(100);
    }
}
