using HomeMonitor.Contracts;
using MassTransit;

namespace HomeMonitor.MockWorker;

public class MockTemperatureMonitor : BackgroundService
{
    private readonly ILogger<MockTemperatureMonitor> _logger;
    private readonly IBus _bus;
    private const int DelayBetweenMeasurementsMs = 10000;
    private const int MinTemperature = 67;
    private const int MaxTemperature = 70;
    private const int MinHumidity = 35;
    private const int MaxHumidity = 38;


    public MockTemperatureMonitor(ILogger<MockTemperatureMonitor> logger, IBus bus)
    {
        _logger = logger;
        _bus = bus;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var random = new Random();
        while (!stoppingToken.IsCancellationRequested)
        {
            var temperatureMeasurement = new
            {
                Id = NewId.NextGuid(),
                Location = "Mock Location",
                Temperature = random.NextDouble() * (MaxTemperature - MinTemperature) + MinTemperature,
                Humidity = random.NextDouble() * (MaxHumidity - MinHumidity) + MinHumidity,
                RecordedAt = DateTimeOffset.Now
            };

            _logger.LogInformation("Obtained Temperature Measurement {TemperatureMeasurement}",
                temperatureMeasurement);

            await _bus.Publish<ITemperatureMeasured>(temperatureMeasurement, stoppingToken);

            _logger.LogInformation("Published Temperature Measurement and waiting {SensorDelayMs}",
                DelayBetweenMeasurementsMs);

            await Task.Delay(DelayBetweenMeasurementsMs, stoppingToken);
        }
    }
}