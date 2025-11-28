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
[Route("api/accounts")]
[ApiController]
public class AccountsV1Controller : ControllerBase
{
    //private readonly ILogger<AccountsV1Controller> _logger;
    //private readonly IAccountsDbContext _db;

    //private readonly IAccountService _service;

    private readonly DtoApiControllerHelper<Account, DtoWithActiveQueryParameters> _dtoApi;

    public AccountsV1Controller(IAccountService service,
                                IAccountsDbContext db,
                                ILogger<AccountsV1Controller>? logger = null)
    {
        _dtoApi = new DtoApiControllerHelper<Account, DtoWithActiveQueryParameters>(this, service, db, logger);

        /*
        _logger = logger ?? NullLogger<AccountsV1Controller>.Instance;

        _db = db;
        // Ensure database seeding, e.g. for some API Unit tests
        try
        {
            _db.Database.EnsureCreated();
        }
        catch (Exception ex)
        {
            _logger.LogCritical("Error during database initialization: {error}", ex.Message);
            throw;
        }

        //_service = new AccountService(_db, _logger);
        _service = service;
        */
    }

    // GET: api/<AccountsController>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Account>>> Get(
        [FromQuery] DtoWithActiveQueryParameters? q = null)
    {
        return await _dtoApi.Get(HttpContext, q);
        /*
        using (_logger.BeginScope("Request ID: {requestID}", HttpContext?.TraceIdentifier ?? "--"))
        {
            IEnumerable<Account> accounts;

            try
            {
                _logger.LogInformation("Start of GET Accounts with parameters {q}", q);
                accounts = await _service.Get(q);
            }
            catch (Exception ex)
            {
                _logger.LogError("Get request failed: {error}", ex.Message);
                return BadRequest(ex);
            }

            _logger.LogInformation("End of GET Accounts.");
            return Ok(accounts);
        }
        */
    }

    // GET api/<AccountsController>/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Account>> Get([FromRoute] int id)
    {
        return await _dtoApi.Get(HttpContext, id);
        /*
        using (_logger.BeginScope("Request ID: {requestID}", HttpContext?.TraceIdentifier ?? "--"))
        {
            Account account;

            try
            {
                account = await _service.GetById(id);
            }
            catch (BadIdException ex)
            {
                var error = ex.GetErrorObject();
                _logger.LogError("Request failed: {error}", error.AsString());
                return BadRequest(error);
            }
            catch (DataNotFoundException ex)
            {
                var error = ex.GetErrorObject();
                _logger.LogWarning("Data not found: {error}", error.AsString());
                return NotFound(error);
            }
            catch (Exception ex)
            {
                _logger.LogError("Get request failed: {error}", ex.Message);
                return BadRequest(ex);
            }

            _logger.LogInformation("End of GET Accounts.");
            return Ok(account);
        }
        */
    }

    /* // GET api/<AccountsController>/True
    [HttpGet("active/{IsActive:bool}")]
    public async Task<ActionResult<Account>> Get(bool isActive)
    {
        var account = await _db.Accounts.Where(a => a.IsActive == isActive).ToListAsync();
        if (account == null)
        {
            return NotFound();
        }

        return Ok(account);
    }*/

    // POST api/<AccountsController>
    [HttpPost]
    public async Task<ActionResult<Account>> Post([FromBody] Account account)
    {
        return await _dtoApi.Post(HttpContext, account);
        /*
        using (_logger.BeginScope("Request ID: {requestID}", HttpContext?.TraceIdentifier ?? "--"))
        {
            try
            {
                var entriesWritten = await _service.Add(ModelState.IsValid, account);
            }
            catch (ModelStateInvalidException<Account> ex)
            {
                var error = ex.GetErrorObject();
                _logger.LogError("Post request model data failure: {error}", error.AsString());
                return BadRequest(error);
            }
            catch (DataException ex)
            {
                var error = ex.GetErrorObject();
                _logger.LogError("Post request data failure: {error}", error.AsString());
                return BadRequest(error);
            }
            catch (Exception ex)
            {
                _logger.LogError("Post request failed: {error}", ex.Message);
                return BadRequest(ex);
            }

            return CreatedAtAction(nameof(Get), new { id = account.Id }, account);
        }
        */
    }

    // PUT api/<AccountsController>/5
    [HttpPut("{id}")]
    public async Task<ActionResult> Put([FromRoute] int id, [FromBody] Account account)
    {
        return await _dtoApi.Put(HttpContext, id, account);
        /*
        using (_logger.BeginScope("Request ID: {requestID}", HttpContext?.TraceIdentifier ?? "--"))
        {
            try
            {
                var entriesWritten = await _service.Update(ModelState.IsValid, id, account);
            }
            catch (ModelStateInvalidException<Account> ex)
            {
                var error = ex.GetErrorObject();
                _logger.LogError("Put request model data failure: {error}", error.AsString());
                return BadRequest(error);
            }
            catch (BadIdException ex)
            {
                var error = ex.GetErrorObject();
                _logger.LogError("Put request failed: {error}", error.AsString());
                return BadRequest(error);
            }
            catch (DataException ex)
            {
                var error = ex.GetErrorObject();
                _logger.LogError("Put request data failure: {error}", error.AsString());
                return BadRequest(error);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!_db.Accounts.Any(a => a.Id == id))
                {
                    _logger.LogError("Concurrency violation due to updating not existing record: {error}", ex.Message);
                    return NotFound(ex);
                }
                _logger.LogError("Concurrency violation due to unknown reason: {error}", ex.Message);
                return BadRequest(ex);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError("Database updating error: {error}", ex.Message);
                return BadRequest(ex);
            }
            catch (Exception ex)
            {
                _logger.LogError("Post request failed: {error}", ex.Message);
                return BadRequest(ex);
            }

            return NoContent();
        }
        */
    }

    /* // PATCH api/<AccountsController>/5
    [HttpPatch("{id}")]
    public async Task<ActionResult> Patch([FromRoute] int id, [FromBody] JsonPatchDocument<Account> patchDoc)
    {
        using (_logger.BeginScope("Request ID: {requestID}", HttpContext?.TraceIdentifier ?? "--"))
        {
            try
            {
                var entriesWritten = await _service.Patch(id, patchDoc);
            }
            //catch (ModelStateInvalidException ex)
            //{
            //    var error = ex.GetErrorObject();
            //    _logger.LogError("Put request model data failure: {error}", error.AsString());
            //    return BadRequest(error);
            //}
            catch (BadIdException ex)
            {
                var error = ex.GetErrorObject();
                _logger.LogError("Put request failed: {error}", error.AsString());
                return BadRequest(error);
            }
            catch (DataException ex)
            {
                var error = ex.GetErrorObject();
                _logger.LogError("Put request data failure: {error}", error.AsString());
                return BadRequest(error);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!_db.Accounts.Any(a => a.Id == id))
                {
                    _logger.LogError("Concurrency violation due to updating not existing record: {error}", ex.Message);
                    return NotFound(ex);
                }
                _logger.LogError("Concurrency violation due to unknown reason: {error}", ex.Message);
                return BadRequest(ex);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError("Database updating error: {error}", ex.Message);
                return BadRequest(ex);
            }
            catch (Exception ex)
            {
                _logger.LogError("Post request failed: {error}", ex.Message);
                return BadRequest(ex);
            }

            return NoContent();
        }
    }*/

    // DELETE api/<AccountsController>/5
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete([FromRoute] int id)
    {
        return await _dtoApi.Delete(HttpContext, id);
        /*
        using (_logger.BeginScope("Request ID: {requestID}", HttpContext?.TraceIdentifier ?? "--"))
        {
            try
            {
                var entriesRemoved = await _service.RemoveById(id);
            }
            catch (BadIdException ex)
            {
                var error = ex.GetErrorObject();
                _logger.LogError("Delete request failed: {error}", error.AsString());
                return BadRequest(error);
            }
            catch (DataNotFoundException ex)
            {
                var error = ex.GetErrorObject();
                _logger.LogWarning("Data not found: {error}", error.AsString());
                return NotFound(error);
            }
            catch (Exception ex)
            {
                _logger.LogError("Delete request failed: {error}", ex.Message);
                return BadRequest(ex);
            }

            return Ok();
        }
        */
    }

}
