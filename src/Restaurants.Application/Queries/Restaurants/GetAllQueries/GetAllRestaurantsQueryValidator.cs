using FluentValidation;
using Restaurants.Domain.Constants;

namespace Restaurants.Application.Queries.Restaurants.GetAllQueries;

public class GetAllRestaurantsQueryValidator : AbstractValidator<GetAllRestaurantsQuery>
{
    private readonly string[] allowedSortByOptions = [RestaurantSortByOptions.Name, RestaurantSortByOptions.CreatedBy];
    private readonly string[] allowedSortOrderOption = [SortOrderOptions.Descending, SortOrderOptions.Ascending];
    private readonly int[] allowedPageSizes = [5, 10, 15, 30];

    public GetAllRestaurantsQueryValidator()
    {
        RuleFor(q => q.SortOrder)
            .Must(s => allowedSortOrderOption.Contains(s))
            .WithMessage($"SortOrder must be one of the following options ({string.Join(",",allowedSortOrderOption)})");

        RuleFor(q => q.SortBy)
            .Must(s => allowedSortByOptions.Contains(s))
            .WithMessage($"SortBy must be one of the following options ({string.Join(",",allowedSortByOptions)})");

        RuleFor(q => q.PageSize)
            .NotEmpty()
            .WithMessage("PageSize is required.")
            .Must(p => allowedPageSizes.Contains(p))
            .WithMessage($"PageSize must be one of the following options ({string.Join(",", allowedPageSizes)})");

        RuleFor(q => q.PageNumber)
            .NotEmpty()
            .WithMessage("PageNumber is required.")
            .GreaterThan(0)
            .WithMessage("PageNumber must be greater than 0");
    }
}