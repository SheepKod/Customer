namespace Customer.Application.Dtos;
/// <summary>
///  amas schirdbea validation??
/// </summary>
/// <typeparam name="T"></typeparam>
public class PagedResult<T>
{
    public int TotalCount { get; set; }
    public List<T> Results { get; set; } = [];
}