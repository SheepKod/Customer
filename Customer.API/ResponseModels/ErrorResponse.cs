namespace Customer.API.ResponseModels;

public class ErrorResponse(int statusCode, string message, string? details = null, string? path = null)
{
    public int StatusCode { get; set; } = statusCode;
    public string Message { get; set; } = message;
    public string? Details { get; set; } = details;
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string? Path { get; set; } = path;
}