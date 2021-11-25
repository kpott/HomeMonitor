namespace HomeMonitor.Contracts;

public interface ITemperatureMeasured
{
    public Guid Id { get;  }
    public string Location { get; }
    public decimal Temperature { get; }
    public decimal Humidity { get; }
    public DateTimeOffset RecordedAt { get; }
}