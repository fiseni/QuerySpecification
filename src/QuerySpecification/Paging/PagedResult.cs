namespace Pozitron.QuerySpecification;

public record PagedResult<T>
{
    public Pagination Pagination { get; }
    public List<T> Data { get; }

    public PagedResult(List<T> data, Pagination pagination)
    {
        Data = data;
        Pagination = pagination;
    }
}
