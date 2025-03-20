using Backend.Shared.Exceptions;
using System.Net;
using System.Text.Json;

namespace Backend.Infrastructure.Middleware;

public class GlobalErrorHandlingMiddleware(RequestDelegate next, ILogger<GlobalErrorHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = context.Response;
        response.ContentType = "application/json";

        response.StatusCode = exception switch
        {
            NotFoundError => (int)HttpStatusCode.NotFound,
            ArgumentException => (int)HttpStatusCode.BadRequest,
            _ => (int)HttpStatusCode.InternalServerError
        };

        var result = JsonSerializer.Serialize(new
        {
            message = exception.Message,
            statusCode = response.StatusCode
        });

        logger.LogError(exception, "An error occurred: {Message}", exception.Message);
        await response.WriteAsync(result);
    }
} 