using Microsoft.AspNetCore.Mvc;

namespace SaaS.Areas.Application.Controllers
{
    public class SuperAdminController : Controller
    {
        public IActionResult Panel()
        {
            return View();
        }

        public IActionResult Companies()
        {
            return View();
        }

        public IActionResult Users()
        {
            return View();
        }
    }
}
