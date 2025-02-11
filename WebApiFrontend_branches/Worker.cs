using Services;

namespace WorkerServiceFrontend;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IHost _host;
    ITestEndpointAccess _testEndpointAccess;

    public Worker(ILogger<Worker> logger, IHost host, ITestEndpointAccess testEndpointAccess)
    {
        _logger = logger;
        _host = host;
        _testEndpointAccess = testEndpointAccess;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("JWT CRUD test suite started");

        try
        {
            await _testEndpointAccess.ExecuteTestsAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError($"{ex.Message}.{ex.InnerException?.Message}");
            _logger.LogError("JWT CRUD test suite failed");
        }

        _logger.LogInformation("JWT CRUD test suite ended");
        await _host.StopAsync();
    }
}
