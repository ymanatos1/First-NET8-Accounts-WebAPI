using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebAPI.Lib.Data.Services;
using WebApp.Lib.Data.ViewModels;
using WebApp.Lib.Data.ViewModels.Base;

namespace WebApp.Razor.Pages.Accounts;

public class AccountsModel : PageModel
{
    public readonly string Path = "Accounts";

    private readonly (
            IAccountServiceClient AccountsService,
            IAccountCategoryServiceClient AccountCategoriesService
        ) _service;
    private readonly ILogger<AccountsModel> _logger;

    [BindProperty]
    public AccountsVM ModelVM { get; set; } = new ();


    public AccountsModel(
        IAccountServiceClient serviceAccount,
        IAccountCategoryServiceClient serviceAccountCategory,
        ILogger<AccountsModel> logger)
    {
        _service = (serviceAccount, serviceAccountCategory);
        _logger = logger;
    }

    public async Task OnGetAsync()
    {
        ModelVM = new AccountsVM(VMBase.CRUD.LIST, _service.AccountsService, false, Path,
            await _service.AccountsService.GetAsync(),
            await _service.AccountCategoriesService.GetAsync());
    }

}
