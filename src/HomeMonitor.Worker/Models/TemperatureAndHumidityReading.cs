namespace HomeMonitor.Worker.Models;

public record struct TemperatureAndHumidityReading
{
    public double Temperature { get; init; }
    public double Humidity { get; init; }
}