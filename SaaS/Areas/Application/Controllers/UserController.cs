using Microsoft.AspNetCore.Mvc;

namespace SaaS.Areas.Application.Controllers
{
    [Area("Application")]
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
