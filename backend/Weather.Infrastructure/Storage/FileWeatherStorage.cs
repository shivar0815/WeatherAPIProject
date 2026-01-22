using System.Text.Json;
using Weather.Application.Models;
using Weather.Application.Services;

namespace Weather.Infrastructure.Storage;

public class FileWeatherStorage : IWeatherStorage
{
    private readonly string _storageDirectory;

    public FileWeatherStorage()
    {
        _storageDirectory = Path.Combine(Directory.GetCurrentDirectory(), "weather-data");
        
        if (!Directory.Exists(_storageDirectory))
        {
            Directory.CreateDirectory(_storageDirectory);
        }
    }

    public bool Exists(DateTime date)
    {
        var filePath = GetFilePath(date);
        return File.Exists(filePath);
    }

    public async Task<WeatherResult?> GetStoredWeatherAsync(DateTime date)
    {
        var filePath = GetFilePath(date);
        
        if (!File.Exists(filePath))
            return null;

        try
        {
            var json = await File.ReadAllTextAsync(filePath);
            return JsonSerializer.Deserialize<WeatherResult>(json);
        }
        catch
        {
            return null;
        }
    }

    public async Task SaveWeatherAsync(DateTime date, WeatherResult weather)
    {
        var filePath = GetFilePath(date);
        
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };
        
        var json = JsonSerializer.Serialize(weather, options);
        await File.WriteAllTextAsync(filePath, json);
    }

    private string GetFilePath(DateTime date)
    {
        var fileName = $"{date:yyyy-MM-dd}.json";
        return Path.Combine(_storageDirectory, fileName);
    }
}