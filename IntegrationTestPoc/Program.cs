using IntegrationTestPoc.Domain;
using IntegrationTestPoc.Domain.Interfaces;
using IntegrationTestPoc.Domain.Utils;
using IntegrationTestPoc.Service;
using IntegrationTestPoc.Util;
using IntegrationTestProc.Infra;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IWeatherForecastRepository, WeatherForecastRepository>();
builder.Services.AddScoped<IWeatherForecastService, WeatherForecastService>();
builder.Services.AddTransient<IControllerMessenger, ControllerMessenger>();
builder.Services.AddTransient<IAppConfiguration, AppConfiguration>();
builder.Services.AddDbContext<Context>((serviceProvider, options) =>
{
    var appConfiguration = serviceProvider.GetRequiredService<IAppConfiguration>();

    options.UseSqlServer(appConfiguration.GetSqlServerConnectionString(), sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null);
    });
});

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

namespace IntegrationTestPoc
{
    public partial class Program
    {
    }
}