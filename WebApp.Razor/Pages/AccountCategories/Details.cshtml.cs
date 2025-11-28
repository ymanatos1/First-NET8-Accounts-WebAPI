using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebAPI.Data.Models;
using WebAPI.Lib.Data.Services;
using WebApp.Lib.Data.ViewModels;
using WebApp.Lib.Data.ViewModels.Base;

namespace WebApp.Razor.Pages.AccountCategories;

public class AccountCategoriesDetailsModel : PageModel
{
    public readonly string Path = "AccountCategories";

    private readonly IAccountCategoryServiceClient _service;
    private readonly ILogger<AccountCategoriesDetailsModel> _logger;

    [BindProperty]
    public AccountCategoryVM ModelVM { get; set; } = new();


    public AccountCategoriesDetailsModel(IAccountCategoryServiceClient service, ILogger<AccountCategoriesDetailsModel> logger)
    {
        _service = service;
        _logger = logger;
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var entry = await _service.GetByIdAsync(id);

        if (entry != null)
        {
            ModelVM = new(VMBase.CRUD.READ, _service, false, Path,
                entry
            );

            return Partial(ModelVM.PARTIAL_NAME, ModelVM);
        }

        //return RedirectToAction("Index");
        return RedirectToPage("./Index");
    }

}
