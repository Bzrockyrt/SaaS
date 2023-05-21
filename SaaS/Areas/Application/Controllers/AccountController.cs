using Microsoft.AspNetCore.Mvc;

namespace SaaS.Areas.Application.Controllers
{
    [Area("Application")]
    public class AccountController : Controller
    {
        public IActionResult Manage()
        {
            return View();
        }
    }
}
