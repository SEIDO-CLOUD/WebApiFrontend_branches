using Services;
using WorkerServiceFrontend;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.AddHttpClient(name: "ZooWebApi", configureClient: options =>
{
    options.BaseAddress = new Uri(builder.Configuration["DataService:WebApiBaseUri"]);
    options.DefaultRequestHeaders.Accept.Add(
        new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(
            mediaType: "application/json",
            quality: 1.0));
});

builder.Services.AddSingleton<IAdminService, AdminServiceWapi>();
builder.Services.AddSingleton<IZooService, ZooServiceWapi>();
builder.Services.AddSingleton<ILoginService, LoginServiceWapi>();

var host = builder.Build();
host.Run();
