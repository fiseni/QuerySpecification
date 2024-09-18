namespace Pozitron.QuerySpecification;

public record PagedResponse<T>
{
    public Pagination Pagination { get; }
    public List<T> Data { get; }

    public PagedResponse(List<T> data, Pagination pagination)
    {
        Data = data;
        Pagination = pagination;
    }
}
