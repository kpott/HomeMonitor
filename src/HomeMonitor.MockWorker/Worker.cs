using MassTransit;

namespace HomeMonitor.MockWorker;

public class BusWorker : IHostedService
{
    private readonly ILogger<BusWorker> _logger;
    private readonly IBusControl _bus;

    public BusWorker(ILogger<BusWorker> logger, IBusControl bus)
    {
        _logger = logger;
        _bus = bus;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting the bus...");
        return _bus.StartAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stopping the bus...");
        return _bus.StopAsync(cancellationToken);
    }
}