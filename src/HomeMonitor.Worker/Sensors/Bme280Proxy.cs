using HomeMonitor.Worker.Models;
using HomeMonitor.Worker.Options;
using Iot.Device.Bmxx80;
using Microsoft.Extensions.Options;
using System.Device.I2c;

namespace HomeMonitor.Worker.Sensors;

public class Bme280Proxy : ITemperatureAndHumiditySensor
{
    private readonly ILogger<Bme280Proxy> _logger;
    private readonly I2CSettings _i2CSettings;


    public Bme280Proxy(ILogger<Bme280Proxy> logger, IOptions<I2CSettings> sensorOptions)
    {
        _logger = logger;
        _i2CSettings = sensorOptions.Value;
    }

    public async Task<TemperatureAndHumidityReading> ReadAsync()
    {
        var i2CConnectionSettings =
            new I2cConnectionSettings(_i2CSettings.BusId, _i2CSettings.DeviceAddress);

        _logger.LogInformation("Preparing to make I2C connection {BusId} {DeviceAddress}", _i2CSettings.BusId,
            _i2CSettings.DeviceAddress);

        using var i2CDevice = I2cDevice.Create(i2CConnectionSettings);
        using var sensor = new Bme280(i2CDevice);

        var measurement = await sensor.ReadAsync();

        if (!measurement.Temperature.HasValue)
        {
            throw new NullReferenceException("Null temperature value returned from Bme280 sensor");
        }

        var temperature = measurement.Temperature.Value.DegreesFahrenheit;
        _logger.LogInformation("Obtained Temperature Measurement {Temperature}",
            measurement.Temperature);

        if (!measurement.Humidity.HasValue)
        {
            throw new NullReferenceException("Null humidity value returned from Bme280 sensor");
        }

        var humidity = measurement.Temperature.Value.DegreesFahrenheit;
        _logger.LogInformation("Obtained Humidity Measurement {Humidity}",
            measurement.Humidity);

        return new TemperatureAndHumidityReading
        {
            Temperature = temperature,
            Humidity = humidity,
        };
    }
}