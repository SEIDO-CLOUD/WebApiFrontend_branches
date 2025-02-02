using System;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

using Models;
using Models.DTO;

namespace Services;


public class AdminServiceWapi : IAdminService {

    private readonly ILogger<AdminServiceWapi> _logger; 
    private readonly HttpClient _httpClient;
   
    
    public AdminServiceWapi(ILogger<AdminServiceWapi> logger, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClient = httpClientFactory.CreateClient(name: "ZooWebApi");

    }

    public async Task<AdminInfo> AdminInfoAsync() 
    {
        string uri = $"admin/info";

        //Send the HTTP Message and await the repsonse
        HttpResponseMessage response = await _httpClient.GetAsync(uri);

        //Throw an exception if the response is not successful
        response.EnsureSuccessStatusCode();

        //Get the resonse data
        string s = await response.Content.ReadAsStringAsync();
        var resp = JsonConvert.DeserializeObject<AdminInfo>(s);
        return resp;
    }

    public async Task<ResponseItemDto<GstUsrInfoAllDto>> SeedAsync(int nrOfItems)
    {
        string uri = $"admin/seed?count={nrOfItems}";

        //Send the HTTP Message and await the repsonse
        HttpResponseMessage response = await _httpClient.GetAsync(uri);

        //Throw an exception if the response is not successful
        response.EnsureSuccessStatusCode();

        //Get the response body
        string s = await response.Content.ReadAsStringAsync();
        var info = JsonConvert.DeserializeObject<ResponseItemDto<GstUsrInfoAllDto>>(s);
        return info;
    }
    public async Task<ResponseItemDto<GstUsrInfoAllDto>> RemoveSeedAsync(bool seeded)
    {
        string uri = $"admin/removeseed?seeded={seeded}";

        //Send the HTTP Message and await the repsonse
        HttpResponseMessage response = await _httpClient.GetAsync(uri);

        //Throw an exception if the response is not successful
        response.EnsureSuccessStatusCode();

        //Get the response body
        string s = await response.Content.ReadAsStringAsync();
        var info = JsonConvert.DeserializeObject<ResponseItemDto<GstUsrInfoAllDto>>(s);
        return info;
    }

        public async Task<ResponseItemDto<GstUsrInfoAllDto>> InfoAsync() 
    {
        string uri = $"guest/info";

        //Send the HTTP Message and await the repsonse
        HttpResponseMessage response = await _httpClient.GetAsync(uri);

        //Throw an exception if the response is not successful
        response.EnsureSuccessStatusCode();

        //Get the resonse data
        string s = await response.Content.ReadAsStringAsync();
        var resp = JsonConvert.DeserializeObject<ResponseItemDto<GstUsrInfoAllDto>>(s);
        return resp;
    }
}

public class AbstractConverter<TReal, TAbstract> 
    : JsonConverter where TReal : TAbstract
{
    public override Boolean CanConvert(Type objectType)
        => objectType == typeof(TAbstract);

    public override Object ReadJson(JsonReader reader, Type type, Object value, JsonSerializer jser)
        => jser.Deserialize<TReal>(reader);

    public override void WriteJson(JsonWriter writer, Object value, JsonSerializer jser)
        => jser.Serialize(writer, value);
}