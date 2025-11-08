using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.API.Middleware;
using Restaurants.Application.CustomExceptions;
using System;

namespace Restaurants.API.Tests.MiddlewareTests;

public class ExceptionHandlerMiddlewareTest
{
    private readonly Mock<RequestDelegate> requestDelegateMock;
    private readonly Mock<ILogger<ExceptionHandlerMiddleware>> loggerMock;
    private readonly ExceptionHandlerMiddleware middleware;

    public ExceptionHandlerMiddlewareTest()
    {
        requestDelegateMock = new Mock<RequestDelegate>();
        loggerMock = new Mock<ILogger<ExceptionHandlerMiddleware>>();
        middleware = new ExceptionHandlerMiddleware(requestDelegateMock.Object, loggerMock.Object);
    }

    [Fact]
    public async Task Invoke_UnAuthorizedExceptionThrown_ShouldSetStatusCodeTo403()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        var exception = new UnAuthorizedException();

        requestDelegateMock.Setup(x => x.Invoke(httpContext))
            .ThrowsAsync(exception);

        // Act
        await middleware.Invoke(httpContext);

        // Assert
        httpContext.Response.StatusCode.Should().Be(403);

        loggerMock.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("UnAuthorizedException")),
                exception,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task Invoke_ResourseNotFoundExceptionThrown_ShouldSetStatusCodeTo404()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        var exception = new ResourseNotFoundException("","");

        requestDelegateMock.Setup(x => x.Invoke(httpContext))
            .ThrowsAsync(exception);

        // Act
        await middleware.Invoke(httpContext);

        // Assert
        httpContext.Response.StatusCode.Should().Be(404);

        loggerMock.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("ResourseNotFoundException")),
                exception,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task Invoke_ExceptionThrown_ShouldSetStatusCodeTo500()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        var exception = new Exception();

        requestDelegateMock.Setup(x => x.Invoke(httpContext))
            .ThrowsAsync(exception);

        // Act
        await middleware.Invoke(httpContext);

        // Assert
        httpContext.Response.StatusCode.Should().Be(500);

        loggerMock.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Exception")),
                exception,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task Invoke_NoExceptionThrown_ShouldInvokeRequestDelegate()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();

        requestDelegateMock.Setup(x => x.Invoke(httpContext))
            .Returns(Task.CompletedTask);

        // Act
        await middleware.Invoke(httpContext);

        // Assert
        requestDelegateMock.Verify(x => x.Invoke(httpContext), Times.Once());
    }
}
