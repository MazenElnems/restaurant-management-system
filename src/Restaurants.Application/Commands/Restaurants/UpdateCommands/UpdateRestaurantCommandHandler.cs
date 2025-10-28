using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.CustomExceptions;
using Restaurants.Domain.Entities;
using Restaurants.Domain.RepositoryInterfaces;

namespace Restaurants.Application.Commands.Restaurants.UpdateCommands;

public class UpdateRestaurantCommandHandler : IRequestHandler<UpdateRestaurantCommand>
{
    private readonly IRestaurantsRepository _restaurantsRepository;
    private readonly ILogger<UpdateRestaurantCommandHandler> _logger;

    public UpdateRestaurantCommandHandler(IRestaurantsRepository restaurantsRepository, ILogger<UpdateRestaurantCommandHandler> logger)
    {
        _restaurantsRepository = restaurantsRepository;
        _logger = logger;
    }

    public async Task Handle(UpdateRestaurantCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var restaurant = await _restaurantsRepository.GetByIdAsync(request.Id)
                ?? throw new ResourseNotFoundException("Restaurant", request.Id.ToString());

            restaurant.Name = request.Name;
            restaurant.Description = request.Description;
            restaurant.ContactEmail = request.ContactEmail;
            restaurant.ContactNumber = request.ContactNumber;
            restaurant.HasDelivery = request.HasDelivery;
            restaurant.Address = new Address { City = request.City, Street = request.Street, PostalCode = request.PostalCode };

            await _restaurantsRepository.CommitAsync();
        }
        catch (ResourseNotFoundException ex)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating restaurant ID: {RestaurantId}", request.Id);
            throw;
        }
    }
}

