using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WebAPI.Data.Models;
using WebAPI.Lib.Data.Services;
using WebAPI.Lib.WebAPI.Query;
using WebApp.Lib.Data.Services.Base;

namespace WebApp.Lib.Data.Services;

public class AccountServiceClient : DtoApiServiceClientBase<Account, DtoWithActiveQueryParameters>, IAccountServiceClient
{
    public override string DtoName { get; } = "Account";
    public override string DtoNamePlural { get; } = "Accounts";
    public override string DtoPath { get; } = "Accounts";

    /* //public override string DtoName { get; } = "Account";
    //public override string DtoNamePlural { get; } = "Accounts";
    public string DtoName { get; } = "Account";
    public string DtoNamePlural { get; } = "Accounts";

    private readonly HttpClient _http;
    private readonly ILogger<IAccountServiceClient>? _logger;
    private readonly IConfiguration _configuration;

    private readonly string API_URL = "https://localhost:7171/";
    private readonly string API_VERSION = "1.0";
    private const string API_PATH = "api/accounts/";*/

    public override string API_PATH { get; } = "api/accounts/";

    public AccountServiceClient(HttpClient http, ILogger<IAccountServiceClient>? logger, IConfiguration configuration)
        : base(http, logger, configuration)
    {
        /*
        _logger = logger;
        _configuration = configuration;

        _http = http;
        _http.BaseAddress = new Uri(_configuration["Api:Uri"] ?? API_URL);
        _http.DefaultRequestVersion = new Version(_configuration["Api:Version"] ?? API_VERSION);
        */
    }

    //public new async Task<IEnumerable<Account>> GetAsync()
    //{
    //    return await base.GetAsync();
        /*
        var empty_accounts = new List<Account>();

        try
        {
            var response = await _http.GetAsync(API_PATH + "");
            response.EnsureSuccessStatusCode();

            var accounts = await response.Content.ReadFromJsonAsync<IEnumerable<Account>>();
            return accounts ?? empty_accounts;
        }
        catch (HttpRequestException ex)
        {
            _logger!.LogError(ex.Message);
        }
        catch (Exception ex)
        {
            _logger!.LogError(ex.Message);
        }

        return empty_accounts;*/
    //}

    //public new async Task<Account?> GetByIdAsync(int id)
    //{
    //    return await base.GetByIdAsync(id);
        /*
        try
        {
            var response = await _http.GetAsync(API_PATH + id);
            response.EnsureSuccessStatusCode();

            var account = await response.Content.ReadFromJsonAsync<Account>();
            return account;
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
        */
    //}

    //public new async Task<Account?> AddAsync(Account entry)
    //{
    //    return await base.AddAsync(entry);
        /*
        try
        {
            var json = JsonSerializer.Serialize(account);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _http.PostAsync(API_PATH + "", content);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Update successful!");
                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseBody);

                var createdAccount = await response.Content.ReadFromJsonAsync<Account>();
                return createdAccount;    // SUCCESS
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
        */
    //}

    //public new async Task<bool> UpdateAsync(int id, Account entry)
    //{
    //    return await base.UpdateAsync(id, entry);
        /*
         * try
        {
            var json = JsonSerializer.Serialize(account);
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
        */
    //}

    //public new async Task<bool> RemoveByIdAsync(int id)
    //{
    //    return await base.RemoveByIdAsync(id);
        /*
         * try
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
        */
    //}

}
