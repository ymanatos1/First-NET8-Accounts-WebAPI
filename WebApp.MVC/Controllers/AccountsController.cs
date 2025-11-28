using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using WebAPI.Data.Models;
using WebAPI.Lib.Data.Services;
using WebAPI.Lib.WebAPI.Query;
using WebApp.Lib.Data.ViewModels;
using WebApp.Lib.Data.ViewModels.Base;

namespace WebApp.MVC.Controllers;

//[Route("[controller]/[action]")]
public class AccountsController : BaseCRUDController<Account, AccountVM, DtoWithActiveQueryParameters, IAccountServiceClient>
{
    //public readonly string Path = "Accounts";

    private readonly (
            IAccountServiceClient AccountsService,
            IAccountCategoryServiceClient AccountCategoriesService
        ) _services;


    public AccountsController(
        IAccountServiceClient serviceAccount,
        IAccountCategoryServiceClient serviceAccountCategory,
        ILogger<AccountsController> logger)
        : base(serviceAccount, logger)
    {
        _services = (serviceAccount, serviceAccountCategory);
    }


    public async Task<IActionResult> IndexAsync()
    {
        var model = new AccountsVM(VMBase.CRUD.LIST, _service, false, Path,
            await _services.AccountsService.GetAsync(),
            await _services.AccountCategoriesService.GetAsync()
        );

        return View(model);
    }


    // -------------------------------
    // CREATE CRUD SUPPORT
    // -------------------------------

