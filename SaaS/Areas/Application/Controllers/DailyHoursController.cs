using Microsoft.AspNetCore.Mvc;
using SaaS.DataAccess.Repository.IRepository;
using SaaS.Domain.Work;

namespace SaaS.Areas.Application.Controllers
{
    [Area("Application")]
    public class DailyHoursController : Controller
    {
        private readonly IApplicationUnitOfWork applicationUnitOfWork;

        public DailyHoursController(IApplicationUnitOfWork applicationUnitOfWork)
        {
            this.applicationUnitOfWork = applicationUnitOfWork;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            WorkHour workHour = new WorkHour();
            return View(workHour);
        }

        [HttpPost]
        public IActionResult Create(WorkHour workHour)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (User.Identity.Name is null)
                    {
                        /*Message d'erreur comme quoi le nom de l'utilisateur est null*/
                    }
                    else
                    {
                        string userId = this.applicationUnitOfWork.User.Get(u => u.Fullname == User.Identity.Name).Id;
                        /*workHour.UserId = userId;*/
                        this.applicationUnitOfWork.WorkHour.Add(workHour);
                        this.applicationUnitOfWork.Save();
                    }
                }
                catch (Exception ex)
                {

                }
            }
            var v = workHour;
            return View("dailyhours", "dailyhours");
        }

        [HttpGet]
        public IActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Edit(string? id)
        {
            return View();
        }
    }
}
