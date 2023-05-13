using Microsoft.AspNetCore.Mvc;

namespace SaaS.Areas.SuperAdministration.Controllers
{
    [Area("SuperAdministration")]
    public class CompanyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
