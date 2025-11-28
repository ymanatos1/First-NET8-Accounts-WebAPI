using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebAPI.Data.Models;

namespace WebApp.Razor.UI.Lib.ViewComponents;

public class AccountListTableViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(IEnumerable<Account>? accounts)
    {
        // Ensure non-null list to avoid null-reference errors
        accounts ??= new List<Account>();

        // Pass accounts to the view component view
        return View(accounts);
    }

}
