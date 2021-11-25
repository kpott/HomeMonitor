namespace HomeMonitor.Api.Temperature.GraphQL;

public class TemperatureMeasurementType : ObjectType<TemperatureMeasurement>
{
    protected override void Configure(IObjectTypeDescriptor<TemperatureMeasurement> descriptor)
    {
        descriptor
            .Field(f => f.RecordedAt)
            .Type<DateTimeType>();

        descriptor
            .Field(f => f.Location)
            .Type<StringType>();

        descriptor
            .Field(f => f.Temperature)
            .Type<DecimalType>();
        
        descriptor
            .Field(f => f.Humidity)
            .Type<DecimalType>();
    }
}