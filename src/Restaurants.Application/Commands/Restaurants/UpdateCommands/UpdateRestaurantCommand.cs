using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Restaurants.Application.Commands.Restaurants.UpdateCommands;

public class UpdateRestaurantCommand : IRequest
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public bool HasDelivery { get; set; }
    public string? ContactEmail { get; set; }
    [Phone(ErrorMessage = "Contact number is not a valid phone number (include country/area code if required).")]
    public string? ContactNumber { get; set; }
    public string City { get; set; }
    public string Street { get; set; }
    public string PostalCode { get; set; }
}
