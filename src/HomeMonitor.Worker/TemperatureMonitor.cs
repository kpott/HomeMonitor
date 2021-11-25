using HomeMonitor.Contracts;
using HomeMonitor.Worker.Options;
using MassTransit;
using Microsoft.Extensions.Options;

namespace HomeMonitor.Worker;

public class TemperatureMonitor : BackgroundService
{
    private readonly ILogger<TemperatureMonitor> _logger;
    private readonly AppSettings _appSettings;
    private readonly IBus _bus;

    public TemperatureMonitor(ILogger<TemperatureMonitor> logger, IOptions<AppSettings> appSettingsOptions, IBus bus)
    {
        _logger = logger;
        _appSettings = appSettingsOptions.Value;
        _bus = bus;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var random = new Random();


            var temperatureMeasurement = new
            {
                Id = NewId.NextGuid(),
                Location = _appSettings.Location,
                Temperature = random.Next(72, 75),
                Humidity = random.Next(40, 45),
                RecordedAt = DateTimeOffset.Now
            };

            await _bus.Publish<ITemperatureMeasured>(temperatureMeasurement, stoppingToken);

            _logger.LogInformation("Published Temperature Measurement {TemperatureMeasurement}",
                temperatureMeasurement);

            await Task.Delay(5000, stoppingToken);
        }
    }
}