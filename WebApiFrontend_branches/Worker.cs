using Services;

namespace WorkerServiceFrontend;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IHost _host;
    private IAdminService _adminService;

    public Worker(ILogger<Worker> logger, IHost host, IAdminService adminService)
    {
        _logger = logger;
        _host = host;
        _adminService = adminService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine($"Hello world from {nameof(WorkerServiceFrontend)}!");

        var adminInfo = await _adminService.AdminInfoAsync();
        Console.WriteLine(adminInfo.DataConnectionTag);

        var info = await _adminService.InfoAsync();
        Console.WriteLine(info.Item.Db.Title);

        await _host.StopAsync();
    }
}
