using Backend.Infrastructure.Middleware;

namespace Backend.Config.Extensions;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseGlobalErrorHandling(this IApplicationBuilder app)
    {
        return app.UseMiddleware<GlobalErrorHandlingMiddleware>();
    }
} 