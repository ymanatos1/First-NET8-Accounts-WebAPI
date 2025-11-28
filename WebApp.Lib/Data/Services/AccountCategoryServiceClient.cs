using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WebAPI.Data.Models;
using WebAPI.Lib.Data.Services;
using WebAPI.Lib.WebAPI.Query;
using WebApp.Lib.Data.Services.Base;

namespace WebApp.Lib.Data.Services;

public class AccountCategoryServiceClient : DtoApiServiceClientBase<AccountCategory, DtoQueryParameters>, IAccountCategoryServiceClient
{
    public override string DtoName { get; } = "Account Category";
    public override string DtoNamePlural { get; } = "Account Categories";
    public override string DtoPath { get; } = "AccountCategories";

    public override string API_PATH { get; } = "api/accountcategories/";

    public AccountCategoryServiceClient(HttpClient http, ILogger<IAccountCategoryServiceClient>? logger, IConfiguration configuration)
        : base(http, logger, configuration)
    {
    }

    //public new async Task<IEnumerable<AccountCategory>> GetAsync()
    //{
    //    return await base.GetAsync();
    //}

    //public new async Task<AccountCategory?> GetByIdAsync(int id)
    //{
    //    return await base.GetByIdAsync(id);
    //}

    //public new async Task<AccountCategory?> AddAsync(AccountCategory entry)
    //{
    //    return await base.AddAsync(entry);
    //}

    //public new async Task<bool> UpdateAsync(int id, AccountCategory entry)
    //{
    //    return await base.UpdateAsync(id, entry);
    //}

    //public new async Task<bool> RemoveByIdAsync(int id)
    //{
    //    return await base.RemoveByIdAsync(id);
    //}

}
