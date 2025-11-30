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

}
