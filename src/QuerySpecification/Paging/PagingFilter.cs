namespace Pozitron.QuerySpecification;

/// <summary>
/// Represents a filter for paging.
/// </summary>
public record PagingFilter
{
    /// <summary>
    /// Gets or sets the page number.
    /// </summary>
    public int? Page { get; init; }

    /// <summary>
    /// Gets or sets the page size.
    /// </summary>
    public int? PageSize { get; init; }
}
