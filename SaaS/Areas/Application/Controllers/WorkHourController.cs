using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using SaaS.DataAccess.Exceptions.Application.WorkHour;
using SaaS.DataAccess.Repository.IRepository;
using SaaS.DataAccess.Services;
using SaaS.DataAccess.Utils;
using SaaS.Domain;
using SaaS.Domain.Work;
using SaaS.ViewModels.Application.WorkHour;

namespace SaaS.Areas.Application.Controllers
{
    [Area("Application")]
    public class WorkHourController : Controller
    {
        private readonly IApplicationUnitOfWork applicationUnitOfWork;
        private readonly TenantService tenantService;
        private readonly TenantSettings tenantSettings;

        public static List<IndexWorkHourViewModel> WorkHours = new List<IndexWorkHourViewModel>();

        public WorkHourController(IApplicationUnitOfWork applicationUnitOfWork,
            TenantService tenantService,
            IOptions<TenantSettings> options)
        {
            this.applicationUnitOfWork = applicationUnitOfWork;
            this.tenantService = tenantService;
            this.tenantSettings = options.Value;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            CreateWorkHourViewModel createWorkHourViewModel = new CreateWorkHourViewModel()
            {
                WorkManagersList = this.applicationUnitOfWork.User.GetAll().Where(u => u.IsAppearingInWorkerWorkHours).Select(d => new SelectListItem
                {
                    Text = d.Fullname,
                    Value = d.Id
                }),
                WorkSitesList = this.applicationUnitOfWork.WorkSite.GetAll().Where(ws => ws.IsEnable && ws.EndDate is null).Select(d => new SelectListItem
                {
                    Text = d.Name, 
                    Value = d.Id
                }),
                WorkHour = new WorkHour()
            };
            return View(createWorkHourViewModel);
        }

        [HttpPost]
        public IActionResult Create(CreateWorkHourViewModel createWorkHourViewModel)
        {
            if (ModelState.IsValid)
            {
                if (User?.Identity?.Name is null)
                    createWorkHourViewModel.WorkHour.CreatorId = string.Empty;
                else
                    createWorkHourViewModel.WorkHour.CreatorId = this.applicationUnitOfWork.User.Get(u => u.UserName == User.Identity.Name).Id;
                try
                {
                    string userId = this.applicationUnitOfWork.User.Get(u => u.UserName == User.Identity.Name).Id;

                    IEnumerable<WorkHour> workHours = this.applicationUnitOfWork.WorkHour.GetAll();

                    foreach (WorkHour wh in workHours)
                    {
                        if (wh.WorkDay == createWorkHourViewModel.WorkHour.WorkDay && wh.UserId == userId)
                        {
                            throw new WorkDayHasAlreadyBeenRegisteredException();
                        }
                    }
                    createWorkHourViewModel.WorkHour.UserId = userId;
                    this.applicationUnitOfWork.WorkHour.Add(createWorkHourViewModel.WorkHour);
                    this.applicationUnitOfWork.Save();
                    this.applicationUnitOfWork.Log.CreateNewEventInlog(null, User, $"{User?.Identity?.Name} a enregistré ses heures de travail", "", LogType.Success);
                    TempData["success-title"] = "Enregistrement heures";
                    TempData["success-message"] = "Vos heures ont bien été enregistrées";
                    return RedirectToAction("Index");
                }
                catch (WorkDayHasAlreadyBeenRegisteredException ex)
                {
                    TempData["warning-title"] = "Renseignement heures salarié";
                    TempData["warning-message"] = "Vous avez déjà renseigné des heures pour ce jour";
                    return View(createWorkHourViewModel);
                }
                catch (Exception ex)
                {
                    this.applicationUnitOfWork.Log.CreateNewEventInlog(ex, User, $"Erreur lors de l'enregistrement des heures de {User?.Identity?.Name}", "Exception", LogType.Error);
                    TempData["warning-title"] = "Enregistrement heures";
                    TempData["warning-message"] = "Erreur lors de l'enregistrement de vos heures de travail";
                    return View(createWorkHourViewModel);
                }
            }
            return View();
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

        #region API CALLS
        [HttpGet]
        public IActionResult GetWorkHours()
        {
            /*WorkHours.Clear();
            if (this.tenantSettings.Companies is not null)
            {
                foreach (TenantData tenantData in this.tenantSettings.Companies.Values)
                {
                    if (tenantData.name == this.tenantService.GetTenantName())
                    {
                        using (SqlConnection connection = new SqlConnection(tenantData.connectionString))
                        {
                            try
                            {
                                connection.Open();
                                var query = "SELECT WorkHour.Id, WorkHour.WorkDay, WorkHour.Work"
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }
                }
            }*/

            string userId = this.applicationUnitOfWork.User.Get(u => u.UserName == User.Identity.Name).Id;

            foreach (WorkHour wh in this.applicationUnitOfWork.WorkHour.GetAll().Where(wh => wh.UserId == userId && wh.IsEnable))
            {
                WorkHours.Add(new IndexWorkHourViewModel
                {
                    WorkDay = wh.WorkDay,
                    MorningStart = wh.MorningStart,
                    MorningEnd = wh.MorningEnd,
                    TotalMorningHours = wh.TotalMorningHours,
                    EveningStart = wh.EveningStart,
                    EveningEnd = wh.EveningEnd,
                    TotalEveningHours = wh.TotalEveningHours,
                });
            }
            return Json(new { data = WorkHours });
        }

        [HttpPost]
        public IActionResult AddWorkSite(List<string> workSiteList)
        {
            return RedirectToAction("Create");
        }
        #endregion
    }
}
