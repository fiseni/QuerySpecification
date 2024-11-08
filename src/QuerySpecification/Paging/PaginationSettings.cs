namespace Pozitron.QuerySpecification;

/// <summary>
/// Represents pagination settings.
/// </summary>
public record PaginationSettings
{
    /// <summary>
    /// Gets the default page number.
    /// </summary>
    public int DefaultPage { get; } = 1;

    /// <summary>
    /// Gets the default page size.
    /// </summary>
    public int DefaultPageSize { get; } = 10;

    /// <summary>
    /// Gets the default page size limit.
    /// </summary>
    public int DefaultPageSizeLimit { get; } = 50;

    /// <summary>
    /// Gets the default pagination settings.
    /// </summary>
    public static PaginationSettings Default { get; } = new();

    private PaginationSettings() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="PaginationSettings"/> class with the specified default page size and page size limit.
    /// </summary>
    /// <param name="defaultPageSize">The default page size.</param>
    /// <param name="defaultPageSizeLimit">The default page size limit.</param>
    public PaginationSettings(int defaultPageSize, int defaultPageSizeLimit)
    {
        DefaultPageSize = defaultPageSize;
        DefaultPageSizeLimit = defaultPageSizeLimit;
    }
}
