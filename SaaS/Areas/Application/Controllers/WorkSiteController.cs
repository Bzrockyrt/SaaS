using Microsoft.AspNetCore.Mvc;

namespace SaaS.Areas.Application.Controllers
{
    public class WorkSiteController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
