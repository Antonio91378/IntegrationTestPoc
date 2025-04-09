using System.Net;
using System.Net.Http.Json;
using IntegrationTestPoc.Domain;
using IntegrationTestPoc.Tests.Helper;
using IntegrationTestPoc.Tests.Infra;
using IntegrationTestPoc.Tests.Utils;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

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
        // Act
        var response = await _client.GetAsync("/api/WeatherForecast");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var forecasts = await response.Content.ReadFromJsonAsync<IEnumerable<WeatherForecast>>();
        Assert.NotNull(forecasts);
        Assert.True(forecasts.Any(), "The response should contain at least one weather forecast.");
    }
}