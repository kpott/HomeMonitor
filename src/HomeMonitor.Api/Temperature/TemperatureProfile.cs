using AutoMapper;
using HomeMonitor.Api.Temperature.GraphQL;
using HomeMonitor.Contracts;

namespace HomeMonitor.Api.Temperature;

public class TemperatureProfile : Profile
{
    public TemperatureProfile()
    {
        CreateMap<ITemperatureMeasured, TemperatureMeasurement>();
    }
}