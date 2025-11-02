using FluentValidation;

namespace Restaurants.Application.Commands.Restaurants.UpdateCommands;

public class UpdateRestaurantCommandValidator : AbstractValidator<UpdateRestaurantCommand>
{
    public UpdateRestaurantCommandValidator()
    {
        RuleFor(r => r.Name)
            .MaximumLength(20)
            .WithMessage("Restaurant name must not exceed 30 characters.");

        RuleFor(r => r.HasDelivery)
            .NotEmpty()
            .WithMessage("Please specify whether the restaurant offers delivery.");

        RuleFor(r => r.ContactEmail)
            .EmailAddress()
            .WithMessage("Contact email is not a valid email address (example: user@domain.com).");

        RuleFor(r => r.ContactNumber)
            .MaximumLength(11)
            .WithMessage("Please enter a valid phone number with 11 Digit.");

        RuleFor(r => r.PostalCode)
           .Matches("@\"^\\d{5}$\"")
           .WithMessage("Postal code must be exactly 5 digits (example: 12345).");
    }
}
