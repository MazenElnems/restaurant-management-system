using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.CustomExceptions;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Enums;
using Restaurants.Domain.Interfaces;

namespace Restaurants.Application.Commands.Restaurants.UpdateCommands;

public class UpdateRestaurantCommandHandler : IRequestHandler<UpdateRestaurantCommand>
{
    private readonly IRestaurantsRepository _restaurantsRepository;
    private readonly ILogger<UpdateRestaurantCommandHandler> _logger;
    private readonly IRestaurantAuthorizationService _authorizationService;

    public UpdateRestaurantCommandHandler(IRestaurantsRepository restaurantsRepository, ILogger<UpdateRestaurantCommandHandler> logger, IRestaurantAuthorizationService authorizationService)
    {
        _restaurantsRepository = restaurantsRepository;
        _logger = logger;
        _authorizationService = authorizationService;
    }

    public async Task Handle(UpdateRestaurantCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var restaurant = await _restaurantsRepository.GetByIdAsync(request.Id)
                ?? throw new ResourseNotFoundException("Restaurant", request.Id.ToString());

            if(!_authorizationService.Authorize(restaurant,RestaurantOperation.Update))
                throw new UnAuthorizedException("User is not authorized to update this restaurant.");

            restaurant.Name = request.Name;
            restaurant.Description = request.Description;
            restaurant.ContactEmail = request.ContactEmail;
            restaurant.ContactNumber = request.ContactNumber;
            restaurant.HasDelivery = request.HasDelivery;
            restaurant.Address = new Address { City = request.City, Street = request.Street, PostalCode = request.PostalCode };

            await _restaurantsRepository.CommitAsync();
        }
        catch(UnAuthorizedException ex)
        {
            throw;
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

