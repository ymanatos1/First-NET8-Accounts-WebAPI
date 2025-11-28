using Microsoft.AspNetCore.Mvc;
using WebAPI.Data.Models;

namespace WebApp.Razor.UI.Lib;

public class AccountListViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(List<Account> accounts)
    {
        return View("Default", accounts);
    }
}
