using IntegrationTestPoc.Domain;
using IntegrationTestPoc.Domain.Utils;
using IntegrationTestPoc.Domain.Interfaces;

namespace IntegrationTestPoc.Service;

public class WeatherForecastService : IWeatherForecastService
{
    private readonly IWeatherForecastRepository _repository;

    public WeatherForecastService(IWeatherForecastRepository repository)
    {
        _repository = repository;
    }

    public async Task<ControllerMessenger> CreateAsync(WeatherForecast forecast)
    {
        var messenger = new ControllerMessenger();
        try
        {
            await _repository.CreateAsync(forecast);
            return messenger.ReturnSuccess(201, "Weather forecast created successfully.");
        }
        catch (Exception ex)
        {
            return messenger.ReturnInternalError500(ex.Message);
        }
    }

    public async Task<ControllerMessenger> DeleteAsync(int id)
    {
        var messenger = new ControllerMessenger();
        try
        {
            await _repository.DeleteAsync(id);
            return messenger.ReturnSuccess(200, "Weather forecast deleted successfully.");
        }
        catch (Exception ex)
        {
            return messenger.ReturnInternalError500(ex.Message);
        }
    }

    public async Task<ControllerMessenger> GetAllAsync()
    {
        var messenger = new ControllerMessenger();
        try
        {
            var forecasts = await _repository.GetAllAsync();
            if (forecasts == null || !forecasts.Any())
                return messenger.ReturnNotFound404("No weather forecasts found.");
            
            return messenger.ReturnSuccess(200, forecasts);
        }
        catch (Exception ex)
        {
            return messenger.ReturnInternalError500(ex.Message);
        }
    }

    public async Task<ControllerMessenger> GetByIdAsync(int id)
    {
        var messenger = new ControllerMessenger();
        try
        {
            var forecast = await _repository.GetByIdAsync(id);
            return forecast != null
                ? messenger.ReturnSuccess(200, forecast)
                : messenger.ReturnNotFound404($"Weather forecast with ID {id} not found.");
        }
        catch (Exception ex)
        {
            return messenger.ReturnInternalError500(ex.Message);
        }
    }

    public async Task<ControllerMessenger> UpdateAsync(WeatherForecast forecast)
    {
        var messenger = new ControllerMessenger();
        try
        {
            await _repository.UpdateAsync(forecast);
            return messenger.ReturnSuccess(200, "Weather forecast updated successfully.");
        }
        catch (Exception ex)
        {
            return messenger.ReturnInternalError500(ex.Message);
        }
    }
}
