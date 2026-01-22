using System.Text.Json;
using Weather.Application.Models;
using Weather.Application.Services;

namespace Weather.Infrastructure.WeatherApi;

public class OpenMeteoClient : IWeatherService
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "https://archive-api.open-meteo.com/v1/archive";
    
    private const double Latitude = 32.78;
    private const double Longitude = -96.8;

    public OpenMeteoClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<WeatherResult> GetWeatherAsync(DateTime date)
    {
        try
        {
            var dateStr = date.ToString("yyyy-MM-dd");
                     
            var url = $"{BaseUrl}?latitude={Latitude}&longitude={Longitude}&start_date={dateStr}&end_date={dateStr}&daily=temperature_2m_max,temperature_2m_min,precipitation_sum&timezone=auto";

            var response = await _httpClient.GetAsync(url);
            
            if (!response.IsSuccessStatusCode)
            {
                return new WeatherResult
                {
                    Date = dateStr,
                    Status = "ApiError",
                    ErrorMessage = $"API returned status code: {response.StatusCode}"
                };
            }

            var content = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<OpenMeteoResponse>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            
            if (apiResponse?.Daily == null || 
                apiResponse.Daily.Time == null || 
                apiResponse.Daily.Time.Length == 0)
            {
                return new WeatherResult
                {
                    Date = dateStr,
                    Status = "NoData",
                    ErrorMessage = "No weather data available for this date"
                };
            }

            return new WeatherResult
            {
                Date = dateStr,
                MinTemp = apiResponse.Daily.Temperature_2m_min?[0],
                MaxTemp = apiResponse.Daily.Temperature_2m_max?[0],
                Precipitation = apiResponse.Daily.Precipitation_sum?[0],
                Status = "Success"
            };
        }
        catch (HttpRequestException ex)
        {
            return new WeatherResult
            {
                Date = date.ToString("yyyy-MM-dd"),
                Status = "NetworkError",
                ErrorMessage = $"Network error: {ex.Message}"
            };
        }
        catch (Exception ex)
        {
            return new WeatherResult
            {
                Date = date.ToString("yyyy-MM-dd"),
                Status = "Error",
                ErrorMessage = $"Unexpected error: {ex.Message}"
            };
        }
    }
}

public class OpenMeteoResponse
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public DailyData? Daily { get; set; }
}

public class DailyData
{
    public string[]? Time { get; set; }
    public double[]? Temperature_2m_max { get; set; }
    public double[]? Temperature_2m_min { get; set; }
    public double[]? Precipitation_sum { get; set; }
}