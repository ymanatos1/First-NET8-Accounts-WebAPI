using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using WebAPI.Controllers;
using WebAPI.Data.Models;
using WebAPI.Data.Models.Db;
using WebAPI.Exceptions;
using WebAPI.Lib.Data.Services;
using WebAPI.Lib.WebAPI.Query;

namespace WebAPI.Infrastructure.Api;

public class DtoApiControllerHelper<T, TT>
    where T : Dto, new()
    where TT : DtoQueryParameters
{
    private readonly ControllerBase _controllerBase;

    private readonly ILogger<ControllerBase> _logger;
    private readonly IAccountsDbContext _db;

    private readonly IDtoService<T, TT> _service;

    public DtoApiControllerHelper(ControllerBase controllerBase,
                            IDtoService<T, TT> service,
                            IAccountsDbContext db,
                            ILogger<ControllerBase>? logger = null)
    {
        _controllerBase = controllerBase;

        _logger = logger ?? NullLogger<ControllerBase>.Instance;

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
    }

    public async Task<ActionResult<IEnumerable<T>>> Get(
        HttpContext? httpContext,
        TT? q = null)
    {
        using (_logger.BeginScope("Request ID: {requestID}", httpContext?.TraceIdentifier ?? "--"))
        {
            IEnumerable<T> entries;

            try
            {
                _logger.LogInformation($"Start of GET {_service.DtoNamePlural} with parameters {q}", q);
                entries = await _service.Get(q);
            }
            catch (Exception ex)
            {
                _logger.LogError("Get request failed: {error}", ex.Message);
                return _controllerBase.BadRequest(ex);
            }

            _logger.LogInformation($"End of GET {_service.DtoNamePlural} with parameters {q}", q);
            return _controllerBase.Ok(entries);
        }
    }

    public async Task<ActionResult<T>> Get(
        HttpContext? httpContext,
        int id)
    {
        using (_logger.BeginScope("Request ID: {requestID}", httpContext?.TraceIdentifier ?? "--"))
        {
            T entry;

            try
            {
                _logger.LogInformation($"Start of GET {_service.DtoNamePlural} with parameters {id}", id);
                entry = await _service.GetById(id);
            }
            catch (BadIdException ex)
            {
                var error = ex.GetErrorObject();
                _logger.LogError("Request failed: {error}", error.AsString());
                return _controllerBase.BadRequest(error);
            }
            catch (DataNotFoundException ex)
            {
                var error = ex.GetErrorObject();
                _logger.LogWarning("Data not found: {error}", error.AsString());
                return _controllerBase.NotFound(error);
            }
            catch (Exception ex)
            {
                _logger.LogError("Get request failed: {error}", ex.Message);
                return _controllerBase.BadRequest(ex);
            }

            _logger.LogInformation($"End of GET {_service.DtoNamePlural} with parameters {id}", id);
            return _controllerBase.Ok(entry);
        }
    }

    public async Task<ActionResult<T>> Post(
        HttpContext? httpContext,
        T entry)
    {
        using (_logger.BeginScope("Request ID: {requestID}", httpContext?.TraceIdentifier ?? "--"))
        {
            try
            {
                _logger.LogInformation($"Start of POST {_service.DtoNamePlural} with parameters {entry}", entry);
                var entriesWritten = await _service.Add(_controllerBase.ModelState.IsValid, entry);
            }
            catch (ModelStateInvalidException<T> ex)
            {
                var error = ex.GetErrorObject();
                _logger.LogError("Post request model data failure: {error}", error.AsString());
                return _controllerBase.BadRequest(error);
            }
            catch (DataException ex)
            {
                var error = ex.GetErrorObject();
                _logger.LogError("Post request data failure: {error}", error.AsString());
                return _controllerBase.BadRequest(error);
            }
            catch (Exception ex)
            {
                _logger.LogError("Post request failed: {error}", ex.Message);
                return _controllerBase.BadRequest(ex);
            }

            _logger.LogInformation($"End of POST {_service.DtoNamePlural} with parameters {entry}", entry);
            return _controllerBase.CreatedAtAction(nameof(Get), new { id = entry.Id }, entry);
        }
    }

    public async Task<ActionResult> Put(
        HttpContext? httpContext,
        int id, 
        T entry)
    {
        using (_logger.BeginScope("Request ID: {requestID}", httpContext?.TraceIdentifier ?? "--"))
        {
            try
            {
                _logger.LogInformation($"Start of PUT {_service.DtoNamePlural} with parameters {id} and {entry}", id, entry);
                var entriesWritten = await _service.Update(_controllerBase.ModelState.IsValid, id, entry);
            }
            catch (ModelStateInvalidException<T> ex)
            {
                var error = ex.GetErrorObject();
                _logger.LogError("Put request model data failure: {error}", error.AsString());
                return _controllerBase.BadRequest(error);
            }
            catch (BadIdException ex)
            {
                var error = ex.GetErrorObject();
                _logger.LogError("Put request failed: {error}", error.AsString());
                return _controllerBase.BadRequest(error);
            }
            catch (DataException ex)
            {
                var error = ex.GetErrorObject();
                _logger.LogError("Put request data failure: {error}", error.AsString());
                return _controllerBase.BadRequest(error);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                //if (!_service._dbSet.Any(a => a.Id == id))
                if (!_service.DBSetHasAny(id))
                {
                    _logger.LogError("Concurrency violation due to updating not existing record: {error}", ex.Message);
                    return _controllerBase.NotFound(ex);
                }
                _logger.LogError("Concurrency violation due to unknown reason: {error}", ex.Message);
                return _controllerBase.BadRequest(ex);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError("Database updating error: {error}", ex.Message);
                return _controllerBase.BadRequest(ex);
            }
            catch (Exception ex)
            {
                _logger.LogError("Post request failed: {error}", ex.Message);
                return _controllerBase.BadRequest(ex);
            }

            _logger.LogInformation($"End of PUT {_service.DtoNamePlural} with parameters {id} and {entry}", id, entry);
            return _controllerBase.NoContent();
        }
    }

    public async Task<ActionResult> Delete(
        HttpContext? httpContext, 
        int id)
    {
        using (_logger.BeginScope("Request ID: {requestID}", httpContext?.TraceIdentifier ?? "--"))
        {
            try
            {
                _logger.LogInformation($"Start of PUTDELETE {_service.DtoNamePlural} with parameters {id}", id);
                var entriesRemoved = await _service.RemoveById(id);
            }
            catch (BadIdException ex)
            {
                var error = ex.GetErrorObject();
                _logger.LogError("Delete request failed: {error}", error.AsString());
                return _controllerBase.BadRequest(error);
            }
            catch (DataNotFoundException ex)
            {
                var error = ex.GetErrorObject();
                _logger.LogWarning("Data not found: {error}", error.AsString());
                return _controllerBase.NotFound(error);
            }
            catch (Exception ex)
            {
                _logger.LogError("Delete request failed: {error}", ex.Message);
                return _controllerBase.BadRequest(ex);
            }

            _logger.LogInformation($"End of DELETE {_service.DtoNamePlural} with parameters {id}", id);
            return _controllerBase.Ok();
        }
    }

}
