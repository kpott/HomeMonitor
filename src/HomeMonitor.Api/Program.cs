using HomeMonitor.Api.Options;
using HomeMonitor.Api.Temperature;
using HomeMonitor.Api.Temperature.GraphQL;
using HomeMonitor.Api.Temperature.MassTransit;
using MassTransit;
using MassTransit.JobService.Configuration;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Formatting.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, lc) => lc
    .WriteTo.Console(new JsonFormatter()));

builder.Services.AddOptions<RabbitMqSettings>()
    .Bind(builder.Configuration.GetSection(RabbitMqSettings.ConfigurationSectionName))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddOptions<InfluxDbSettings>()
    .Bind(builder.Configuration.GetSection(InfluxDbSettings.ConfigurationSectionName))
    .ValidateDataAnnotations()
    .ValidateOnStart();

var rabbitMqSettings = new RabbitMqSettings();
builder.Configuration.GetSection(RabbitMqSettings.ConfigurationSectionName)
    .Bind(rabbitMqSettings);

builder.Services.AddAutoMapper(typeof(TemperatureProfile));

builder.Services
    .AddGraphQLServer()
    .AddQueryType<TemperatureQueryType>()
    .AddSubscriptionType<TemperatureSubscriptions>()
    .AddInMemorySubscriptions();

builder.Services.AddMassTransit(x =>
    {
        x.SetKebabCaseEndpointNameFormatter();

        x.AddConsumersFromNamespaceContaining<MeasurementConsumer>();

        x.UsingRabbitMq((context, cfg) =>
        {
            cfg.Host(rabbitMqSettings.Uri, configurator =>
            {
                configurator.Username(rabbitMqSettings.Username);
                configurator.Password(rabbitMqSettings.Password);
            });
            var options = new ServiceInstanceOptions();

            cfg.ServiceInstance(options, instance => { instance.ConfigureEndpoints(context); });
        });
    })
    .AddMassTransitHostedService();

var app = builder.Build();

app
    .UseRouting()
    .UseWebSockets()
    .UseEndpoints(endpoints => { endpoints.MapGraphQL(); });

app.Run();