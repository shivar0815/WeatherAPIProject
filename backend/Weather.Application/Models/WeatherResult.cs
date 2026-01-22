namespace Weather.Application.Models;

public class WeatherResult
{
    public string Date { get; set; } = string.Empty;
    public double? MinTemp { get; set; }
    public double? MaxTemp { get; set; }
    public double? Precipitation { get; set; }
    public string Status { get; set; } = "Success";
    public string? ErrorMessage { get; set; }
}