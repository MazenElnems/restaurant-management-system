using FluentAssertions;
using Restaurants.Application.Users;
using Restaurants.Domain.Constants;
using System.Data;

namespace Restaurants.Application.Tests.ApplicationLogicTest;

public class CurrentUserIdentityTest
{
    [Theory]
    [InlineData(UserRoles.Admin)]
    [InlineData(UserRoles.Owner)]
    public void IsInRole_WithSomeRoles_ShouldReturnsTrue(string role)
    {
        // Arrange
        var currentUserIdentity = new CurrentUserIdentity(1, "test@gmail.com", [UserRoles.Admin, UserRoles.Owner], null, null);

        // Act
        bool isInRole = currentUserIdentity.IsInRole(role);

        // Assert
        isInRole.Should().BeTrue();
    }

    [Theory]
    [InlineData(UserRoles.Admin)]
    [InlineData(UserRoles.Owner)]
    [InlineData(UserRoles.Staff)]
    public void IsInRole_WithEmptyRoles_ShouldReturnsFalse(string role)
    {
        // Arrange
        var currentUserIdentity = new CurrentUserIdentity(1, "test@gmail.com",[], null, null);

        // Act
        bool isInRole = currentUserIdentity.IsInRole(role);

        // Assert
        isInRole.Should().BeFalse();
    }

    [Fact]
    public void IsInRole_InValidRoleName_ShouldReturnsFalse()
    {
        // Arrange
        var currentUserIdentity = new CurrentUserIdentity(1, "test@gmail.com", [UserRoles.Admin], null, null);

        // Act
        bool isInRole = currentUserIdentity.IsInRole(UserRoles.Admin.ToLower());

        // Assert
        isInRole.Should().BeFalse();
    }
}
