using IntegrationTestPoc.Domain;
using IntegrationTestPoc.Domain.Utils;
using IntegrationTestPoc.Service;
using Microsoft.AspNetCore.Mvc;

namespace IntegrationTestPoc.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly IWeatherForecastService _weatherForecastService;

    public WeatherForecastController(IWeatherForecastService weatherForecastService)
    {
        _weatherForecastService = weatherForecastService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _weatherForecastService.GetAllAsync();
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _weatherForecastService.GetByIdAsync(id);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] WeatherForecast forecast)
    {
        var result = await _weatherForecastService.CreateAsync(forecast);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update([FromBody] WeatherForecast forecast)
    {
        var result = await _weatherForecastService.UpdateAsync(forecast);
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _weatherForecastService.DeleteAsync(id);
        return StatusCode(result.StatusCode, result);
    }
}
