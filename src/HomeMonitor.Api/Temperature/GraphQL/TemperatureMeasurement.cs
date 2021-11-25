using HomeMonitor.Contracts;

namespace HomeMonitor.Api.Temperature.GraphQL;

public class TemperatureMeasurement : ITemperatureMeasured
{
    public Guid Id { get; init; }
    public string Location { get; init; }
    public decimal Temperature { get; init; }
    public decimal Humidity { get; init; }
    public DateTimeOffset RecordedAt { get; init; }
}