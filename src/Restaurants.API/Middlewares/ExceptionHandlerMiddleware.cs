using Restaurants.Application.CustomExceptions;

namespace Restaurants.API.Middlewares;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlerMiddleware> _logger;

    public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch(ResourseNotFoundException ex)
        {
            _logger.LogWarning("ResourseNotFoundException caught in exception handler middleware: {ExceptionMessage}", ex.Message);
            httpContext.Response.StatusCode = 404;
            await httpContext.Response.WriteAsync(ex.Message);
        }
        catch(Exception ex)
        {
            _logger.LogError("Exception caught in exception handler middleware: {ExceptionMessage}", ex.Message);
            httpContext.Response.StatusCode = 500;
            await httpContext.Response.WriteAsync("something went wrong");
        }
    }
}

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionHandlerMiddleware>();
    }
}
