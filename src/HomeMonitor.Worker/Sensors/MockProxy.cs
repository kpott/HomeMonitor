using HomeMonitor.Worker.Models;

namespace HomeMonitor.Worker.Sensors;

public class MockProxy : ITemperatureAndHumiditySensor
{
    private readonly ILogger<MockProxy> _logger;
    private const int MinTemperature = 67;
    private const int MaxTemperature = 70;
    private const int MinHumidity = 35;
    private const int MaxHumidity = 38;

    public MockProxy(ILogger<MockProxy> logger)
    {
        _logger = logger;
    }

    public Task<TemperatureAndHumidityReading> ReadAsync()
    {
        var random = new Random();

        var temperature = random.NextDouble() * (MaxTemperature - MinTemperature) + MinTemperature;
        _logger.LogInformation("Obtained mock temperature measurement {Temperature}",
            temperature);

        var humidity = random.NextDouble() * (MaxHumidity - MinHumidity) + MinHumidity;
        _logger.LogInformation("Obtained mock humidity measurement {Humidity}",
            humidity);
        
        return Task.FromResult(new TemperatureAndHumidityReading
        {
            Temperature = temperature,
            Humidity = humidity,
        });
    }
}