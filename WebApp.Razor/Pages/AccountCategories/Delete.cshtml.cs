using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Reflection;
using WebAPI.Data.Models;
using WebAPI.Lib.Data.Services;
using WebApp.Lib.Data.ViewModels;
using WebApp.Lib.Data.ViewModels.Base;

namespace WebApp.Razor.Pages.AccountCategories;

public class AccountCategoriesDeleteModel : PageModel
{
    public readonly string Path = "AccountCategories";

    private readonly IAccountCategoryServiceClient _service;
    private readonly ILogger<AccountCategoriesDeleteModel> _logger;

    [BindProperty]
    public AccountCategoryVM ModelVM { get; set; } = new();


    public AccountCategoriesDeleteModel(IAccountCategoryServiceClient service, ILogger<AccountCategoriesDeleteModel> logger)
    {
        _service = service;
        _logger = logger;
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var entry = await _service.GetByIdAsync(id);

        if (entry != null)
        {
            ModelVM = new(VMBase.CRUD.DELETE, _service, false, Path,
                entry);

            return Partial(ModelVM.PARTIAL_NAME, ModelVM);
        }

        //return RedirectToAction("Index");
        return RedirectToPage("./Index");
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        ModelVM.UpdateBase(VMBase.CRUD.DELETE, _service, false, Path);
        ModelVM.Entry.Id = id;

        if (ModelVM.Entry == null || ModelVM.Entry.Id <= 0)
            return showPopupError(ModelVM, $"{_service.DtoName} IS NULL OR ModelState invalid!");

        var success = await _service.RemoveByIdAsync(ModelVM.Entry.Id);
        if (!success)
            return showPopupError(ModelVM, $"New {_service.DtoName} not deleted due to error!");

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
