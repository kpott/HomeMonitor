using HomeMonitor.Worker.Models;
using HomeMonitor.Worker.Options;
using Iot.Device.Si7021;
using Microsoft.Extensions.Options;
using System.Device.I2c;

namespace HomeMonitor.Worker.Sensors;

public class Si7021Proxy : ITemperatureAndHumiditySensor
{
    private readonly ILogger<Si7021Proxy> _logger;
    private readonly I2CSettings _i2CSettings;


    public Si7021Proxy(ILogger<Si7021Proxy> logger, IOptions<I2CSettings> sensorOptions)
    {
        _logger = logger;
        _i2CSettings = sensorOptions.Value;
    }

    public Task<TemperatureAndHumidityReading> ReadAsync()
    {
        var i2CConnectionSettings =
            new I2cConnectionSettings(_i2CSettings.BusId, _i2CSettings.DeviceAddress);

        _logger.LogInformation("Preparing to make I2C connection {BusId} {DeviceAddress}", _i2CSettings.BusId,
            _i2CSettings.DeviceAddress);

        using var i2CDevice = I2cDevice.Create(i2CConnectionSettings);
        using var sensor = new Si7021(i2CDevice);

        var temperatureMeasurement = sensor.Temperature.DegreesFahrenheit;
        _logger.LogInformation("Obtained Temperature Measurement {TemperatureMeasurement}",
            temperatureMeasurement);

        var humidityMeasurement = sensor.Humidity.Percent;
        _logger.LogInformation("Obtained Humidity Measurement {HumidityMeasurement}",
            humidityMeasurement);

        return Task.FromResult(new TemperatureAndHumidityReading
        {
            Temperature = temperatureMeasurement,
            Humidity = humidityMeasurement
        });
    }
}