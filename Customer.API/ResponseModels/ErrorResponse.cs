namespace Customer.API.ResponseModels;

public class ErrorResponse(int statusCode, string title, string detail, string type, string instance)
{
    public string Type { get; set; } = type;
    public int Status { get; set; } = statusCode;
    public string Title { get; set; } = title;
    public string? Detail { get; set; } = detail;
    public string Instance { get; set; } = instance;
}