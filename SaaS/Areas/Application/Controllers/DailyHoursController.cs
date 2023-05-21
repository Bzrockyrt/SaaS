using Microsoft.AspNetCore.Mvc;

namespace SaaS.Areas.Application.Controllers
{
    [Area("Application")]
    public class DailyHoursController : Controller
    {
        public IActionResult DailyHours()
        {
            return View();
        }

        public IActionResult ValidHours()
        {
            var v = false;
            return View("Index");
        }
    }
}
