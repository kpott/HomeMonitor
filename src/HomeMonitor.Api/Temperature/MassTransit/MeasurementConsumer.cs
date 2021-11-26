using AutoMapper;
using HomeMonitor.Api.Options;
using HomeMonitor.Api.Temperature.GraphQL;
using HomeMonitor.Contracts;
using HotChocolate.Subscriptions;
using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;
using MassTransit;
using Microsoft.Extensions.Options;

namespace HomeMonitor.Api.Temperature.MassTransit;

public class MeasurementConsumer : IConsumer<ITemperatureMeasured>
{
    private readonly ILogger<MeasurementConsumer> _logger;
    private readonly InfluxDbSettings _influxDbSettings;
    private readonly ITopicEventSender _eventSender;
    private readonly IMapper _mapper;

    public MeasurementConsumer(ILogger<MeasurementConsumer> logger, IOptions<InfluxDbSettings> influxDBOptions,
        ITopicEventSender eventSender, IMapper mapper)
    {
        _logger = logger;
        _influxDbSettings = influxDBOptions.Value;
        _eventSender = eventSender;
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<ITemperatureMeasured> context)
    {
        using var client = InfluxDBClientFactory.Create(_influxDbSettings.Uri.ToString(), _influxDbSettings.Token);
        using var write = client.GetWriteApi();

        var temperatureMeasurement = _mapper.Map<TemperatureMeasurement>(context.Message);
        _logger.LogInformation("Received Temperature Measurement {@TemperatureMeasurement}",
            temperatureMeasurement);

        var temperaturePoint = PointData.Measurement("temperature")
            .Tag("location", temperatureMeasurement.Location)
            .Field("value", temperatureMeasurement.Temperature)
            .Timestamp(temperatureMeasurement.RecordedAt, WritePrecision.Ns);

        write.WritePoint("homemonitor", "home", temperaturePoint);

        var humidityPoint = PointData.Measurement("humidity")
            .Tag("location", temperatureMeasurement.Location)
            .Field("value", temperatureMeasurement.Humidity)
            .Timestamp(temperatureMeasurement.RecordedAt, WritePrecision.Ns);

        write.WritePoint("homemonitor", "home", humidityPoint);

        TemperatureHistory.Measurements.Add(temperatureMeasurement);

        await _eventSender.SendAsync(
            nameof(TemperatureSubscriptions.OnTemperatureReportedAsync),
            temperatureMeasurement);
    }
}