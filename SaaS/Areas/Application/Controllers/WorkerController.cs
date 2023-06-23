using Microsoft.AspNetCore.Mvc;
using SaaS.DataAccess.Repository.IRepository;
using SaaS.Domain.OTHER;

namespace SaaS.Areas.Application.Controllers
{
    [Area("Application")]
    public class WorkerController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public WorkerController(IUnitOfWork unitOfWork)
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
            User worker = new User();
            return View(worker);
        }

        [HttpPost]
        public IActionResult Create(User worker)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    this.unitOfWork.User.Add(worker);
                    this.unitOfWork.Save();
                }
                catch (Exception ex)
                {

                }
            }
            return View("index", "worker");
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAllWorkers()
        {
            IEnumerable<User> workers = this.unitOfWork.User.GetAll();
            return Json(new { data = workers });   
        }
        #endregion
    }
}
