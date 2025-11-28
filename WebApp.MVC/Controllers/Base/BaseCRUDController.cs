using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Reflection;
using System.Text.Json;
using WebAPI.Data.Models;
using WebAPI.Lib.Data.Services;
using WebAPI.Lib.WebAPI.Query;
using WebApp.Lib.Data.ViewModels.Base;
using static WebApp.Lib.Data.ViewModels.Base.VMBase;

namespace WebApp.MVC.Controllers;

public abstract class BaseCRUDController<Tdto, Tvm, TdtoQp, Tsrvc> : Controller
    where Tdto : Dto, new()
    where Tvm : DtoVM<Tdto>
    where TdtoQp : DtoQueryParameters
    where Tsrvc : IDtoServiceClient<Tdto, TdtoQp>
{
    public string Path { get; } // = "Accounts";

    protected readonly IDtoServiceClient<Tdto, TdtoQp> _service;
    protected readonly ILogger<BaseCRUDController<Tdto, Tvm, TdtoQp, Tsrvc>> _logger;

    public BaseCRUDController(
        IDtoServiceClient<Tdto, TdtoQp> service,
        ILogger<BaseCRUDController<Tdto, Tvm, TdtoQp, Tsrvc>> logger
        )
    {
        _service = service;
        _logger = logger;

        Path = _service.DtoPath;
    }

    public async Task<IActionResult> ServeGetAsync(
        Func<VMBase.CRUD, bool, Tdto?, Task<Tvm>> modelBuilder,
        bool isModal, VMBase.CRUD crud, int id = 0)
    {
        Tvm model;

        if (id == 0)
        {
            model = await modelBuilder(crud, isModal, null);

            return isModal 
                ? PartialView(model.PARTIAL_NAME, model) 
                : View(model);  // full page
        }
        else
        {
            var entry = await _service.GetByIdAsync(id);

            if (entry != null)
            {
                model = await modelBuilder(crud, isModal, entry);

                return isModal
                    ? PartialView(model.PARTIAL_NAME, model)
                    : View(model);  // full page
            }

            return RedirectToAction("Index");
        }
    }

    public async Task<IActionResult> ServeOperationAsync(
        Tvm model, Func<Tvm, Task>? showPopupErrorInit,
        bool isModal, VMBase.CRUD crud, int id = 0)
    {
        string? error;

        model.UpdateBase(crud, _service, isModal, Path);

        var modelStateIsValid =
            model.Entry != null &&
            ((crud == CRUD.DELETE && model.Entry.Id > 0) || ModelState.IsValid);
        if (!modelStateIsValid)
        {
            error = $"{_service.DtoName} IS NULL OR ModelState invalid!";
            return await showPopupErrorAsync(model, showPopupErrorInit, error);
        }

        error = await operateAsync(crud, id, model);
        //if (!success)
        if (error != null)
            return await showPopupErrorAsync(model, showPopupErrorInit, error);

        return await showPopupErrorAsync(model);
    }


    private async Task<string?> operateAsync(VMBase.CRUD crud, int id, Tvm model)
    {
        switch (crud)
        {
            case VMBase.CRUD.CREATE:
                var createdEntry = await _service.AddAsync(model.Entry);
                return (createdEntry?.Id ?? 0) > 0 ? null : $"New {_service.DtoName} not created due to error!";

            case VMBase.CRUD.UPDATE:
                var successU = await _service.UpdateAsync(id, model.Entry);
                return successU ? null : $"{_service.DtoName} entry not updated due to error!";

            case VMBase.CRUD.DELETE:
                var successD = await _service.RemoveByIdAsync(id);
                return successD ? null : $"{_service.DtoName} entry not deleted due to error!";

            default: return $"ERROR: Unsupported operation ({crud.GetType().Name}={crud})!";
        }
    }

    private async Task<IActionResult> showPopupErrorAsync(
        Tvm model, Func<Tvm, Task>? showPopupErrorInit = null, string? error = null)
    {
        if (error != null && showPopupErrorInit != null)
            await showPopupErrorInit(model);

        model.Error = error;

        if (model.IsModal)
        {
            return error != null
                ? PartialView(model.PARTIAL_NAME, model)
                : model.SuccessJson(model.Entry);
        }
        else
        {
            if (error == null)
            {
                TempData["Toast"] = JsonSerializer.Serialize(model.SuccessJson(model.Entry).Value);
            }
            return error != null
                ? View(model.PAGE_NAME, model)
                : RedirectToAction("Index");
        }
    }


}
