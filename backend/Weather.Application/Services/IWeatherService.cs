using Weather.Application.Models;

namespace Weather.Application.Services;

public interface IWeatherService
{
    Task<WeatherResult> GetWeatherAsync(DateTime date);
}