using AutoMapper;
using HomeMonitor.Api.Temperature.GraphQL;
using HomeMonitor.Contracts;
using HotChocolate.Subscriptions;
using MassTransit;

namespace HomeMonitor.Api.Temperature.MassTransit;

public class MeasurementConsumer : IConsumer<ITemperatureMeasured>
{
    private readonly ILogger<MeasurementConsumer> _logger;
    private readonly ITopicEventSender _eventSender;
    private readonly IMapper _mapper;

    public MeasurementConsumer(ILogger<MeasurementConsumer> logger, ITopicEventSender eventSender, IMapper mapper)
    {
        _logger = logger;
        _eventSender = eventSender;
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<ITemperatureMeasured> context)
    {
        var temperatureMeasurement = _mapper.Map<TemperatureMeasurement>(context.Message);
        _logger.LogInformation("Received Temperature Measurement {@TemperatureMeasurement}",
            temperatureMeasurement);

        TemperatureHistory.Measurements.Add(temperatureMeasurement);
        
        await _eventSender.SendAsync(
            nameof(TemperatureSubscriptions.OnTemperatureReportedAsync),
            temperatureMeasurement);
    }
}