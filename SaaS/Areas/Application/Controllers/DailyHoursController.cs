using Microsoft.AspNetCore.Mvc;
using SaaS.DataAccess.Repository.IRepository;
using SaaS.Domain.Models;
using SaaS.ViewModels.Application.DailyHours;

namespace SaaS.Areas.Application.Controllers
{
    [Area("Application")]
    public class DailyHoursController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public DailyHoursController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
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
                        string userId = this.unitOfWork.User.Get(u => u.Fullname == User.Identity.Name).Id;
                        /*workHour.UserId = userId;*/
                        this.unitOfWork.WorkHour.Add(workHour);
                        this.unitOfWork.Save();
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
