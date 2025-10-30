using System.Diagnostics;

namespace Restaurants.API.Middlewares;

public class CustomLogginMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<CustomLogginMiddleware> _logger;

    public CustomLogginMiddleware(RequestDelegate next, ILogger<CustomLogginMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        var stopwatch = Stopwatch.StartNew();
        await _next(httpContext);
        stopwatch.Stop();

        if(stopwatch.ElapsedMilliseconds >= 4000)
        {
            _logger.LogWarning("Request: {method}: {path} tooks {time} which exceeds the threshold."
                ,httpContext.Request.Method
                ,httpContext.Request.Path
                ,stopwatch.ElapsedMilliseconds
            );
        }
    }
}

public static class CustomLogginMiddlewareExtensions
{
    public static IApplicationBuilder UseCustomLogginMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<CustomLogginMiddleware>();
    }
}
