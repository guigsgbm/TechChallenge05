using Infrastructure.Messaging;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly ItemMessaging _itemMessaging;

    public Worker(ILogger<Worker> logger, ItemMessaging itemMessaging)
    {
        _logger = logger;
        _itemMessaging = itemMessaging;
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        _itemMessaging.StartMessageProcessing();
        return base.StartAsync(cancellationToken);
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _itemMessaging.StopMessageProcessing();
        return base.StopAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }

            await Task.Delay(5000, stoppingToken);
        }
    }
}
