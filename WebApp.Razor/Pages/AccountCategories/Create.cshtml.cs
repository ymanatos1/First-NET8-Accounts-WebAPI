using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Reflection;
using WebAPI.Data.Models;
using WebAPI.Lib.Data.Services;
using WebApp.Lib.Data.ViewModels;
using WebApp.Lib.Data.ViewModels.Base;

namespace WebApp.Razor.Pages.AccountCategories;

public class AccountCategoriesCreateModel : PageModel
{
    public readonly string Path = "AccountCategories";

    private readonly IAccountCategoryServiceClient _service;
    private readonly ILogger<AccountCategoriesCreateModel> _logger;

    [BindProperty]
    public AccountCategoryVM ModelVM { get; set; } = new();


    public AccountCategoriesCreateModel(IAccountCategoryServiceClient service, ILogger<AccountCategoriesCreateModel> logger)
    {
        _service = service;
        _logger = logger;
    }

    public IActionResult OnGet()
    {
        ModelVM = new AccountCategoryVM(VMBase.CRUD.CREATE, _service, false, Path,
            _service.New());

        return Partial(ModelVM.PARTIAL_NAME, ModelVM);
    }

    public async Task<IActionResult> OnPostAsync()
    {
        ModelVM.UpdateBase(VMBase.CRUD.CREATE, _service, false, Path);

        if (ModelVM.Entry == null || !ModelState.IsValid)
            return showPopupError(ModelVM, $"{_service.DtoName} IS NULL OR ModelState invalid!");

        var createdEntry = await _service.AddAsync(ModelVM.Entry);
        if ((createdEntry?.Id ?? 0) <= 0)
            return showPopupError(ModelVM, $"New {_service.DtoName} not added due to error!");

        return showPopupError(ModelVM);
    }


    private IActionResult showPopupError(AccountCategoryVM model, string? error = null)
    {
        model.Error = error;

        return error != null
            //? Partial(PARTIAL_NAME, this)
            ? Partial(model.PARTIAL_NAME, model)
            : model.SuccessJson(model.Entry);
    }

}
