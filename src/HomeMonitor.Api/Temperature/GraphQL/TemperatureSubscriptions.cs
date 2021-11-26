namespace HomeMonitor.Api.Temperature.GraphQL;

[ExtendObjectType(Name = "Subscription")]
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