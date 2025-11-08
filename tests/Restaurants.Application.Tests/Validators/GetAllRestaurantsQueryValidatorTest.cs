using FluentAssertions;
using FluentValidation.TestHelper;
using Restaurants.Application.Queries.Restaurants.GetAllQueries;
using Restaurants.Domain.Constants;

namespace Restaurants.Application.Tests.Validators;

public class GetAllRestaurantsQueryValidatorTest
{
    private readonly GetAllRestaurantsQueryValidator validator;

    public GetAllRestaurantsQueryValidatorTest()
    {
        validator = new GetAllRestaurantsQueryValidator();
    }

    [Theory]
    [InlineData(RestaurantSortByOptions.CreatedBy, SortOrderOptions.Ascending)]
    [InlineData(RestaurantSortByOptions.CreatedBy, SortOrderOptions.Descending)]
    [InlineData(RestaurantSortByOptions.Name, SortOrderOptions.Ascending)]
    [InlineData(RestaurantSortByOptions.Name, SortOrderOptions.Descending)]
    [InlineData(RestaurantSortByOptions.Name, null)]
    [InlineData(RestaurantSortByOptions.CreatedBy, null)]
    [InlineData(null, SortOrderOptions.Descending)]
    [InlineData(null, SortOrderOptions.Ascending)]
    [InlineData(null, null)]
    public void Validator_ValidGetAllRestaurantsQuery_ShouldNotHaveAnyValidationErrors(string? sortBy,string? sortOrder)
    {
        // Arrange
        var query = new GetAllRestaurantsQuery
        {
            PageNumber = 1,
            PageSize = 5,
            SortBy = sortBy,
            SortOrder = sortOrder
        };

        // Act
        var result = validator.TestValidate(query);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData("name")]
    [InlineData("na me")]
    [InlineData("id")]
    [InlineData("createdby")]
    [InlineData("created by")]
    public void Validator_InValidSortByParameter_ShouldHaveValidationError(string sortBy)
    {
        // Arrange 
        var query = new GetAllRestaurantsQuery
        {
            PageNumber = 1,
            PageSize = 5,
            SortBy = sortBy
        };

        // Act
        var result = validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(q => q.SortBy);
    }

    [Theory]
    [InlineData("asc")]
    [InlineData("dsc")]
    [InlineData("ascending")]
    [InlineData("descending")]
    public void Validator_InValidSortOrderParameter_ShouldHaveValidationError(string sortOrder)
    {
        // Arrange 
        var query = new GetAllRestaurantsQuery
        {
            PageNumber = 1,
            PageSize = 5,
            SortOrder = sortOrder
        };

        // Act
        var result = validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(q => q.SortOrder);
    }

    [Theory]
    [InlineData(0,0)]
    [InlineData(-1,-1)]
    [InlineData(null,null)]
    [InlineData(null,0)]
    [InlineData(0,null)]
    public void Validator_InValidPaginationParameters_ShouldHaveValidationError(int? pageNumber, int? pageSize)
    {
        // Arrange
        var query = new GetAllRestaurantsQuery();

        if(pageNumber.HasValue)
            query.PageNumber = pageNumber.Value;

        if(pageSize.HasValue)
            query.PageSize = pageSize.Value;

        // Act
        var result = validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(q => q.PageSize);
        result.ShouldHaveValidationErrorFor(q => q.PageNumber);
    }
}