    [HttpGet]
    public async Task<IActionResult> CreateQuick()
    {
        /*var model = await BuildVMAsync(VMBase.CRUD.CREATE);

        return PartialView(model.PARTIAL_NAME, model);

        return CreateGetQuickBase(await BuildVMAsync(VMBase.CRUD.CREATE));*/

        return await ServeGetAsync(BuildVMAsync, true, VMBase.CRUD.CREATE);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateQuick(AccountVM model)
    {
        /*model.UpdateBase(VMBase.CRUD.CREATE, _services.AccountsService, true, Path);

        if (model.Entry == null || !ModelState.IsValid)
            return await showPopupErrorAsync(model, $"{_services.AccountsService.DtoName} IS NULL OR ModelState invalid!");

        var createdEntry = await _services.AccountsService.AddAsync(model.Entry);
        if ((createdEntry?.Id ?? 0) <= 0)
            return await showPopupErrorAsync(model, $"New {_services.AccountsService.DtoName} not added due to error!");

        return await showPopupErrorAsync(model);*/

        return await ServeOperationAsync(model, LoadDropdownsAsync, true, VMBase.CRUD.CREATE);
    }


    [HttpGet]
    public async Task<IActionResult> Create()
    {
        /*var model = await BuildVMAsync(VMBase.CRUD.CREATE);

        return View(model); // full page

        return CreateGetBase(await BuildVMAsync(VMBase.CRUD.CREATE));*/
        
        return await ServeGetAsync(BuildVMAsync, false, VMBase.CRUD.CREATE);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(AccountVM model)
    {
        return await ServeOperationAsync(model, LoadDropdownsAsync, false, VMBase.CRUD.CREATE);
    }


    // -------------------------------
    // DETAILS CRUD SUPPORT
    // -------------------------------

    [HttpGet]
    public async Task<IActionResult> DetailsQuick(int id)
    {
        /*var entry = await _services.AccountsService.GetByIdAsync(id);

        if (entry != null)
        {
            var model = await BuildVMAsync(VMBase.CRUD.READ, entry);

            return PartialView(model.PARTIAL_NAME, model);
        }

        return RedirectToAction("Index");*/

        return await ServeGetAsync(BuildVMAsync, true, VMBase.CRUD.READ, id);
    }


    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        /*var entry = await _services.AccountsService.GetByIdAsync(id);

        if (entry != null)
        {
            var model = await BuildVMAsync(VMBase.CRUD.READ, entry);

            return View(model); // full page
        }

        return RedirectToAction("Index");*/

        return await ServeGetAsync(BuildVMAsync, false, VMBase.CRUD.READ, id);
    }


    // -------------------------------
    // EDIT CRUD SUPPORT
    // -------------------------------

    [HttpGet]
    public async Task<IActionResult> EditQuick(int id)
    {
        /*var entry = await _services.AccountsService.GetByIdAsync(id);

        if (entry != null)
        {
            var model = await BuildVMAsync(VMBase.CRUD.UPDATE, entry);

            return PartialView(model.PARTIAL_NAME, model);
        }

        return RedirectToAction("Index");*/

        return await ServeGetAsync(BuildVMAsync, true, VMBase.CRUD.UPDATE, id);
    }

    //[HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditQuick(AccountVM model)
    {
        /*model.UpdateBase(VMBase.CRUD.UPDATE, _services.AccountsService, true, Path);

        if (model.Entry == null || !ModelState.IsValid)
            return await showPopupErrorAsync(model, $"{_services.AccountsService.DtoName} IS NULL OR ModelState invalid!");

        var success = await _services.AccountsService.UpdateAsync(model.Entry.Id, model.Entry);
        if (!success)
            return await showPopupErrorAsync(model, $"New {_services.AccountsService.DtoName} not updated due to error!");

        return await showPopupErrorAsync(model);*/

        // DO NOT use model.EntryId here, its 0 in just posted model.
        return await ServeOperationAsync(model, LoadDropdownsAsync, true, VMBase.CRUD.UPDATE, model.Entry.Id);
    }


    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        /*var entry = await _services.AccountsService.GetByIdAsync(id);

        if (entry != null)
        {
            var model = await BuildVMAsync(VMBase.CRUD.UPDATE, entry);

            return View(model); // full page
        }

        return RedirectToAction("Index");*/

        return await ServeGetAsync(BuildVMAsync, false, VMBase.CRUD.UPDATE, id);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(AccountVM model)
    {
        // DO NOT use model.EntryId here, its 0 in just posted model.
        return await ServeOperationAsync(model, LoadDropdownsAsync, false, VMBase.CRUD.UPDATE, model.Entry.Id);
    }


    // -------------------------------
    // DELETE CRUD SUPPORT
    // -------------------------------

    [HttpGet]
    public async Task<IActionResult> DeleteQuick(int id)
    {
        /*var entry = await _services.AccountsService.GetByIdAsync(id);

        if (entry != null)
        {
            var model = await BuildVMAsync(VMBase.CRUD.DELETE, entry);

            return PartialView(model.PARTIAL_NAME, model);
        }

        return RedirectToAction("Index");*/

        return await ServeGetAsync(BuildVMAsync, true, VMBase.CRUD.DELETE, id);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteQuick(AccountVM model)
    {
        /*model.UpdateBase(VMBase.CRUD.DELETE, _services.AccountsService, true, Path);

        if (model.Entry == null || model.Entry.Id <= 0)
            return await showPopupErrorAsync(model, $"{_services.AccountsService.DtoName} IS NULL OR ModelState invalid!");

        var success = await _services.AccountsService.RemoveByIdAsync(model.Entry.Id);
        if (!success)
            return await showPopupErrorAsync(model, $"New {_services.AccountsService.DtoName} not deleted due to error!");

        return await showPopupErrorAsync(model);*/

        // DO NOT use model.EntryId here, its 0 in just posted model.
        return await ServeOperationAsync(model, LoadDropdownsAsync, true, VMBase.CRUD.DELETE, model.Entry.Id);
    }


    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        /*var entry = await _services.AccountsService.GetByIdAsync(id);

        if (entry != null)
        {
            var model = await BuildVMAsync(VMBase.CRUD.DELETE, entry);

            return View(model); // full page
        }

        return RedirectToAction("Index");*/

        return await ServeGetAsync(BuildVMAsync, false, VMBase.CRUD.DELETE, id);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(AccountVM model)
    {
        // DO NOT use model.EntryId here, its 0 in just posted model.
        return await ServeOperationAsync(model, LoadDropdownsAsync, false, VMBase.CRUD.DELETE, model.Entry.Id);
    }




    private async Task LoadDropdownsAsync(AccountVM model)
    {
        model.CategoryList = await _services.AccountCategoriesService.GetAsync();
    }

    /*private async Task<IActionResult> showPopupErrorAsync(AccountVM model, string? error = null)
    {
        if (error != null)
            await LoadDropdownsAsync(model);

        //PopupError = error;
        model.Error = error;

        if (model.IsModal)
        {
            return error != null
                //? Partial(PARTIAL_NAME, this)
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
                //? Partial(PARTIAL_NAME, this)
                ? View(model.PAGE_NAME, model)
                : RedirectToAction("Index");
        }
        //: new JsonResult(new
        //  {
        //      success = true,
        //      toastType = "success",  // CREATE: success, UPDATE: info, DELETE: danger, WARNING: warning
        //      icon = "check-circle",  // CREATE; check-circle, UPDATE: pencil, DELETE: trash, WARNING: exclamation-triangle, INFO: info-circle
        //      message = $"{_service.AccountsService.DtoName} '{model.Entry.Name}' created successfully with Id #{model.Entry.Id}!"
        //  });
    }*/


    private async Task<AccountVM> BuildVMAsync(VMBase.CRUD crud, bool isModal, Account? entry = null)
    {
        return new AccountVM(crud, _services.AccountsService, isModal, Path,
            entry ?? _services.AccountsService.New(),
            await _services.AccountCategoriesService.GetAsync()
        );
    }

}
