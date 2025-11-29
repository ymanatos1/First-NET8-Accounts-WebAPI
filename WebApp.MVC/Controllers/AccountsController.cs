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
        return await ServeGetAsync(BuildVMAsync, true, VMBase.CRUD.CREATE);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateQuick(AccountVM model)
    {
        return await ServeOperationAsync(model, LoadDropdownsAsync, true, VMBase.CRUD.CREATE);
    }


    [HttpGet]
    public async Task<IActionResult> Create()
    {
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
        return await ServeGetAsync(BuildVMAsync, true, VMBase.CRUD.READ, id);
    }


    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        return await ServeGetAsync(BuildVMAsync, false, VMBase.CRUD.READ, id);
    }


    // -------------------------------
    // EDIT CRUD SUPPORT
    // -------------------------------

    [HttpGet]
    public async Task<IActionResult> EditQuick(int id)
    {
        return await ServeGetAsync(BuildVMAsync, true, VMBase.CRUD.UPDATE, id);
    }

    //[HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditQuick(AccountVM model)
    {
        // DO NOT use model.EntryId here, its 0 in just posted model.
        return await ServeOperationAsync(model, LoadDropdownsAsync, true, VMBase.CRUD.UPDATE, model.Entry.Id);
    }


    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
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
        return await ServeGetAsync(BuildVMAsync, true, VMBase.CRUD.DELETE, id);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteQuick(AccountVM model)
    {
        // DO NOT use model.EntryId here, its 0 in just posted model.
        return await ServeOperationAsync(model, LoadDropdownsAsync, true, VMBase.CRUD.DELETE, model.Entry.Id);
    }


    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
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
                //: RedirectToAction("Index");
                : return RedirectToAction(nameof(Index), "Accounts");
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
