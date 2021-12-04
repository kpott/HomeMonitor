using HomeMonitor.Worker.Models;

namespace HomeMonitor.Worker.Sensors;

public interface ITemperatureAndHumiditySensor
{
    public Task<TemperatureAndHumidityReading> ReadAsync();
}