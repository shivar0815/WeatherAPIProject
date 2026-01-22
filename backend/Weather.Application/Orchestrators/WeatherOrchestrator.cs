using Weather.Application.Models;
using Weather.Application.Services;

namespace Weather.Application.Orchestrators;

public class WeatherOrchestrator
{
    private readonly IDateReader _dateReader;
    private readonly IWeatherService _weatherService;
    private readonly IWeatherStorage _weatherStorage;

    public WeatherOrchestrator(
        IDateReader dateReader,
        IWeatherService weatherService,
        IWeatherStorage weatherStorage)
    {
        _dateReader = dateReader;
        _weatherService = weatherService;
        _weatherStorage = weatherStorage;
    }

    public async Task<List<WeatherResult>> ProcessWeatherDataAsync(string dateFilePath)
    {
        var results = new List<WeatherResult>();
        
        var dateParseResults = await _dateReader.ReadDatesAsync(dateFilePath);

        foreach (var dateResult in dateParseResults)
        {
            if (!dateResult.IsValid)
            {
                results.Add(new WeatherResult
                {
                    Date = dateResult.OriginalValue,
                    Status = "Invalid",
                    ErrorMessage = dateResult.ErrorMessage ?? "Invalid date format"
                });
                continue;
            }

            var date = dateResult.ParsedDate!.Value;

            if (_weatherStorage.Exists(date))
            {
                var stored = await _weatherStorage.GetStoredWeatherAsync(date);
                if (stored != null)
                {
                    results.Add(stored);
                    continue;
                }
            }

            var weatherResult = await FetchAndStoreWeatherAsync(date);
            results.Add(weatherResult);
        }

        return results;
    }

    private async Task<WeatherResult> FetchAndStoreWeatherAsync(DateTime date)
    {
        try
        {
            var weatherResult = await _weatherService.GetWeatherAsync(date);
            
            if (weatherResult.Status == "Success")
            {
                await _weatherStorage.SaveWeatherAsync(date, weatherResult);
            }
            
            return weatherResult;
        }
        catch (Exception ex)
        {
            return new WeatherResult
            {
                Date = date.ToString("yyyy-MM-dd"),
                Status = "Error",
                ErrorMessage = $"Failed to fetch weather: {ex.Message}"
            };
        }
    }
}