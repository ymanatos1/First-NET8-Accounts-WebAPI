using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Reflection;
using WebAPI.Data.Models;
using WebAPI.Lib.Data.Services;
using WebApp.Lib.Data.ViewModels;
using WebApp.Lib.Data.ViewModels.Base;

namespace WebApp.Razor.Pages.Accounts;

public class AccountEditModel : PageModel
{
    public readonly string Path = "Accounts";

    private readonly (
            IAccountServiceClient AccountsService,
            IAccountCategoryServiceClient AccountCategoriesService
        ) _service;
    private readonly ILogger<AccountEditModel> _logger;

    [BindProperty]
    public AccountVM ModelVM { get; set; } = new();


    public AccountEditModel(
        IAccountServiceClient serviceAccount,
        IAccountCategoryServiceClient serviceAccountCategory,
        ILogger<AccountEditModel> logger)
    {
        _service = (serviceAccount, serviceAccountCategory);
        _logger = logger;
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var entry = await _service.AccountsService.GetByIdAsync(id);

        if (entry != null)
        {
            ModelVM = new(VMBase.CRUD.UPDATE, _service.AccountsService, false, Path,
                entry,
                await _service.AccountCategoriesService.GetAsync());

            return Partial(ModelVM.PARTIAL_NAME, ModelVM);
        }

        //return RedirectToAction("Index");
        return RedirectToPage("./Index");
    }

    public async Task<IActionResult> OnPostAsync()
    {
        ModelVM.UpdateBase(VMBase.CRUD.UPDATE, _service.AccountsService, false, Path);

        if (ModelVM.Entry == null || !ModelState.IsValid)
            return await showPopupErrorAsync(ModelVM, $"{_service.AccountsService.DtoName} IS NULL OR ModelState invalid!");

        var success = await _service.AccountsService.UpdateAsync(ModelVM.Entry.Id, ModelVM.Entry);
        if (!success)
            return await showPopupErrorAsync(ModelVM, $"New {_service.AccountsService.DtoName} not updated due to error!");

        return await showPopupErrorAsync(ModelVM);
    }


    private async Task LoadDropdownsAsync(AccountVM model)
    {
        model.CategoryList = await _service.AccountCategoriesService.GetAsync();
    }

    private async Task<IActionResult> showPopupErrorAsync(AccountVM model, string? error = null)
    {
        if (error != null)
            await LoadDropdownsAsync(model);

        model.Error = error;

        return error != null
            //? Partial(PARTIAL_NAME, this)
            ? Partial(model.PARTIAL_NAME, model)
            : model.SuccessJson(model.Entry);
    }

}
