namespace HomeMonitor.Api.Temperature.GraphQL;

public class TemperatureQueryType : ObjectType<TemperatureQuery>
{
    protected override void Configure(IObjectTypeDescriptor<TemperatureQuery> descriptor)
    {
        descriptor
            .Field(f => f.GetLatestTemperatureMeasurement())
            .Type<TemperatureMeasurementType>();
    }
}