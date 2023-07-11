using Microsoft.AspNetCore.Mvc;
using SaaS.DataAccess.Repository.IRepository;

namespace SaaS.Areas.Application.Controllers
{
    [Area("Application")]
    public class FunctionnalityController : Controller
    {
        private readonly IApplicationUnitOfWork applicationUnitOfWork;

        public FunctionnalityController(IApplicationUnitOfWork applicationUnitOfWork)
        {
            this.applicationUnitOfWork = applicationUnitOfWork;
        }

        public IActionResult Index()
        {
            if (this.applicationUnitOfWork.User.CanUserAccessFunctionnality("Access_Administration", User))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Connection", new { area = "Application" });
            }
        }
    }
}
