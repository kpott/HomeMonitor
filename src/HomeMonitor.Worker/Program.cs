using HomeMonitor.Worker;
using HomeMonitor.Worker.Options;
using MassTransit;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddHostedService<BusWorker>();
        services.AddHostedService<TemperatureMonitor>();

        services.AddOptions<SensorSettings>()
            .Bind(context.Configuration.GetSection(SensorSettings.ConfigurationSectionName));

        var rabbitMqOptions = new HomeMonitor.Worker.Options.RabbitMqSettings();
        context.Configuration.GetSection(HomeMonitor.Worker.Options.RabbitMqSettings.ConfigurationSectionName)
            .Bind(rabbitMqOptions);

        services.AddMassTransit(serviceCollectionConfigurator =>
        {
            serviceCollectionConfigurator.AddBus(serviceProvider =>
                Bus.Factory.CreateUsingRabbitMq(busFactoryConfigurator =>
                {
                    busFactoryConfigurator.Host(rabbitMqOptions.Uri, h =>
                    {
                        h.Username(rabbitMqOptions.Username);
                        h.Password(rabbitMqOptions.Password);
                    });
                }));
        });
    })
    .Build();

await host.RunAsync();