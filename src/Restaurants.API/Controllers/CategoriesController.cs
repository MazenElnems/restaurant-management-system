using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Commands.Categories.CreateCategory;
using Restaurants.Application.Commands.Categories.DeleteCategory;
using Restaurants.Application.Commands.Categories.UpdateCategory;
using Restaurants.Application.DTOs.Categories;
using Restaurants.Application.Queries.Categories.GetCategories;
using Restaurants.Domain.Constants;

namespace Restaurants.API.Controllers;

[Authorize]
[Route("api/restaurants/{restaurantId}/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetAllCategoriesDto>>> GetAll(int restaurantId)
    {
        var dto = await _mediator.Send(new GetAllCategoriesQuery(restaurantId));
        return dto;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetCategoryByIdDto>> GetById(int restaurantId, int id)
    {
        var dto = await _mediator.Send(new GetCategoryByIdQuery(id, restaurantId));
        return dto;
    }

    [HttpPut("{id}")]
    [Authorize(Roles = UserRoles.Owner)]
    public async Task<IActionResult> Update(int restaurantId, int id, UpdateCategoryCommand command)
    {
        command.Id = id;
        command.RestaurantId = restaurantId;
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = UserRoles.Owner)]
    public async Task<IActionResult> Delete(int restaurantId,int id)
    {
        await _mediator.Send(new DeleteCategoryCommand(id, restaurantId));
        return NoContent();
    }

    [HttpPost]
    [Authorize(Roles = UserRoles.Owner)]
    public async Task<IActionResult> Create(int restaurantId, CreateCategoryCommand command)
    {
        command.RestaurantId = restaurantId;
        int id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id, restaurantId }, id);
    }
}

