using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Data.Models;
using WebAPI.Data.Models.Db;
using WebAPI.Infrastructure.Api;
using WebAPI.Lib.Data;
using WebAPI.Lib.Data.Services;
using WebAPI.Lib.WebAPI.Query;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers;

[ApiVersion("1.0")]
//[Route("api/[controller]")]
//[Route("api/v{version:apiVersion}/syscontent")]
[Route("api/syscontent")]
[ApiController]
public class SysContentV1Controller : ControllerBase
{
    private readonly DtoApiControllerHelper<Account, DtoWithActiveQueryParameters> _dtoApi;
    private readonly DtoApiControllerHelper<AccountCategory, DtoQueryParameters> _dtoApi2;

    public SysContentV1Controller(IAccountService service, IAccountCategoryService service2,
                                IAccountsDbContext db,
                                ILogger<SysContentV1Controller>? logger = null)
    {
        _dtoApi = new DtoApiControllerHelper<Account, DtoWithActiveQueryParameters>(this, service, db, logger);
        _dtoApi2 = new DtoApiControllerHelper<AccountCategory, DtoQueryParameters>(this, service2, db, logger);
    }

    // GET: api/<AccountsController>
    [HttpGet]
    public async Task<ActionResult<SysContent>> Get()
    {
        IEnumerable<Account>? accounts = null;
        IEnumerable<AccountCategory>? accountCategories = null;

        var accountsResult = await _dtoApi.Get(HttpContext);

        if (accountsResult.Result is OkObjectResult accountsOk)
            accounts = (IEnumerable<Account>?)accountsOk?.Value;

        var accountCategoriesResult = await _dtoApi2.Get(HttpContext);

        if (accountCategoriesResult.Result is OkObjectResult accountCategoriesOk)
            accountCategories = (IEnumerable<AccountCategory>?)accountCategoriesOk?.Value;

        var sc = new SysContent();
        sc.Accounts = accounts;
        sc.AccountCategories = accountCategories;

        return Ok(sc);
    }

}
