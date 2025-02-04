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
        _logger.LogInformation("CRUD access started");

        await AdminAccess(settings);
        await ReadAccess(settings);
        await UpdateAccess(settings);
        await CreateAccess(settings);

        _logger.LogInformation("CRUD access finnished");
        await _host.StopAsync();
    }

    private async Task CreateAccess(JsonSerializerSettings settings)
    {
        _logger.LogTrace($"\n\n{nameof(_zooService.ReadZooDtoAsync)}: Create item");
        var item = new ZooCuDto();

        item.Name = "Martins created empty Zoo";
        item.City = "Martins City";
        item.Country = "Martins Country";

        var zoo = await _zooService.CreateZooAsync(item);
        _logger.LogTrace(JsonConvert.SerializeObject(zoo, settings));
    }

    private async Task UpdateAccess(JsonSerializerSettings settings)
    {
        _logger.LogTrace($"\n\n{nameof(_zooService.ReadZooDtoAsync)}: Update and Item");
        var respItems = await _zooService.ReadZoosAsync(true, true, null, 0, 5);
        var respItem = await _zooService.ReadZooDtoAsync(respItems.PageItems[0].ZooId, false);
        var oldName = respItem.Item.Name;

        respItem.Item.Name = "Martins updated empty Zoo";
        respItem.Item.City = "Martins City";
        respItem.Item.Country = "Martins Country";
        respItem.Item.AnimalsId = null;
        respItem.Item.EmployeesId = null;

        var zoo = await _zooService.UpdateZooAsync(respItem.Item);
        _logger.LogTrace(JsonConvert.SerializeObject(zoo, settings));
    }

    private async Task ReadAccess(JsonSerializerSettings settings)
    {
        _logger.LogTrace($"\n\n{nameof(_zooService.ReadZoosAsync)}: ReadItems page 0");
        var respItems = await _zooService.ReadZoosAsync(true, true, null, 0, 5);
        _logger.LogTrace(JsonConvert.SerializeObject(respItems, settings));


        _logger.LogTrace($"\n\n{nameof(_zooService.ReadZooAsync)}: ReadItem");
        respItems = await _zooService.ReadZoosAsync(true, true, null, 0, 5);
        var respItem = await _zooService.ReadZooAsync(respItems.PageItems[0].ZooId, false);
        _logger.LogTrace(JsonConvert.SerializeObject(respItem, settings));
        if (respItem.Item.ZooId != respItems.PageItems[0].ZooId)
        {
            _logger.LogError($"Read error in {nameof(_zooService.ReadZooAsync)}");
            _logger.LogError(JsonConvert.SerializeObject(respItem, settings));
        }
    }

    private async Task AdminAccess(JsonSerializerSettings settings)
    {
        _logger.LogTrace($"\n\n{nameof(_adminService.AdminInfoAsync)}:");
        var adminInfo = await _adminService.AdminInfoAsync();
        _logger.LogTrace(JsonConvert.SerializeObject(adminInfo, settings));
        if (adminInfo.AppEnvironment != "Production")
        {
            _logger.LogError($"Environment error in {nameof(_adminService.AdminInfoAsync)}");
            _logger.LogError(JsonConvert.SerializeObject(adminInfo, settings));
        }


        _logger.LogTrace($"\n\n{nameof(_adminService.InfoAsync)}:");
        var info = await _adminService.InfoAsync();
        _logger.LogTrace(JsonConvert.SerializeObject(info, settings));

        _logger.LogTrace($"\n\n{nameof(_adminService.RemoveSeedAsync)}:");
        info = await _adminService.RemoveSeedAsync(true);
        _logger.LogTrace(JsonConvert.SerializeObject(info, settings));

        _logger.LogTrace($"\n\n{nameof(_adminService.SeedAsync)}:");
        _logger.LogTrace(JsonConvert.SerializeObject(info, settings));
        info = await _adminService.SeedAsync(10);
        if (info.Item.Db.NrSeededZoos != 10)
        {
            _logger.LogError($"Seed error in {nameof(_adminService.SeedAsync)}");
            _logger.LogError(JsonConvert.SerializeObject(info, settings));
        }
    }
}
