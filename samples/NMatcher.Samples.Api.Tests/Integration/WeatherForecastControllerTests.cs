using FluentAssertions;
using NMatcher.FluentAssertions;

namespace NMatcher.Samples.Api.Tests.Integration;


[Trait("Category", "Sample")]
public class WeatherForecastControllerTests
{
    [Fact]
    public async Task test_weather_forecast_endpoint()
    {
        var program = new TestApiProgram();
        var client = program.CreateClient();
        
        var rs = await client.GetAsync("/WeatherForecast");
        var response = await rs.Content.ReadAsStringAsync();

        response.Should().MatchJson(@"
        [
          {
            ""date"": ""@string@.IsDateTime()"",
            ""temperatureC"": ""@int@"",
            ""temperatureF"": ""@int@"",
            ""summary"": ""@string@.OneOf('Freezing', 'Cool', 'Mild', 'Warm', 'Balmy', 'Hot')""
          },
          ""@skip@""
        ]
        ");
    }
}