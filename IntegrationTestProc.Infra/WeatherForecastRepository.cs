using IntegrationTestPoc.Domain;
using IntegrationTestPoc.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IntegrationTestProc.Infra;

public class WeatherForecastRepository : IWeatherForecastRepository
{
    private readonly Context _context;

    public WeatherForecastRepository(Context context)
    {
        _context = context;
    }

    public async Task CreateAsync(WeatherForecast forecast)
    {
        await _context.WeatherForecasts.AddAsync(forecast);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var forecast = await _context.WeatherForecasts.FindAsync(id);
        if (forecast != null)
        {
            _context.WeatherForecasts.Remove(forecast);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<WeatherForecast>> GetAllAsync()
    {
        return await _context.WeatherForecasts.ToListAsync();
    }

    public async Task<WeatherForecast?> GetByIdAsync(int id)
    {
        return await _context.WeatherForecasts.FindAsync(id);
    }

    public async Task UpdateAsync(WeatherForecast forecast)
    {
        _context.WeatherForecasts.Update(forecast);
        await _context.SaveChangesAsync();
    }
}
