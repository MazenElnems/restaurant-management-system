using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Resaurants.DTOs;
using Restaurants.Application.Resaurants.Services.Interfaces;

namespace Restaurants.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantsController : ControllerBase
    {
        private readonly IRestaurantsService _restaurantsService;

        public RestaurantsController(IRestaurantsService restaurantsService)
        {
            _restaurantsService = restaurantsService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AllRestaurantsDto>>> GetAll()
        {
            var restaurants = await _restaurantsService.GetAllRestaurantsAsync();
            return restaurants;
        } 

        [HttpGet("{id}")]
        public async Task<ActionResult<RestaurantDto>> GetById(int id)
        {
            var restaurant = await _restaurantsService.GetRestaurantByIdAsync(id);

            if (restaurant is null)
                return NotFound(new {message = "Invalid restaurant id"});

            return restaurant;
        }
        
        [HttpPost]
        public async Task<IActionResult> Post(CreateRestaurantDto dto)
        {
            int id = await _restaurantsService.CreateRestaurantAsync(dto);
            var restaurant = new
            {
                id,
                dto.Name,
                dto.Description,
                dto.HasDelivery,
                dto.ContactEmail,
                dto.ContactNumber,
                dto.City,
                dto.Street,
                dto.PostalCode
            };

            return CreatedAtAction(nameof(GetById), new { id }, restaurant);
        }
    }
}
