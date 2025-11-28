using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/alive")]
    [ApiController]
    //public class AccountsV1Controller : ControllerBase
    public class AliveController : Controller
    {
        [HttpGet]
        public bool Get() { return true; }

    }
}
