using HomeMonitor.Worker;
using HomeMonitor.Worker.Options;
using HomeMonitor.Worker.Sensors;
using MassTransit;
using Microsoft.Extensions.Options;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddOptions<RabbitMqSettings>()
            .Bind(context.Configuration.GetSection(RabbitMqSettings.ConfigurationSectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddOptions<MonitorSettings>()
            .Bind(context.Configuration.GetSection(MonitorSettings.ConfigurationSectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddOptions<I2CSettings>()
            .Bind(context.Configuration.GetSection(I2CSettings.ConfigurationSectionName));

        var rabbitMqSettings = new RabbitMqSettings();
        context.Configuration.GetSection(RabbitMqSettings.ConfigurationSectionName)
            .Bind(rabbitMqSettings);

        var monitorSettings = new MonitorSettings();
        context.Configuration.GetSection(MonitorSettings.ConfigurationSectionName)
            .Bind(monitorSettings);

        services.AddHostedService<BusWorker>();
        services.AddHostedService<TemperatureMonitor>();

        services.AddSingleton<ITemperatureAndHumiditySensor, Bme280Proxy>();
        services.AddSingleton<ITemperatureAndHumiditySensor, Si7021Proxy>();
        services.AddSingleton<ITemperatureAndHumiditySensor, MockProxy>();

        services.AddSingleton<ITemperatureAndHumiditySensor>(serviceProvider =>
        {
            ITemperatureAndHumiditySensor? sensor = monitorSettings.SensorType.ToLower() switch
            {
                "bme280" => new Bme280Proxy(serviceProvider.GetRequiredService<ILogger<Bme280Proxy>>(),
                    serviceProvider.GetRequiredService<IOptions<I2CSettings>>()),
                "si7021" => new Si7021Proxy(serviceProvider.GetRequiredService<ILogger<Si7021Proxy>>(),
                    serviceProvider.GetRequiredService<IOptions<I2CSettings>>()),
                "mock" => new MockProxy(serviceProvider.GetRequiredService<ILogger<MockProxy>>()),
                _ => throw new NotSupportedException("Configured sensor not supported by worker")
            };

            return sensor ?? throw new NullReferenceException("Unable to create sensor proxy during startup");
        });

        services.AddMassTransit(serviceCollectionConfigurator =>
        {
            serviceCollectionConfigurator.AddBus(serviceProvider =>
                Bus.Factory.CreateUsingRabbitMq(busFactoryConfigurator =>
                {
                    busFactoryConfigurator.Host(rabbitMqSettings.Uri, h =>
                    {
                        h.Username(rabbitMqSettings.Username);
                        h.Password(rabbitMqSettings.Password);
                    });
                }));
        });
    })
    .Build();

await host.RunAsync();