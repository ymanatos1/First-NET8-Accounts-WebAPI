using Microsoft.AspNetCore.Mvc.RazorPages;
using WebAPI.Data.Models;
using WebAPI.Lib.Data.Services;

namespace WebApp.Razor.Pages;

public class IndexModel : PageModel
{
    private readonly IAccountServiceClient _service;
    private readonly ILogger<IndexModel> _logger;

    //public IEnumerable<string> ListItems = [ url ];
    public IEnumerable<Account> AccountsList { get; set; } = Enumerable.Empty<Account>();

    public IndexModel(IAccountServiceClient service, ILogger<IndexModel> logger)
    {
        _service = service;
        _logger = logger;
    }

    public async Task OnGetAsync()
    {
        //ViewData["Accounts"] = await _service.GetAsync();
        this.AccountsList = await _service.GetAsync();
    }
}
