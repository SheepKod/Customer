using Microsoft.AspNetCore.Http;

namespace Customer.Application.DTOs;

public class UploadImageDto
{
    public int CustomerId { get; set; }
    public required IFormFile Image { get; set; }
}