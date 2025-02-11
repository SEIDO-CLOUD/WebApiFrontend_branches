using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

using Models.DTO;
namespace Services;

public class TestEndpointAccess : ITestEndpointAccess
{
    private readonly ILogger<TestEndpointAccess> _logger;
    private readonly IAdminService _adminService;
    private readonly IZooService _zooService;
    private readonly ILoginService _loginService;

    public TestEndpointAccess(ILogger<TestEndpointAccess> logger,IAdminService adminService, IZooService zooService, ILoginService loginService)
    {
        _logger = logger;
        _adminService = adminService;
        _zooService = zooService;
        _loginService = loginService;
    }

    public async Task ExecuteTestsAsync()
    {
        JsonSerializerSettings settings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented // This enables indentation and newlines
        };
        _logger.LogInformation("TestEndpointAccess suite started");

        var creds = new LoginCredentialsDto(){UserNameOrEmail = "sysadmin1", Password="sysadmin1"};

        try
        {
            var token = await LoginAccess(settings, creds);
            _adminService.BearerToken = token;
            _zooService.BearerToken = token;

            await AdminAccess(settings);
            await ReadAccess(settings);
            await UpdateAccess(settings);
            await CreateAccess(settings);
            await DeleteAccess(settings);

            _logger.LogInformation("All tests completed successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError($"{ex.Message}.{ex.InnerException?.Message}");
            _logger.LogError("TestEndpointAccess failed");
        }

        _logger.LogInformation("TestEndpointAccess suite ended");
    }

    private async Task CreateAccess(JsonSerializerSettings settings)
    {
        _logger.LogInformation($"Test: {nameof(_zooService.CreateZooAsync)}");
        var item = new ZooCuDto();

        item.Name = "Martins created empty Zoo";
        item.City = "Martins City";
        item.Country = "Martins Country";

        var zoo = await _zooService.CreateZooAsync(item);
        _logger.LogTrace(JsonConvert.SerializeObject(zoo, settings));
    }

    private async Task UpdateAccess(JsonSerializerSettings settings)
    {
        _logger.LogInformation($"Test: {nameof(_zooService.UpdateZooAsync)}");
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
        _logger.LogInformation($"Test: {nameof(_zooService.ReadZoosAsync)}");
        var respItems = await _zooService.ReadZoosAsync(true, true, null, 0, 5);
        _logger.LogTrace(JsonConvert.SerializeObject(respItems, settings));


        _logger.LogTrace($"Test: {nameof(_zooService.ReadZooAsync)}");
        var respItem = await _zooService.ReadZooAsync(respItems.PageItems[0].ZooId, false);
        _logger.LogTrace(JsonConvert.SerializeObject(respItem, settings));
        if (respItem.Item.ZooId != respItems.PageItems[0].ZooId)
        {
            _logger.LogError($"Read error in {nameof(_zooService.ReadZooAsync)}");
            _logger.LogError(JsonConvert.SerializeObject(respItem, settings));
        }
    }

    private async Task DeleteAccess(JsonSerializerSettings settings)
    {
        _logger.LogInformation($"Test: {nameof(_zooService.DeleteZooAsync)}");
        var respItems = await _zooService.ReadZoosAsync(true, true, null, 0, 5);
        var respItem = await _zooService.DeleteZooAsync(respItems.PageItems[0].ZooId);

        try 
        {
            await _zooService.ReadZooAsync(respItems.PageItems[0].ZooId, false);

            //I should reach this place, as item should not exist
            _logger.LogError($"Delete error in {nameof(_zooService.DeleteZooAsync)}");
            _logger.LogTrace(JsonConvert.SerializeObject(respItem, settings));
        }
        catch (Exception ex)
        {
             _logger.LogDebug($"Successfully deleted item {respItems.PageItems[0].ZooId}");
        }
    }

    private async Task AdminAccess(JsonSerializerSettings settings)
    {
        _logger.LogInformation($"Test: {nameof(_adminService.AdminInfoAsync)}:");
        var adminInfo = await _adminService.AdminInfoAsync();
        _logger.LogTrace(JsonConvert.SerializeObject(adminInfo, settings));
        if (adminInfo.AppEnvironment != "Production")
        {
            _logger.LogError($"Environment error in {nameof(_adminService.AdminInfoAsync)}");
            _logger.LogError(JsonConvert.SerializeObject(adminInfo, settings));
        }

        _logger.LogInformation($"Test: {nameof(_adminService.InfoAsync)}:");
        var info = await _adminService.InfoAsync();
        _logger.LogTrace(JsonConvert.SerializeObject(info, settings));


        _logger.LogInformation($"Test: {nameof(_adminService.RemoveSeedAsync)}:");
        info = await _adminService.RemoveSeedAsync(true);
        _logger.LogTrace(JsonConvert.SerializeObject(info, settings));

        _logger.LogInformation($"Test: {nameof(_adminService.SeedAsync)}:");
        _logger.LogTrace(JsonConvert.SerializeObject(info, settings));
        info = await _adminService.SeedAsync(10);
        if (info.Item.Db.NrSeededZoos != 10)
        {
            _logger.LogError($"Seed error in {nameof(_adminService.SeedAsync)}");
            _logger.LogError(JsonConvert.SerializeObject(info, settings));
        }


    }

    private async Task<string> LoginAccess(JsonSerializerSettings settings, LoginCredentialsDto creds)
    {
        _logger.LogInformation($"Test: {nameof(_loginService.LoginUserAsync)}:");
        try 
        {
            var login = await _loginService.LoginUserAsync(creds);
            if (login.Item.UserName == creds.UserNameOrEmail)
            _logger.LogInformation($"Successfully logged in {login.Item.UserName}");

            return login.Item.JwtToken.EncryptedToken;
        }
        catch (Exception ex)
        {
            _logger.LogError($"{ex.Message}");
            throw;
        }
    }
}