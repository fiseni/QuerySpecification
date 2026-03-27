namespace Pozitron.QuerySpecification;

/// <summary>
/// Represents a filter for paging.
/// </summary>
public interface IPagingFilter
{
    /// <summary>
    /// Gets the page number.
    /// </summary>
    int? Page { get; }

    /// <summary>
    /// Gets the page size.
    /// </summary>
    int? PageSize { get; }
}
