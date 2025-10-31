using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Commands.Dishes.CreateDish;
using Restaurants.Application.Commands.Dishes.DeleteDish;
using Restaurants.Application.Commands.Dishes.MoveDishesCommands;
using Restaurants.Application.Commands.Dishes.UpdateDish;
using Restaurants.Application.DTOs.Dishes;
using Restaurants.Application.Queries.Dishes.GetDishesQueries;
using Restaurants.Domain.Constants;

namespace Restaurants.API.Controllers
{
    [Authorize]
    [Route("api/restaurants/{restaurantId}/[controller]")]
    [ApiController]
    public class DishesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DishesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<GetAllDishesDto>>> GetAll(int restaurantId)
        {
            var dto = await _mediator.Send(new GetAllDishesQuery(restaurantId));
            return dto;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetDishDto>> GetById(int restaurantId, int id)
        {
            var dto = await _mediator.Send(new GetDishByIdQuery(restaurantId, id));
            return dto;
        }

        [HttpGet("categories/{categoryId}/dishes")]
        public async Task<List<GetAllDishesDto>> GetByCategory(int restaurantId,int categoryId)
        {
            var dto = await _mediator.Send(new GetDishesByCategoryIdQuery(restaurantId, categoryId));
            return dto;
        }

        [HttpPut("{id}/move")]
        public async Task<IActionResult> MoveToCategory(MoveToCategoryCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.Owner)]
        public async Task<IActionResult> Create(int restaurantId, CreateDishCommand command)
        {
            command.RestaurantId = restaurantId;
            int id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id, restaurantId }, id);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = UserRoles.Owner)]
        public async Task<IActionResult> Update(int restaurantId, int id, UpdateDishCommand command)
        {
            command.Id = id;
            command.RestaurantId = restaurantId;
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = UserRoles.Owner)]
        public async Task<IActionResult> Delete(int restaurantId, int id)
        {
            await _mediator.Send(new DeleteDishCommand(id, restaurantId));
            return NoContent();
        }
    }
}
