using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using WebAPI.Data.Models;
using WebAPI.Lib.Data.Services;
using WebAPI.Lib.WebAPI.Query;

namespace WebApp.Lib.Data.Services.Base;

public abstract class DtoApiServiceClientBase<T, TT> : IDtoServiceClient<T, TT>
    where T : Dto, new()
    where TT : DtoQueryParameters
{
    public abstract string DtoName { get; }
    public abstract string DtoNamePlural { get; }
    public abstract string DtoPath { get; }

    public abstract string API_PATH { get; }


    private readonly HttpClient _http;
    private readonly ILogger? _logger;
    private readonly IConfiguration _configuration;

    private readonly string DEFAULT_API_URL = "https://localhost:7171/";
    private readonly string DEFAULT_API_VERSION = "1.0";
    //private const string API_PATH = "api/accounts/";

    public DtoApiServiceClientBase(HttpClient http, ILogger? logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;

        _http = http;
        _http.BaseAddress = new Uri(_configuration["Api:Uri"] ?? DEFAULT_API_URL);
        _http.DefaultRequestVersion = new Version(_configuration["Api:Version"] ?? DEFAULT_API_VERSION);
    }

    public T New()
    {
        return new T();
    }

    public async Task<IEnumerable<T>> GetAsync()
    //public async Task<IEnumerable<T>> GetAsync(TT? q = null)
    {
        var empty_entries = new List<T>();

        try
        {
            var response = await _http.GetAsync(API_PATH + "");
            response.EnsureSuccessStatusCode();

            var entries = await response.Content.ReadFromJsonAsync<IEnumerable<T>>();
            return entries ?? empty_entries;
        }
        catch (HttpRequestException ex)
        {
            _logger!.LogError(ex.Message);
        }
        catch (Exception ex)
        {
            _logger!.LogError(ex.Message);
        }

        return empty_entries;
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        try
        {
            var response = await _http.GetAsync(API_PATH + id);
            response.EnsureSuccessStatusCode();

            var entry = await response.Content.ReadFromJsonAsync<T>();
            return entry;
        }
        catch (HttpRequestException ex)
        {
            _logger!.LogError(ex.Message);
        }
        catch (Exception ex)
        {
            _logger!.LogError(ex.Message);
        }

        return null;
    }

    public async Task<T?> AddAsync(T entry)
    {
        try
        {
            var json = JsonSerializer.Serialize(entry);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _http.PostAsync(API_PATH + "", content);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Update successful!");
                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseBody);

                var createdEntry = await response.Content.ReadFromJsonAsync<T>();
                return createdEntry;    // SUCCESS
            }
            else
            {
                Console.WriteLine($"Update failed: {response.StatusCode}");
                Console.WriteLine(await response.Content.ReadAsStringAsync());
            }
        }
        catch (HttpRequestException ex)
        {
            _logger!.LogError(ex.Message);
        }
        catch (Exception ex)
        {
            _logger!.LogError(ex.Message);
        }

        return null;    // FAILED!
    }

    public async Task<bool> UpdateAsync(int id, T entry)
    {
        try
        {
            var json = JsonSerializer.Serialize(entry);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _http.PutAsync(API_PATH + id, content);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Update successful!");
                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseBody);

                return true;    // SUCCESS
            }
            else
            {
                Console.WriteLine($"Update failed: {response.StatusCode}");
                Console.WriteLine(await response.Content.ReadAsStringAsync());
            }
        }
        catch (HttpRequestException ex)
        {
            _logger!.LogError(ex.Message);
        }
        catch (Exception ex)
        {
            _logger!.LogError(ex.Message);
        }

        return false;    // FAILED!
    }

    public async Task<bool> RemoveByIdAsync(int id)
    {
        try
        {
            HttpResponseMessage response = await _http.DeleteAsync(API_PATH + id);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Delete successful!");
                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseBody);

                return true;    // SUCCESS
            }
            else
            {
                Console.WriteLine($"Delete failed: {response.StatusCode}");
                Console.WriteLine(await response.Content.ReadAsStringAsync());
            }
        }
        catch (HttpRequestException ex)
        {
            _logger!.LogError(ex.Message);
        }
        catch (Exception ex)
        {
            _logger!.LogError(ex.Message);
        }

        return false;    // FAILED!
    }

}
