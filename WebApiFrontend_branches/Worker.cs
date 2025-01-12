namespace WorkerServiceFrontend;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IHost _host;

    public Worker(ILogger<Worker> logger, IHost host)
    {
        _logger = logger;
        _host = host;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine($"Hello world from {nameof(WorkerServiceFrontend)}!");
        await _host.StopAsync();
    }
}
