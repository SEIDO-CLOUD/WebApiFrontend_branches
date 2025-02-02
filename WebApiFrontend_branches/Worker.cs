using Microsoft.VisualBasic;
using Models.DTO;
using Newtonsoft.Json;
using Services;

namespace WorkerServiceFrontend;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IHost _host;
    private IAdminService _adminService;
    private IZooService _zooService;

    public Worker(ILogger<Worker> logger, IHost host, IAdminService adminService, IZooService zooService)
    {
        _logger = logger;
        _host = host;
        _adminService = adminService;
        _zooService = zooService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        JsonSerializerSettings settings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented // This enables indentation and newlines
        };

        await AdminAccess(settings);
        //await ReadAccess(settings);

        Console.WriteLine($"\n\n{nameof(_zooService.ReadZooDtoAsync)}: update first item");
        var respItems = await _zooService.ReadZoosAsync(true, true, null, 0, 5);
        var respItem = await _zooService.ReadZooDtoAsync(respItems.PageItems[0].ZooId, false);
        var oldName = respItem.Item.Name;
        var oldAnimals = respItem.Item.AnimalsId;

        respItem.Item.Name = "Martins renamed Zoo";
        var zoo = await _zooService.UpdateZooAsync(respItem.Item);
        Console.WriteLine(JsonConvert.SerializeObject(zoo, settings));

        System.Console.WriteLine("\n\n...and update back:");
        respItem.Item.Name = oldName;
        zoo = await _zooService.UpdateZooAsync(respItem.Item);
        Console.WriteLine(JsonConvert.SerializeObject(zoo, settings));


        await _host.StopAsync();
    }

    private async Task ReadAccess(JsonSerializerSettings settings)
    {
        Console.WriteLine($"\n\n{nameof(_zooService.ReadZoosAsync)}: page 0");
        var respItems = await _zooService.ReadZoosAsync(true, true, null, 0, 5);
        Console.WriteLine(JsonConvert.SerializeObject(respItems, settings));


        Console.WriteLine($"\n\n{nameof(_zooService.ReadZooAsync)}: first item");
        respItems = await _zooService.ReadZoosAsync(true, true, null, 0, 5);
        var respItem = await _zooService.ReadZooAsync(respItems.PageItems[0].ZooId, false);
        Console.WriteLine(JsonConvert.SerializeObject(respItem, settings));
    }

    private async Task AdminAccess(JsonSerializerSettings settings)
    {
        Console.WriteLine($"\n\n{nameof(_adminService.AdminInfoAsync)}:");
        var adminInfo = await _adminService.AdminInfoAsync();
        Console.WriteLine(JsonConvert.SerializeObject(adminInfo, settings));

        Console.WriteLine($"\n\n{nameof(_adminService.InfoAsync)}:");
        var info = await _adminService.InfoAsync();
        Console.WriteLine(JsonConvert.SerializeObject(info, settings));

        Console.WriteLine($"\n\n{nameof(_adminService.RemoveSeedAsync)}:");
        info = await _adminService.RemoveSeedAsync(true);
        Console.WriteLine(JsonConvert.SerializeObject(info, settings));

        Console.WriteLine($"\n\n{nameof(_adminService.SeedAsync)}:");
        info = await _adminService.SeedAsync(10);
        Console.WriteLine(JsonConvert.SerializeObject(info, settings));
    }
}
