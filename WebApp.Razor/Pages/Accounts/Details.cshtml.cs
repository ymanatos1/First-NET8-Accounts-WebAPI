using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebAPI.Data.Models;
using WebAPI.Lib.Data.Services;
using WebApp.Lib.Data.ViewModels;
using WebApp.Lib.Data.ViewModels.Base;

namespace WebApp.Razor.Pages.Accounts;

public class AccountDetailsModel : PageModel
{
    public readonly string Path = "Accounts";

    private readonly (
            IAccountServiceClient AccountsService,
            IAccountCategoryServiceClient AccountCategoriesService
        ) _service;
    private readonly ILogger<AccountDetailsModel> _logger;

    [BindProperty]
    public AccountVM ModelVM { get; set; } = new();


    public AccountDetailsModel(
        IAccountServiceClient serviceAccount,
        IAccountCategoryServiceClient serviceAccountCategory,
        ILogger<AccountDetailsModel> logger)
    {
        _service = (serviceAccount, serviceAccountCategory);
        _logger = logger;
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var entry = await _service.AccountsService.GetByIdAsync(id);

        if (entry != null)
        {
            ModelVM = new(VMBase.CRUD.READ, _service.AccountsService, false, Path,
                entry,
                await _service.AccountCategoriesService.GetAsync()
            );

            return Partial(ModelVM.PARTIAL_NAME, ModelVM);
        }

        //return RedirectToAction("Index");
        return RedirectToPage("./Index");
    }

}
