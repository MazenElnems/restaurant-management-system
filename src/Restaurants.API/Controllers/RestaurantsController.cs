using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Commands.Restaurants.CraeteCommands;
using Restaurants.Application.Commands.Restaurants.DeleteCommands;
using Restaurants.Application.Commands.Restaurants.UpdateCommands;
using Restaurants.Application.DTOs.Restaurants;
using Restaurants.Application.Queries.Restaurant.GetRestaurantQueries;

namespace Restaurants.API.Controllers
{
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

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<AllRestaurantsDto>), StatusCodes.Status200OK)]
        [Authorize]
        public async Task<ActionResult<IEnumerable<AllRestaurantsDto>>> GetAll()
        {
            var restaurants = await _mediator.Send(new GetAllRestaurantsQuery());
            return restaurants;
        } 

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(RestaurantDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound, "text/plain")]
        public async Task<ActionResult<RestaurantDto>> GetById(int id)
        {
            var restaurant = await _mediator.Send(new GetRestaurantByIdQuery(id));
            return restaurant;
        }
        
        [HttpPost]
        [ProducesResponseType(typeof(RestaurantDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> Post(CreateRestaurantCommand command)
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
        [ProducesResponseType(typeof(RestaurantDto), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound, "text/plain")]
        public async Task<IActionResult> Update(int id, UpdateRestaurantCommand command)
        {
            command.Id = id;
            await _mediator.Send(command);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(RestaurantDto), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound, "text/plain")]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteRestaurantCommand(id));
            return NoContent();
        }
    }
}
