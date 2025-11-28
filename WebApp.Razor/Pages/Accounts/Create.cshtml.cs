using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Reflection;
using WebAPI.Data.Models;
using WebAPI.Lib.Data.Services;
using WebApp.Lib.Data.ViewModels;
using WebApp.Lib.Data.ViewModels.Base;

namespace WebApp.Razor.Pages.Accounts;

public class AccountCreateModel : PageModel
{
    public readonly string Path = "Accounts";

    private readonly (
            IAccountServiceClient AccountsService,
            IAccountCategoryServiceClient AccountCategoriesService
        ) _service;
    private readonly ILogger<AccountCreateModel> _logger;

    [BindProperty]
    public AccountVM ModelVM { get; set; } = new();


    public AccountCreateModel(
        IAccountServiceClient serviceAccount,
        IAccountCategoryServiceClient serviceAccountCategory,
        ILogger<AccountCreateModel> logger)
    {
        _service = (serviceAccount, serviceAccountCategory);
        _logger = logger;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        ModelVM = new(VMBase.CRUD.CREATE, _service.AccountsService, false, Path,
            _service.AccountsService.New(),
            await _service.AccountCategoriesService.GetAsync());

        //return Partial(ModelVM.PARTIAL_NAME, this);
        return Partial(ModelVM.PARTIAL_NAME, ModelVM);
    }

    public async Task<IActionResult> OnPostAsync()
    {
        ModelVM.UpdateBase(VMBase.CRUD.CREATE, _service.AccountsService, false, Path);

        if (ModelVM.Entry == null || !ModelState.IsValid)
            return await showPopupErrorAsync(ModelVM, $"{_service.AccountsService.DtoName} IS NULL OR ModelState invalid!");

        var createdEntry = await _service.AccountsService.AddAsync(ModelVM.Entry);
        if ((createdEntry?.Id ?? 0) <= 0)
            return await showPopupErrorAsync(ModelVM, $"New {_service.AccountsService.DtoName} not added due to error!");

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
            //? Partial(model.PARTIAL_NAME, this)
            : model.SuccessJson(model.Entry);
    }

}
