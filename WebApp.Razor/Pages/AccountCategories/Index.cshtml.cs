using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Reflection;
using WebAPI.Lib.Data.Services;
using WebApp.Lib.Data.ViewModels;
using WebApp.Lib.Data.ViewModels.Base;

namespace WebApp.Razor.Pages.AccountCategories;

public class AccountCategoriesModel : PageModel
{
    public readonly string Path = "AccountCategories";

    private readonly IAccountCategoryServiceClient _service;
    private readonly ILogger<AccountCategoriesModel> _logger;

    [BindProperty]
    public AccountCategoriesVM ModelVM { get; set; } = new();


    public AccountCategoriesModel(IAccountCategoryServiceClient service, ILogger<AccountCategoriesModel> logger)
    {
        _service = service;
        _logger = logger;
    }

    public async Task OnGetAsync()
    {
        ModelVM = new(VMBase.CRUD.LIST, _service, false, Path,
            await _service.GetAsync());
    }

}
