using Microsoft.AspNetCore.Mvc;
using SaaS.DataAccess.Repository.PIPL.IRepository;
using SaaS.Domain.PIPL;
using SaaS.ViewModels.SuperCompany.Log;

namespace SaaS.Areas.SuperCompany.Controllers
{
    [Area("SuperCompany")]
    public class LogController : Controller
    {
        private readonly ISuperCompanyUnitOfWork superCompanyUnitOfWork;

        public LogController(ISuperCompanyUnitOfWork superCompanyUnitOfWork)
        {
            this.superCompanyUnitOfWork = superCompanyUnitOfWork;
        }

        public IActionResult Index()
        {
            IndexLogViewModel indexLogViewModel = new IndexLogViewModel();
            indexLogViewModel.Logs = this.superCompanyUnitOfWork.Log.GetAll().ToList();
            return View(indexLogViewModel);
        }

        public IActionResult Detail(string? id)
        {
            return View();
        }

        #region APICALLS
        [HttpGet]
        public IActionResult GetAllSuperCompanyLogs()
        {
            IEnumerable<Log> logs = this.superCompanyUnitOfWork.Log.GetAll().Where(spl => spl.IsEnable == true);

            return Json(new { data = logs });
        }
        #endregion
    }
}
