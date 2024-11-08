namespace Pozitron.QuerySpecification;

/// <summary>
/// Represents a paged result with data and pagination information.
/// </summary>
/// <typeparam name="T">The type of the data.</typeparam>
public record PagedResult<T>
{
    /// <summary>
    /// Gets the pagination information.
    /// </summary>
    public Pagination Pagination { get; }

    /// <summary>
    /// Gets the data.
    /// </summary>
    public List<T> Data { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PagedResult{T}"/> class.
    /// </summary>
    /// <param name="data">The data.</param>
    /// <param name="pagination">The pagination information.</param>
    public PagedResult(List<T> data, Pagination pagination)
    {
        Data = data;
        Pagination = pagination;
    }
}
