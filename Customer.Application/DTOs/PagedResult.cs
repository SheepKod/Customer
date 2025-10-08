namespace Customer.Application.Dtos;

public class PagedResult<T>
{
    public int TotalCount { get; set; }
    public List<T> Results { get; set; } = [];
}