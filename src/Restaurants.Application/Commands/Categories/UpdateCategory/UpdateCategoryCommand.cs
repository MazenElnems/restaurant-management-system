using MediatR;
using System.Text.Json.Serialization;

namespace Restaurants.Application.Commands.Categories.UpdateCategory;

public class UpdateCategoryCommand : IRequest
{
    [JsonIgnore]
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    [JsonIgnore]
    public int RestaurantId { get; set; }
}
