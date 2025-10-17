using Microsoft.AspNetCore.Mvc;
using Restaurants.API.Models;
using Restaurants.API.Services.Interfaces;

namespace Restaurants.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IWeatherForecastService _weatherForecastService;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherForecastService weatherForecastService)
    {
        _logger = logger;
        _weatherForecastService = weatherForecastService;
    }

    [HttpGet]
    public IEnumerable<WeatherForecast> Get()
    {
        var forcasts = _weatherForecastService.GetForecasts();
        return forcasts;
    }

    [HttpPost("range")]
    public IActionResult GetInRange([FromQuery] int n,[FromBody] TemperatureRangeRequest rangeRequest)
    {
        if (rangeRequest.Min > rangeRequest.Max || n <= 0)
            return BadRequest(new {message = "min should be less than max and n must be positive number"});

        var forcasts = _weatherForecastService.GetForecastsInRange(rangeRequest.Min, rangeRequest.Max, numberOfResults:n);

        return Ok(forcasts);
    }
}
