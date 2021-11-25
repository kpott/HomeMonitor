using HomeMonitor.Api.Temperature.GraphQL;
using HomeMonitor.Contracts;

namespace HomeMonitor.Api.Temperature;

public static class TemperatureHistory
{
    public static List<TemperatureMeasurement> Measurements { get; set; } = new();
}