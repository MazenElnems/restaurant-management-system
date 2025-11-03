using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using Restaurants.Application.Users;
using Restaurants.Domain.Constants;
using System.Security.Claims;

namespace Restaurants.Application.Tests.ApplicationLogicTest;

public class UserContextTest
{
    private Mock<IHttpContextAccessor> httpContextAccessorMock;
    private IUserContext userContext;

    public UserContextTest()
    {
        httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        userContext = new UserContext(httpContextAccessorMock.Object);
    }

    [Fact]
    public void GetCurrentUser_WithValidUserClaims_ShouldReturnsValidCurrentUserIdentity()
    {
        // Arrange
        var claims = new Claim[]
        {
            new(ClaimTypes.NameIdentifier,"1"),
            new(ClaimTypes.Email,"test@gmail.com"),
            new(ClaimTypes.Role,UserRoles.Admin),
            new("DateOfBirth","2005-01-10"),
            new("Nationality","Egyptian")
        };

        var cp = new ClaimsPrincipal(new ClaimsIdentity(claims, "Test"));

        httpContextAccessorMock.Setup(x => x.HttpContext).Returns(new DefaultHttpContext
        {
            User = cp
        });

        // Act
        var currentUser = userContext.GetCurrentUser();

        // Assert
        currentUser.Should().NotBeNull();
        currentUser.Id.Should().Be(1);
        currentUser.Email.Should().Be("test@gmail.com");
        currentUser.Roles.Contains(UserRoles.Admin).Should().BeTrue();
        currentUser.DateOfBirth.Should().Be(new DateOnly(2005, 1, 10));
        currentUser.Nationality.Should().Be("Egyptian");
    }


    [Fact]
    public void GetCurrentUser_WithNullUser_ShouldReturnsNull()
    {
        // Arrange
        httpContextAccessorMock.Setup(x => x.HttpContext).Returns(new DefaultHttpContext
        {
            User = null
        });

        // Act
        var currentUser = userContext.GetCurrentUser();

        // Assert
        currentUser.Should().BeNull();
    }
}
