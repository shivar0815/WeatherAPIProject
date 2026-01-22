using Microsoft.AspNetCore.Mvc;
using Weather.Application.Orchestrators;

namespace Weather.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WeatherController : ControllerBase
{
    private readonly WeatherOrchestrator _orchestrator;
    private readonly ILogger<WeatherController> _logger;

    public WeatherController(
        WeatherOrchestrator orchestrator,
        ILogger<WeatherController> logger)
    {
        _orchestrator = orchestrator;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetWeather()
    {
        try
        {
            var dateFilePath = Path.Combine(Directory.GetCurrentDirectory(), "dates.txt");
            
            if (!System.IO.File.Exists(dateFilePath))
            {
                _logger.LogError("dates.txt file not found at {Path}", dateFilePath);
                return BadRequest(new { error = "dates.txt file not found" });
            }

            var results = await _orchestrator.ProcessWeatherDataAsync(dateFilePath);
            return Ok(results);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing weather data");
            return StatusCode(500, new 
            { 
                error = "Internal server error", 
                message = ex.Message 
            });
        }
    }
}