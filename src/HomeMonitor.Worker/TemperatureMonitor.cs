using HomeMonitor.Contracts;
using HomeMonitor.Worker.Options;
using HomeMonitor.Worker.Sensors;
using MassTransit;
using Microsoft.Extensions.Options;

namespace HomeMonitor.Worker;

public class TemperatureMonitor : BackgroundService
{
    private readonly ILogger<TemperatureMonitor> _logger;
    private readonly MonitorSettings _monitorSettings;
    private readonly IBus _bus;
    private readonly ITemperatureAndHumiditySensor _sensor;

    public TemperatureMonitor(ILogger<TemperatureMonitor> logger, IOptions<MonitorSettings> monitorOptions, IBus bus,
        ITemperatureAndHumiditySensor sensor)
    {
        _logger = logger;
        _monitorSettings = monitorOptions.Value;
        _bus = bus;
        _sensor = sensor;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var reading = await _sensor.ReadAsync();

            var temperatureMeasurement = new
            {
                Id = NewId.NextGuid(),
                _monitorSettings.Location,
                reading.Temperature,
                reading.Humidity,
                RecordedAt = DateTimeOffset.Now
            };

            await _bus.Publish<ITemperatureMeasured>(temperatureMeasurement, stoppingToken);

            _logger.LogInformation("Published Temperature Measurement and waiting {SensorDelayMs}",
                _monitorSettings.DelayBetweenMeasurementsMs);

            await Task.Delay(_monitorSettings.DelayBetweenMeasurementsMs, stoppingToken);
        }
    }
}