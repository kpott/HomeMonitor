using HomeMonitor.MockWorker;
using MassTransit;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddHostedService<BusWorker>();
        services.AddHostedService<MockTemperatureMonitor>();

        var rabbitMqOptions = new HomeMonitor.MockWorker.Options.RabbitMqSettings();
        context.Configuration.GetSection(HomeMonitor.MockWorker.Options.RabbitMqSettings.ConfigurationSectionName)
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