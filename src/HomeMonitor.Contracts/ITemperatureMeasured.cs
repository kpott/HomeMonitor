namespace HomeMonitor.Contracts;

public interface ITemperatureMeasured
{
    public Guid Id { get; }
    public string Location { get; }
    public double Temperature { get; }
    public double Humidity { get; }
    public DateTimeOffset RecordedAt { get; }
}