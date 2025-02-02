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

var host = builder.Build();
host.Run();
