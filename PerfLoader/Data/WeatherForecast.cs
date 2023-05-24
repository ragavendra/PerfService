namespace PerfLoader.Data;

public class WeatherForecast
{
    public DateTime Date { get; set; }

    public int TemperatureC { get; set; }

    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    public string? Summary { get; set; }

    // On a scale of 1 - 10, this can be set based on temp (-20 to 55 C).
    public int SummaryIndex(int scale)
    {
        var tempC = TemperatureC + 20;

        return tempC/scale;
        /*
        switch (75/tempC)
        {
            case <= 1: return 1;
            case <= 2: return 2;
        };

        return default;*/
    }
}
