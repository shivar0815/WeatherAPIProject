using Weather.Application.Models;

namespace Weather.Application.Services;

public interface IWeatherStorage
{
    Task<WeatherResult?> GetStoredWeatherAsync(DateTime date);
    Task SaveWeatherAsync(DateTime date, WeatherResult weather);
    bool Exists(DateTime date);
}