namespace PerfLoader.Data;

public class WeatherForecastService
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    public Task<WeatherForecast[]> GetForecastAsync(DateTime startDate)
    {
        var res = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = startDate.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                // Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                // Summary = Summaries[-20 + (75/10 * TemperatureC)];
            });

        return Task.FromResult(res.ToArray());
    }
}
