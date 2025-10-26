using MediatR;

namespace Restaurants.Application.Commands.Categories.DeleteCategory;

public class DeleteCategoryCommand(int id,int restaurantId) : IRequest
{
    public int Id { get; init; } = id;
    public int RestaurantId { get; init; } = restaurantId;
}
