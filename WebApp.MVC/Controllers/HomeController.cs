using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebAPI.Lib.Data.Services;
using WebApp.MVC.Models;

namespace WebApp.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAccountServiceClient _service;
        private readonly ILogger<HomeController> _logger;

        //public IEnumerable<Account> AccountsList { get; set; } = new List<Account>();

        public HomeController(IAccountServiceClient service, ILogger<HomeController> logger)
        {
            _service = service;
            _logger = logger;
        }

        public async Task<IActionResult> IndexAsync()
        {
            //ViewData["Accounts"] = await _service.GetAsync();
            //this.AccountsList = await _service.GetAsync();
            var accountsList = await _service.GetAsync();
            //var vm = new AccountsPageViewModel
            //{
            //    Accounts = accounts
            //};

            return View(accountsList);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
