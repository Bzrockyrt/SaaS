using Microsoft.AspNetCore.Mvc;

namespace SaaS.Areas.Application.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
