using MediatR;
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

        public RestaurantsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AllRestaurantsDto>>> GetAll()
        {
            var restaurants = await _mediator.Send(new GetAllRestaurantsQuery());
            return restaurants;
        } 

        [HttpGet("{id}")]
        public async Task<ActionResult<RestaurantDto>> GetById(int id)
        {
            var restaurant = await _mediator.Send(new GetRestaurantByIdQuery(id));

            if (restaurant is null)
                return NotFound(new {message = "Invalid restaurant id"});

            return restaurant;
        }
        
        [HttpPost]
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
        public async Task<IActionResult> Update(int id, UpdateRestaurantCommand command)
        {
            command.Id = id;
            bool isUpdated = await _mediator.Send(command);

            if (!isUpdated)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            bool isDeleted = await _mediator.Send(new DeleteRestaurantCommand(id));

            if (!isDeleted)
                return NotFound();

            return NoContent();
        }

    }
}
