namespace HomeMonitor.Api.Temperature.GraphQL;

public class TemperatureSubscriptions
{
    [Subscribe]
    [Topic]
    public Task<TemperatureMeasurement> OnTemperatureReportedAsync(
        [EventMessage] TemperatureMeasurement temperatureMeasurement)
    {
        return Task.FromResult(temperatureMeasurement);
    }
}