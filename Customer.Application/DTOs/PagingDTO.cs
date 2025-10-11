namespace Customer.Application.Dtos;

// TODO: Add Validation
public class PagingDTO
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}