using System;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

using Models;
using Models.DTO;

namespace Services;


public class ZooServiceWapi : IZooService {

    private readonly ILogger<AdminServiceWapi> _logger; 
    private readonly HttpClient _httpClient;

    //To ensure Json deserializern is using the class implementations instead of the Interfaces 
    readonly JsonSerializerSettings _jsonSettings = new JsonSerializerSettings
    {
        Converters = {
            new AbstractConverter<Zoo, IZoo>(),
            new AbstractConverter<Animal, IAnimal>(),
            new AbstractConverter<Employee, IEmployee>(),
            
        },
    };
    
    public ZooServiceWapi(ILogger<AdminServiceWapi> logger, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClient = httpClientFactory.CreateClient(name: "ZooWebApi");

    }

    #region Zoo CRUD
    public async Task<ResponsePageDto<IZoo>> ReadZoosAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize)
    {
        string uri = $"zoo/readitems?seeded={seeded}&flat={flat}&filter={filter}&pagenr={pageNumber}&pagesize={pageSize}";

        //Send the HTTP Message and await the repsonse
        HttpResponseMessage response = await _httpClient.GetAsync(uri);

        //Throw an exception if the response is not successful
        response.EnsureSuccessStatusCode();

        //Get the resonse data
        string s = await response.Content.ReadAsStringAsync();
        var resp = JsonConvert.DeserializeObject<ResponsePageDto<IZoo>>(s, _jsonSettings);
        return resp;
    }
    public async Task<ResponseItemDto<IZoo>> ReadZooAsync(Guid id, bool flat)
    {
        string uri = $"zoo/readitem?id={id}&flat={flat}";

        //Send the HTTP Message and await the repsonse
        HttpResponseMessage response = await _httpClient.GetAsync(uri);

        //Throw an exception if the response is not successful
        response.EnsureSuccessStatusCode();

        //Get the response body
        string s = await response.Content.ReadAsStringAsync();
        var resp = JsonConvert.DeserializeObject<ResponseItemDto<IZoo>>(s, _jsonSettings);
        return resp;
    }

    public async Task<ResponseItemDto<ZooCuDto>> ReadZooDtoAsync(Guid id, bool flat)
    {
        string uri = $"zoo/readitemdto?id={id}&flat={flat}";

        //Send the HTTP Message and await the repsonse
        HttpResponseMessage response = await _httpClient.GetAsync(uri);

        //Throw an exception if the response is not successful
        response.EnsureSuccessStatusCode();

        //Get the response body
        string s = await response.Content.ReadAsStringAsync();
        var resp = JsonConvert.DeserializeObject<ResponseItemDto<ZooCuDto>>(s, _jsonSettings);
        return resp;
    }
     public async Task<ResponseItemDto<IZoo>> DeleteZooAsync(Guid id)
    {
        string uri = $"zoo/deleteitem/{id}";

        //Send the HTTP Message and await the repsonse
        HttpResponseMessage response = await _httpClient.DeleteAsync(uri);

        //Throw an exception if the response is not successful
        response.EnsureSuccessStatusCode();

        //Get the response body
        string s = await response.Content.ReadAsStringAsync();
        var resp = JsonConvert.DeserializeObject<ResponseItemDto<IZoo>>(s, _jsonSettings);
        return resp;
    }
    public async Task<ResponseItemDto<IZoo>> UpdateZooAsync(ZooCuDto item)
    {
        string uri = $"zoo/updateitem/{item.ZooId}";

        //Prepare the request body
        string body = JsonConvert.SerializeObject(item);
        var requestContent = new StringContent(body, System.Text.Encoding.UTF8, "application/json");

        //Send the HTTP Message and await the repsonse
        HttpResponseMessage response = await _httpClient.PutAsync(uri, requestContent);

        //Throw an exception if the response is not successful
        response.EnsureSuccessStatusCode();

        //Get the response body
        string s = await response.Content.ReadAsStringAsync();
        var resp = JsonConvert.DeserializeObject<ResponseItemDto<IZoo>>(s, _jsonSettings);
        return resp;
    }
    public async Task<ResponseItemDto<IZoo>> CreateZooAsync(ZooCuDto item)
    {
        string uri = $"zoo/createitem";

        //Prepare the request content
        string body = JsonConvert.SerializeObject(item);
        var requestContent = new StringContent(body, System.Text.Encoding.UTF8, "application/json");

        //Send the HTTP Message and await the repsonse
        HttpResponseMessage response = await _httpClient.PostAsync(uri, requestContent);

        //Throw an exception if the response is not successful
        response.EnsureSuccessStatusCode();

        //Get the resonse data
        string s = await response.Content.ReadAsStringAsync();
        var resp = JsonConvert.DeserializeObject<ResponseItemDto<IZoo>>(s, _jsonSettings);
        return resp;
    }
    #endregion
}