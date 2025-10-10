using System.Text.Json;
using Customer.Application.Exceptions;
using Customer.API.ResponseModels;

namespace Customer.API.Middlewares;

public class GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger)
{

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (NotFoundException ex)
        {
            logger.LogError(ex.Message, ex);
            await HandleExceptionAsync(context, "Resource Not Found", ex.Message,ex.GetType().Name,404);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message, ex);
            
            await HandleExceptionAsync(context, "Internal Server Error",  null,ex.GetType().Name,500);
        }
    }
    
    private static async Task HandleExceptionAsync(HttpContext context, string title, string? detail, string type, int statusCode)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        var errorResponse = new ErrorResponse(
            statusCode,
            title,
            detail,
            instance: context.Request.Path.ToString(),
            type: type
        );

        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        var jsonResponse = JsonSerializer.Serialize(errorResponse, jsonOptions);
        await context.Response.WriteAsync(jsonResponse);
    }
}