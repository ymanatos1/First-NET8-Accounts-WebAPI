using Microsoft.AspNetCore.Mvc;
using WebAPI.Data.Models;
using WebAPI.Lib.Data.Services;
using WebAPI.Lib.WebAPI.Query;
using WebApp.Lib.Data.ViewModels;
using WebApp.Lib.Data.ViewModels.Base;

namespace WebApp.MVC.Controllers;

//[Route("[controller]/[action]")]
public class AccountCategoriesController : BaseCRUDController<AccountCategory, AccountCategoryVM, DtoQueryParameters, IAccountCategoryServiceClient>
{
    //public readonly string Path = "AccountCategories";

    //private readonly IAccountCategoryServiceClient _service;

    //private readonly Func<VMBase.CRUD, AccountCategory?, Task<AccountCategoryVM>> _syncBuildVM;

    public AccountCategoriesController(
        IAccountCategoryServiceClient service,
        ILogger<AccountCategoriesController> logger)
        : base(service, logger)
    {
        //_syncBuildVM = (crud, entry) => Task.FromResult(BuildVM(crud, entry));
    }


    public async Task<IActionResult> IndexAsync()
    {
        //_model.Entries = await _service.GetAsync();
        //_model.Path = Path;
        var model = new AccountCategoriesVM(VMBase.CRUD.LIST, _service, false, Path,
            await _service.GetAsync()
        );

        return View(model);
    }


    // -------------------------------
    // CREATE CRUD SUPPORT
    // -------------------------------

    [HttpGet]
    public async Task<IActionResult> CreateQuickAsync()
    {
        /*var model = BuildVM(VMBase.CRUD.CREATE);

        return PartialView(model.PARTIAL_NAME, model);

        return await ServeGet(BuildVM, true, VMBase.CRUD.CREATE);*/
            
        return await ServeGetAsync(BuildVMAsync, true, VMBase.CRUD.CREATE);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateQuick(AccountCategoryVM model)
    {
        /*model.UpdateBase(VMBase.CRUD.CREATE, _service, true, Path);

        if (model.Entry == null || !ModelState.IsValid)
            return showPopupError(model, $"{_service.DtoName} IS NULL OR ModelState invalid!");

        var createdEntry = await _service.AddAsync(model.Entry);
        if ((createdEntry?.Id ?? 0) <= 0)
            return showPopupError(model, $"New {_service.DtoName} not added due to error!");

        return showPopupError(model);*/

        return await ServeOperationAsync(model, null, true, VMBase.CRUD.CREATE);
    }

    
    [HttpGet]
    public async Task<IActionResult> CreateAsync()
    {
        return await ServeGetAsync(BuildVMAsync, false, VMBase.CRUD.CREATE);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(AccountCategoryVM model)
    {
        return await ServeOperationAsync(model, null, false, VMBase.CRUD.CREATE);
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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditQuick(AccountCategoryVM model)
    {
        // DO NOT use model.EntryId here, its 0 in just posted model.
        return await ServeOperationAsync(model, null, true, VMBase.CRUD.UPDATE, model.Entry.Id);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        return await ServeGetAsync(BuildVMAsync, false, VMBase.CRUD.UPDATE, id);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(AccountCategoryVM model)
    {
        // DO NOT use model.EntryId here, its 0 in just posted model.
        return await ServeOperationAsync(model, null, false, VMBase.CRUD.UPDATE, model.Entry.Id);
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
    public async Task<IActionResult> DeleteQuick(AccountCategoryVM model)
    {
        // DO NOT use model.EntryId here, its 0 in just posted model.
        return await ServeOperationAsync(model, null, true, VMBase.CRUD.DELETE, model.Entry.Id);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        return await ServeGetAsync(BuildVMAsync, false, VMBase.CRUD.DELETE, id);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(AccountCategoryVM model)
    {
        // DO NOT use model.EntryId here, its 0 in just posted model.
        return await ServeOperationAsync(model, null, false, VMBase.CRUD.DELETE, model.Entry.Id);
    }




    /*private IActionResult showPopupError(AccountCategoryVM model, string? error = null)
    {
        //if (error != null)
        //    await LoadDropdownsAsync(model);

        //PopupError = error;
        model.Error = error;

        //return error == null ? Page() : Partial("_Create", this);
        return error != null
            //? Partial(PARTIAL_NAME, this)
            ? PartialView(model.PARTIAL_NAME, model)
            : model.SuccessJson(model.Entry);
        //: new JsonResult(new
        //  {
        //      success = true,
        //      toastType = "success",  // CREATE: success, UPDATE: info, DELETE: danger, WARNING: warning
        //      icon = "check-circle",  // CREATE; check-circle, UPDATE: pencil, DELETE: trash, WARNING: exclamation-triangle, INFO: info-circle
        //      message = $"{_service.AccountsService.DtoName} '{model.Entry.Name}' created successfully with Id #{model.Entry.Id}!"
        //  });
    }*/


    private Task<AccountCategoryVM> BuildVMAsync(VMBase.CRUD crud, bool isModal, AccountCategory? entry)
        => Task.FromResult(BuildVM(crud, isModal, entry));

    private AccountCategoryVM BuildVM(VMBase.CRUD crud, bool isModal, AccountCategory? entry = null)
    {
        return new AccountCategoryVM(crud, _service, isModal, Path,
            entry ?? _service.New()
        );
    }

}
