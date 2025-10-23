using MediatR;
using Restaurants.Domain.RepositoryInterfaces;

namespace Restaurants.Application.Commands.Restaurants.DeleteCommands;

public class DeleteRestaurantCommandHandler : IRequestHandler<DeleteRestaurantCommand, bool>
{
    private readonly IRestaurantsRepository _restaurantsRepository;

    public DeleteRestaurantCommandHandler(IRestaurantsRepository restaurantsRepository)
    {
        _restaurantsRepository = restaurantsRepository;
    }

    public async Task<bool> Handle(DeleteRestaurantCommand request, CancellationToken cancellationToken)
    {
        var restaurant = await _restaurantsRepository.GetByIdAsync(request.Id);

        if (restaurant is null)
            return false;

        int rowsAffected = await _restaurantsRepository.DeleteAsync(restaurant);
        return rowsAffected > 0;
    }
}
