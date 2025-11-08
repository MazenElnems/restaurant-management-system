using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Application.DTOs.Common;
using Restaurants.Application.DTOs.Restaurants;
using Restaurants.Application.Queries.Restaurants.GetAllQueries;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Interfaces;

namespace Restaurants.Application.Tests.Handlers;

public class GetAllRestaurantsQueryHandlerTest
{
    private readonly Mock<IRestaurantsRepository> repositoryMock;
    private readonly Mock<IMapper> mapperMock;
    private readonly Mock<ILogger<GetAllRestaurantsQueryHandler>> loggerMock;
    private readonly GetAllRestaurantsQueryHandler handler;

    public GetAllRestaurantsQueryHandlerTest()
    {
        repositoryMock = new();
        mapperMock = new();
        loggerMock = new();
        handler = new(repositoryMock.Object,mapperMock.Object,loggerMock.Object);
    }

    [Theory]
    [InlineData(1,5)]
    [InlineData(2,5)]
    [InlineData(3,5)]
    public async Task Handle_ValidRequest_ShouldReturnsValidPagedResult(int pageNo,int pageSize)
    {
        // Arrange
        var query = new GetAllRestaurantsQuery
        {
            PageNumber = pageNo,
            PageSize = pageSize,
            SearchString = "a",
            SortBy = RestaurantSortByOptions.CreatedBy,
            SortOrder = SortOrderOptions.Descending
        };

        var restaurants = new List<Restaurant>
        {
            new Restaurant{Id = 1, Name = "Test 1"},
            new Restaurant{Id = 2, Name = "Test 2"},
            new Restaurant{Id = 3, Name = "Test 3"},
            new Restaurant{Id = 4, Name = "Test 4"},
            new Restaurant{Id = 5, Name = "Test 5"},
        };

        repositoryMock.Setup(x => x.GetPagedAsync(query.SearchString, query.SortBy, query.PageNumber, query.PageSize, false))
            .ReturnsAsync((restaurants, 15));

        var dtos = restaurants.Select(x => new GetAllRestaurantsDto
        {
            Id = x.Id,
            Name = x.Name,
        }).ToList();

        mapperMock.Setup(x => x.Map<List<GetAllRestaurantsDto>>(restaurants))
            .Returns(dtos);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeOfType<PagedResult<GetAllRestaurantsDto>>();
        result.Items.Should().ContainInOrder(dtos);
        result.Items.Count().Should().BeLessThanOrEqualTo(pageSize);
        result.NumberOfPages.Should().Be(3);
        result.ResultsCount.Should().Be(15);
    }

    [Fact]
    public async Task Handle_ExceptionThrown_ShouldThrowsException()
    {
        // Arrange
        var query = new GetAllRestaurantsQuery();

        var exception = new Exception();

        repositoryMock.Setup(x => x.GetPagedAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), false))
            .ThrowsAsync(exception);

        // Act
        var func = async () => await handler.Handle(query, CancellationToken.None);

        // Assert 
        await func.Should().ThrowAsync<Exception>();
    } 

}
