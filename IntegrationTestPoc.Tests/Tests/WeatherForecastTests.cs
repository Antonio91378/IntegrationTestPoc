using System.Net;
using System.Net.Http.Json;
using IntegrationTestPoc.Domain;
using IntegrationTestPoc.Tests.Helper;
using IntegrationTestPoc.Tests.Infra;
using IntegrationTestPoc.Tests.Utils;
using Microsoft.Extensions.DependencyInjection;
using FluentAssertions;
using IntegrationTestPoc.Domain.Utils;

namespace IntegrationTestPoc.Tests.Tests;

[Collection("WhiteboxAppFixture")]
[Trait("TestCategory", "Integration")]
public class WeatherForecastTests 
{
    private readonly HttpClient _client;
    private readonly TestStarterHelper _testStarterHelper;


    public WeatherForecastTests(CustomWebApplicationFactory factory)
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", Constants.AspnetcoreEnvironment);

        _testStarterHelper = factory.Services.GetRequiredService<TestStarterHelper>();
        _testStarterHelper.GenerateDatabase();
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAll_ReturnsOkAndData()
    {
        // Arrange
        var weatherForecasts = Enumerable.Range(1, 10).Select(i => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(i),
            TemperatureC = 20 + i,
            Summary = $"Summary {i}",
            SecondaryId = Guid.NewGuid().ToString()
        }).ToList();

        await _testStarterHelper.SeedRangeAsync(weatherForecasts);

        // Act
        var response = await _client.GetAsync("/api/WeatherForecast");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var forecasts = await response.Content.ReadFromJsonAsync<ControllerMessenger>();
        forecasts.Should().NotBeNull();
        
    }
}