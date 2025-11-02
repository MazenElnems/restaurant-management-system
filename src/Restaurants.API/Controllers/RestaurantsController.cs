using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurants.API.Authorization.Constants;
using Restaurants.Application.Commands.Restaurants.CraeteCommands;
using Restaurants.Application.Commands.Restaurants.DeleteCommands;
using Restaurants.Application.Commands.Restaurants.UpdateCommands;
using Restaurants.Application.DTOs.Common;
using Restaurants.Application.DTOs.Restaurants;
using Restaurants.Application.Queries.Restaurants.GetAllQueries;
using Restaurants.Application.Queries.Restaurants.GetRestaurantQueries;
using Restaurants.Domain.Constants;

namespace Restaurants.API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class RestaurantsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<RestaurantsController> _logger;

    public RestaurantsController(IMediator mediator, ILogger<RestaurantsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [Authorize(Roles = UserRoles.Admin)]
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<GetAllRestaurantsDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<GetAllRestaurantsDto>>> GetAll([FromQuery] GetAllRestaurantsQuery query)
    {
        var page = await _mediator.Send(query);
        return page;
    } 

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(GetRestaurantByIdDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound, "text/plain")]
    public async Task<ActionResult<GetRestaurantByIdDto>> GetById(int id)
    {
        var restaurant = await _mediator.Send(new GetRestaurantByIdQuery(id));
        return restaurant;
    }

    [HttpPost]
    [ProducesResponseType(typeof(GetRestaurantByIdDto), StatusCodes.Status200OK)]
    [Authorize(Roles = UserRoles.Owner, Policy = AuthorizationPolicies.HasNationalityPolicy)]
    public async Task<IActionResult> Create(CreateRestaurantCommand command)
    {
        int id = await _mediator.Send(command);
        var restaurant = new
        {
            id,
            command.Name,
            command.Description,
            command.HasDelivery,
            command.ContactEmail,
            command.ContactNumber,
            command.City,
            command.Street,
            command.PostalCode
        };

        return CreatedAtAction(nameof(GetById), new { id }, restaurant);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = UserRoles.Owner)]
    [ProducesResponseType(typeof(GetRestaurantByIdDto), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound, "text/plain")]
    public async Task<IActionResult> Update(int id, UpdateRestaurantCommand command)
    {
        command.Id = id;
        await _mediator.Send(command);

        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(GetRestaurantByIdDto), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound, "text/plain")]
    public async Task<IActionResult> Delete(int id)
    {
        await _mediator.Send(new DeleteRestaurantCommand(id));
        return NoContent();
    }
}
