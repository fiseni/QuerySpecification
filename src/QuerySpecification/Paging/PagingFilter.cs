namespace Pozitron.QuerySpecification;

public record PagingFilter
{
    public int? Page { get; init; }
    public int? PageSize { get; init; }
}
