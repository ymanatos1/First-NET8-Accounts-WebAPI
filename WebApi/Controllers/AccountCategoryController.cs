using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using System.Security.Principal;
using WebAPI.Data.Models;
using WebAPI.Data.Models.Db;
using WebAPI.Exceptions;
using WebAPI.Infrastructure.Api;
using WebAPI.Lib.Data.Services;
using WebAPI.Lib.WebAPI.Query;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers;

[ApiVersion("1.0")]
//[Route("api/[controller]")]
//[Route("api/v{version:apiVersion}/accounts")]
[Route("api/accountcategories")]
[ApiController]
public class AccountCategoriesV1Controller : ControllerBase
{
    private readonly DtoApiControllerHelper<AccountCategory, DtoQueryParameters> _dtoApi;

    public AccountCategoriesV1Controller(IAccountCategoryService service,
                                         IAccountsDbContext db,
                                         ILogger<AccountCategoriesV1Controller>? logger = null)
    {
        _dtoApi = new DtoApiControllerHelper<AccountCategory, DtoQueryParameters>(this, service, db, logger);
    }

    // GET: api/<AccountsController>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AccountCategory>>> Get(
        [FromQuery] DtoQueryParameters? q = null)
    {
        return await _dtoApi.Get(HttpContext, q);
    }

    // GET api/<AccountsController>/5
    [HttpGet("{id}")]
    public async Task<ActionResult<AccountCategory>> Get([FromRoute] int id)
    {
        return await _dtoApi.Get(HttpContext, id);
    }

    // POST api/<AccountsController>
    [HttpPost]
    public async Task<ActionResult<AccountCategory>> Post([FromBody] AccountCategory accountCategory)
    {
        return await _dtoApi.Post(HttpContext, accountCategory);
    }

    // PUT api/<AccountsController>/5
    [HttpPut("{id}")]
    public async Task<ActionResult> Put([FromRoute] int id, [FromBody] AccountCategory accountCategory)
    {
        return await _dtoApi.Put(HttpContext, id, accountCategory);
    }

    // DELETE api/<AccountsController>/5
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete([FromRoute] int id)
    {
        return await _dtoApi.Delete(HttpContext, id);
    }

}
