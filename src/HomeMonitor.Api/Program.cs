using HomeMonitor.Api;
using HomeMonitor.Api.Options;
using HomeMonitor.Api.Temperature;
using HomeMonitor.Api.Temperature.GraphQL;
using HomeMonitor.Api.Temperature.MassTransit;
using MassTransit;
using MassTransit.JobService.Configuration;
using Serilog;
using Serilog.Formatting.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, lc) => lc
    .WriteTo.Console(new JsonFormatter()));

var rabbitMqOptions = new HomeMonitor.Api.Options.RabbitMqSettings();
builder.Configuration.GetSection(HomeMonitor.Api.Options.RabbitMqSettings.ConfigurationSectionName)
    .Bind(rabbitMqOptions);

builder.Services.AddOptions<InfluxDbSettings>()
    .Bind(builder.Configuration.GetSection(InfluxDbSettings.ConfigurationSectionName));

builder.Services.AddAutoMapper(typeof(TemperatureProfile));

builder.Services
    .AddGraphQLServer()
    .AddQueryType<TemperatureQueryType>()
    .AddSubscriptionType(d => d.Name("Subscription"))
    .AddTypeExtension<TemperatureSubscriptions>()
    .AddInMemorySubscriptions();

builder.Services.AddMassTransit(x =>
    {
        x.SetKebabCaseEndpointNameFormatter();

        x.AddConsumersFromNamespaceContaining<MeasurementConsumer>();

        x.UsingRabbitMq((context, cfg) =>
        {
            cfg.Host(rabbitMqOptions.Uri, configurator =>
            {
                configurator.Username(rabbitMqOptions.Username);
                configurator.Password(rabbitMqOptions.Password);
            });
            var options = new ServiceInstanceOptions();

            cfg.ServiceInstance(options, instance => { instance.ConfigureEndpoints(context); });
        });
    })
    .AddMassTransitHostedService();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}

app.UseHttpsRedirection();

app
    .UseRouting()
    .UseWebSockets()
    .UseEndpoints(endpoints => { endpoints.MapGraphQL(); });

app.Run();