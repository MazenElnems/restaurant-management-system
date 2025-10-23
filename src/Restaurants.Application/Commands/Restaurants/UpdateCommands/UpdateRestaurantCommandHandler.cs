using AutoMapper;
using MediatR;
using Restaurants.Domain.Entities;
using Restaurants.Domain.RepositoryInterfaces;

namespace Restaurants.Application.Commands.Restaurants.UpdateCommands;

public class UpdateRestaurantCommandHandler : IRequestHandler<UpdateRestaurantCommand, bool>
{
    private readonly IRestaurantsRepository _restaurantsRepository;
    private readonly IMapper _mapper;

    public UpdateRestaurantCommandHandler(IMapper mapper, IRestaurantsRepository restaurantsRepository)
    {
        _mapper = mapper;
        _restaurantsRepository = restaurantsRepository;
    }

    public async Task<bool> Handle(UpdateRestaurantCommand request, CancellationToken cancellationToken)
    {
        var restaurant = await _restaurantsRepository.GetByIdAsync(request.Id);

        if (restaurant is null)
            return false;

        restaurant.Name = request.Name;
        restaurant.Description = request.Description;
        restaurant.ContactEmail = request.ContactEmail;
        restaurant.ContactNumber = request.ContactNumber;
        restaurant.HasDelivery = request.HasDelivery;
        restaurant.Address = new Address { City = request.City, Street = request.Street, PostalCode = request.PostalCode };

        await _restaurantsRepository.CommitAsync();

        return true;
    }
}

