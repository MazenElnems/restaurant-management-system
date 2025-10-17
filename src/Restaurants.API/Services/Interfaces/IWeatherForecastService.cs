namespace Restaurants.API.Services.Interfaces;

public interface IWeatherForecastService
{
    public IEnumerable<WeatherForecast> GetForecasts();
    public IEnumerable<WeatherForecast> GetForecastsInRange(int min,int max,int numberOfResults);
}
