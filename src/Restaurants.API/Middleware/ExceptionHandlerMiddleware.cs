using Azure.Core;
using Restaurants.Application.CustomExceptions;

namespace Restaurants.API.Middleware;

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
        catch(UnAuthorizedException ex)
        {
            _logger.LogError(ex,"UnAuthorizedException caught in exception handler middleware: {ExceptionMessage}", ex.Message);
            httpContext.Response.StatusCode = 403;
            await httpContext.Response.WriteAsync(ex.Message);
        }
        catch(InvalidOperationException ex)
        {
            _logger.LogWarning(ex,"InvalidOperationException caught in exception handler middleware: {ExceptionMessage}", ex.Message);
            httpContext.Response.StatusCode = 400;
            await httpContext.Response.WriteAsync(ex.Message);
        }
        catch (ResourseNotFoundException ex)
        {
            _logger.LogWarning(ex,"ResourseNotFoundException caught in exception handler middleware: {ExceptionMessage}", ex.Message);
            httpContext.Response.StatusCode = 404;
            await httpContext.Response.WriteAsync(ex.Message);
        }
        catch(Exception ex)
        {
            _logger.LogError(ex,"Exception caught in exception handler middleware: {ExceptionMessage}", ex.Message);
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
