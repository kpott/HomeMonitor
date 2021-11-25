namespace HomeMonitor.Api.Temperature.GraphQL;

public class TemperatureQuery
{
    public TemperatureMeasurement GetLatestTemperatureMeasurement() =>
        TemperatureHistory.Measurements.Last();
}