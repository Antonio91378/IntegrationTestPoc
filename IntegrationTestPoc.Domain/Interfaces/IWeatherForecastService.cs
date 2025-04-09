using IntegrationTestPoc.Domain.Utils;

namespace IntegrationTestPoc.Domain;

public interface IWeatherForecastService
{
    Task<ControllerMessenger> GetAllAsync();
    Task<ControllerMessenger> GetByIdAsync(int id);
    Task<ControllerMessenger> CreateAsync(WeatherForecast forecast);
    Task<ControllerMessenger> UpdateAsync(WeatherForecast forecast);
    Task<ControllerMessenger> DeleteAsync(int id);
}


