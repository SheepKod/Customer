namespace Customer.API.ResponseModels;

public class ErrorResponse(int statusCode, string title, string? detail, string type, string instance, Dictionary<string, string[]>? extensions = null)
{
    public string Type { get; set; } = type;
    public int Status { get; set; } = statusCode;
    public string Title { get; set; } = title;
    public string? Detail { get; set; } = detail;
    public Dictionary<string, string[]>? Extensions { get; set; } = extensions;
    public string Instance { get; set; } = instance;
}