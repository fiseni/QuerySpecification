namespace Pozitron.QuerySpecification;

public record PagingFilter
{
    public int? Page { get; set; }
    public int? PageSize { get; set; }
}
