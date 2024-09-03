namespace Pozitron.QuerySpecification;

public class PaginationSettings
{
    public int DefaultPage { get; } = 1;
    public int DefaultPageSize { get; } = 10;
    public int DefaultPageSizeLimit { get; } = 50;

    public static PaginationSettings Default { get; } = new();
    private PaginationSettings() { }

    public PaginationSettings(int defaultPageSize, int defaultPageSizeLimit)
    {
        DefaultPageSize = defaultPageSize;
        DefaultPageSizeLimit = defaultPageSizeLimit;
    }
}
