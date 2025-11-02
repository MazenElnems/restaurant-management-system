using MediatR;
using System.Text.Json.Serialization;

namespace Restaurants.Application.Commands.Categories.CreateCategory;

public class CreateCategoryCommand : IRequest<int>
{
    public string Name { get; set; }
    public string? Description { get; set; }
    [JsonIgnore]
    public int RestaurantId { get; set; }
}
