using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Restaurants.Application.Commands.Restaurants.CraeteCommands;

public class CreateRestaurantCommand : IRequest<int>
{

    [Required(ErrorMessage = "Restaurant name is required.")]
    [StringLength(30, ErrorMessage = "Restaurant name must not exceed 30 characters.")]
    public string Name { get; set; }

    public string? Description { get; set; }

    [Required(ErrorMessage = "Please specify whether the restaurant offers delivery.")]
    public bool HasDelivery { get; set; }

    [EmailAddress(ErrorMessage = "Contact email is not a valid email address (example: user@domain.com).")]
    public string? ContactEmail { get; set; }

    [Phone(ErrorMessage = "Contact number is not a valid phone number (include country/area code if required).")]
    [StringLength(11, ErrorMessage = "Please enter a valid phone number with 11 Digit.")]
    public string? ContactNumber { get; set; }

    [Required(ErrorMessage = "City is required.")]
    public string City { get; set; }

    [Required(ErrorMessage = "Street is required.")]
    public string Street { get; set; }

    [Required(ErrorMessage = "Postal code is required.")]
    [RegularExpression(@"^\d{5}$", ErrorMessage = "Postal code must be exactly 5 digits (example: 12345).")]
    public string PostalCode { get; set; }
}
