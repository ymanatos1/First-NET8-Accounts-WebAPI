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

    public override string API_PATH { get; } = "api/accounts/";

    public AccountServiceClient(HttpClient http, ILogger<IAccountServiceClient>? logger, IConfiguration configuration)
        : base(http, logger, configuration)
    {
    }

}
