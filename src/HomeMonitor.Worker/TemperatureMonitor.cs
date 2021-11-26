using System.Device.I2c;
using HomeMonitor.Contracts;
using HomeMonitor.Worker.Options;
using Iot.Device.Si7021;
using MassTransit;
using Microsoft.Extensions.Options;

namespace HomeMonitor.Worker;

public class TemperatureMonitor : BackgroundService
{
    private readonly ILogger<TemperatureMonitor> _logger;
    private readonly SensorSettings _sensorSettings;
    private readonly IBus _bus;

    public TemperatureMonitor(ILogger<TemperatureMonitor> logger, IOptions<SensorSettings> appSettingsOptions, IBus bus)
    {
        _logger = logger;
        _sensorSettings = appSettingsOptions.Value;
        _bus = bus;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var i2CConnectionSettings =
            new I2cConnectionSettings(_sensorSettings.I2CBusId, _sensorSettings.I2CDeviceAddress);

        _logger.LogInformation("Preparing to make I2C connection {BusId} {DeviceAddress}", _sensorSettings.I2CBusId,
            _sensorSettings.I2CDeviceAddress);

        using var i2CDevice = I2cDevice.Create(i2CConnectionSettings);
        using var sensor = new Si7021(i2CDevice);

        while (!stoppingToken.IsCancellationRequested)
        {
            var temperatureMeasurement = new
            {
                Id = NewId.NextGuid(),
                Location = _sensorSettings.Location,
                Temperature = sensor.Temperature.DegreesFahrenheit,
                Humidity = sensor.Humidity.Percent,
                RecordedAt = DateTimeOffset.Now
            };

            _logger.LogInformation("Obtained Temperature Measurement {TemperatureMeasurement}",
                temperatureMeasurement);

            await _bus.Publish<ITemperatureMeasured>(temperatureMeasurement, stoppingToken);

            _logger.LogInformation("Published Temperature Measurement and waiting {SensorDelayMs}",
                _sensorSettings.DelayBetweenMeasurementsMs);

            await Task.Delay(_sensorSettings.DelayBetweenMeasurementsMs, stoppingToken);
        }
    }
}