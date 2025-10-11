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
        catch (DuplicationException ex)
        {
            logger.LogError(ex.Message, ex);
            await HandleExceptionAsync(context, "Resource Conflict", ex.Message, ex.GetType().Name,
                StatusCodes.Status409Conflict);
        }
        catch (NotFoundException ex)
        {
            logger.LogError(ex.Message, ex);
            await HandleExceptionAsync(context, "Resource Not Found", ex.Message, ex.GetType().Name,
                StatusCodes.Status400BadRequest);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message, ex);

            await HandleExceptionAsync(context, "Internal Server Error", null, ex.GetType().Name,
                StatusCodes.Status500InternalServerError);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, string title, string? detail, string type,
        int statusCode)
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