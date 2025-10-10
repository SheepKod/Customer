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
            logger.LogError(ex.Message);
            await HandleExceptionAsync(context, ex, 404);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            
            await HandleExceptionAsync(context, ex, 500);
        }
    }
    
    private static async Task HandleExceptionAsync(HttpContext context, Exception exception, int statusCode)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        var errorResponse = new ErrorResponse(
            statusCode: statusCode,
            message: exception.Message,
            path: context.Request.Path
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