using Microsoft.AspNetCore.Mvc;
using SaaS.DataAccess.Repository.IRepository;

namespace SaaS.Areas.Application.Controllers
{
    [Area("Application")]
    public class FunctionnalityController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public FunctionnalityController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            if (this.unitOfWork.User.CanUserAccessFunctionnality("Access_Administration", User))
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
