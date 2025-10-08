namespace Customer.Application.Dtos;

public class PagingDTO
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}