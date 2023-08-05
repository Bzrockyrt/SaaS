using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using SaaS.DataAccess.Exceptions.Application.WorkSite;
using SaaS.DataAccess.Repository.IRepository;
using SaaS.DataAccess.Services;
using SaaS.DataAccess.Utils;
using SaaS.Domain;
using SaaS.Domain.PIPL;
using SaaS.Domain.Work;
using SaaS.ViewModels.Application.Worksite;

namespace SaaS.Areas.Application.Controllers
{
    [Area("Application")]
    public class WorkSiteController : Controller
    {
        private readonly IApplicationUnitOfWork applicationUnitOfWork;
        private readonly TenantService tenantService;
        private readonly TenantSettings tenantSettings;

        public static List<IndexWorkSiteViewModel> WorkSites = new List<IndexWorkSiteViewModel>();

        public WorkSiteController(IApplicationUnitOfWork applicationUnitOfWork,
            TenantService tenantService,
            IOptions<TenantSettings> options)
        {
            this.applicationUnitOfWork = applicationUnitOfWork;
            this.tenantService = tenantService;
            this.tenantSettings = options.Value;

        }

        public IActionResult Index()
        {
            if (this.applicationUnitOfWork.User.CanUserAccessFunctionnality("F-WORKSITES", User))
            {
                return View();
            }
            else
            {
                TempData["error-title"] = "Accès chantiers";
                TempData["error-message"] = "Vous n'êtes pas autorisé à accéder aux chantiers";
                return View();
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            WorkSite workSite = new WorkSite();
            return View(workSite);
        }

        [HttpPost]
        public IActionResult Create(WorkSite workSite)
        {
            if (ModelState.IsValid)
            {
                if (User?.Identity?.Name is null)
                    workSite.CreatorId = string.Empty;
                else
                    workSite.CreatorId = this.applicationUnitOfWork.User.GetAll().FirstOrDefault(u => u.UserName == User?.Identity?.Name).Id;
                try
                {
                    IEnumerable<WorkSite> workSites = this.applicationUnitOfWork.WorkSite.GetAll();
                    foreach (WorkSite wSite in workSites)
                    {
                        if (wSite.Name == workSite.Name)
                        {
                            throw new WorkSiteNameAlreadyExistsException();
                        }
                    }

                    this.applicationUnitOfWork.WorkSite.Add(workSite);
                    this.applicationUnitOfWork.Save();
                    this.applicationUnitOfWork.Log.CreateNewEventInlog(null, User, $"Le chantier a bien été ajoutée à la base de données", "", LogType.Success);
                    TempData["success-title"] = "Création chantier";
                    TempData["success-message"] = "Le chantier a bien été créée";
                    return RedirectToAction("Index");
                }
                catch (WorkSiteNameAlreadyExistsException ex)
                {
                    this.applicationUnitOfWork.Log.CreateNewEventInlog(ex, User, "Erreur lors de l'ajout d'une chantier dans la base de données", "WorkSiteNameAlreadyExistsException", LogType.Warning);
                    TempData["warning-title"] = "Création chantier";
                    TempData["warning-message"] = "Le nom de ce chantier existe déjà";
                    return View(workSite);
                }
                catch (Exception ex)
                {
                    this.applicationUnitOfWork.Log.CreateNewEventInlog(ex, User, "Erreur lors de l'ajout d'un chantier dans la base de données", "Exception", LogType.Error);
                    TempData["warning-title"] = "Création chantier";
                    TempData["warning-message"] = "Erreur lors de la création d'un chantier";
                    return View(workSite);
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult Edit(string? id)
        {
            if (id == null || string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            WorkSite worksite = this.applicationUnitOfWork.WorkSite.Get(c => c.Id == id);

            if (worksite is null)
            {
                return NotFound();
            }
            return View(worksite);
        }

        [HttpPost]
        public IActionResult Edit(WorkSite worksite)
        {
            if (ModelState.IsValid)
            {
                this.applicationUnitOfWork.WorkSite.Update(worksite);
                this.applicationUnitOfWork.Save();

                this.applicationUnitOfWork.Log.CreateNewEventInlog(null, User, $"Le chantier a bien été modifié", "", LogType.Success);
                TempData["success-title"] = "Modification chantier";
                TempData["success-message"] = $"Le chantier a bien été modifié";
                return RedirectToAction("Index");
            }
            return View(worksite);
        }

        [HttpGet]
        public IActionResult Details(string? id)
        {
            if (id == null || string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            //Modifier l'entreprise
            WorkSite worksite = this.applicationUnitOfWork.WorkSite.Get(c => c.Id == id);

            if (worksite is null)
            {
                return NotFound();
            }
            return View(worksite);
        }

        #region APICALLS
        [HttpGet]
        public IActionResult GetAllWorkSites(string status)
        {
            WorkSites.Clear();
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
                                var query = string.Empty;

                                switch (status)
                                {
                                    case "unClotured":
                                        query = "SELECT WorkSite.Id, WorkSite.[Name], WorkSite.Code, WorkSite.[Description], Subsidiary.[Name], WorkSite.IsEnable FROM WorkSite LEFT JOIN AspNetUsers ON WorkSite.CreatorId = AspNetUsers.Id LEFT JOIN Job ON AspNetUsers.JobId = Job.Id LEFT JOIN Department ON Department.Id = Job.DepartmentId LEFT JOIN Subsidiary ON Subsidiary.Id = Department.SubsidiaryId WHERE WorkSite.EndDate IS NULL ORDER BY WorkSite.[Name];";
                                        /*workSites = this.applicationUnitOfWork.WorkSite.GetAll().Where(ws => ws.EndDate is null);*/
                                        break;
                                    case "clotured":
                                        query = "SELECT WorkSite.Id, WorkSite.[Name], WorkSite.Code, WorkSite.[Description], Subsidiary.[Name], WorkSite.IsEnable FROM WorkSite LEFT JOIN AspNetUsers ON WorkSite.CreatorId = AspNetUsers.Id LEFT JOIN Job ON AspNetUsers.JobId = Job.Id LEFT JOIN Department ON Department.Id = Job.DepartmentId LEFT JOIN Subsidiary ON Subsidiary.Id = Department.SubsidiaryId WHERE WorkSite.EndDate IS NOT NULL ORDER BY WorkSite.[Name];";
                                        /*workSites = this.applicationUnitOfWork.WorkSite.GetAll().Where(ws => ws.EndDate is not null);*/
                                        break;
                                    default:
                                        query = "SELECT WorkSite.Id, WorkSite.[Name], WorkSite.Code, WorkSite.[Description], Subsidiary.[Name], WorkSite.IsEnable FROM WorkSite LEFT JOIN AspNetUsers ON WorkSite.CreatorId = AspNetUsers.Id LEFT JOIN Job ON AspNetUsers.JobId = Job.Id LEFT JOIN Department ON Department.Id = Job.DepartmentId LEFT JOIN Subsidiary ON Subsidiary.Id = Department.SubsidiaryId ORDER BY WorkSite.[Name];";
                                        /*workSites = this.applicationUnitOfWork.WorkSite.GetAll();*/
                                        break;
                                }

                                SqlCommand command = new SqlCommand(query, connection);
                                SqlDataReader reader = command.ExecuteReader();
                                while (reader.Read())
                                {
                                    WorkSites.Add(new IndexWorkSiteViewModel
                                    {
                                        Id = reader.IsDBNull(0) ? "" : reader.GetString(0),
                                        Name = reader.IsDBNull(1) ? "" : reader.GetString(1),
                                        Code = reader.IsDBNull(2) ? "" : reader.GetString(2),
                                        Description = reader.IsDBNull(3) ? "" : reader.GetString(3),
                                        SubsidiaryName = reader.IsDBNull(4) ? "" : reader.GetString(4),
                                        IsEnable = reader.IsDBNull(5) ? false : reader.GetBoolean(5),
                                    });
                                }
                                reader.Close();
                                connection.Close();
                                return Json(new { data = WorkSites });
                            }
                            catch (Exception ex)
                            {
                                this.applicationUnitOfWork.Log.CreateNewEventInlog(ex, User, "Une erreur est survenue lors de la récupération des chantiers", "Exception", LogType.Error);
                                TempData["error-title"] = "Récupération chantiers";
                                TempData["error-message"] = "Une erreur est survenue lors de la récupération des chantiers";
                                return Json(null);
                            }
                        }
                    }
                }
            }

            return Json(null);
        }

        [HttpPost]
        public IActionResult LockUnlockWorkSite([FromBody] string id)
        {
            var objFromDb = new WorkSite();
            try
            {
                objFromDb = this.applicationUnitOfWork.WorkSite.Get(f => f.Id == id);
                if (objFromDb is null)
                    return Json(new { success = false, message = "Une erreur est survenue lors de l'activation/la désactivation du chantier" });
                if (objFromDb.IsEnable == false)
                    objFromDb.IsEnable = true;
                else
                    objFromDb.IsEnable = false;

                this.applicationUnitOfWork.WorkSite.Update(objFromDb);
                this.applicationUnitOfWork.Save();
                if (objFromDb.IsEnable == true)
                {
                    this.applicationUnitOfWork.Log.CreateNewEventInlog(null, User, $"Le chantier {objFromDb.Name} a bien été activé", "", LogType.Success);
                    TempData["success-title"] = "Activation chantier";
                    TempData["success-message"] = $"Le chantier {objFromDb.Name} a bien été activé";
                }
                else if (objFromDb.IsEnable == false)
                {
                    this.applicationUnitOfWork.Log.CreateNewEventInlog(null, User, $"Le chantier {objFromDb.Name} a bien été desactivé", "", LogType.Success);
                    TempData["success-title"] = "Désactivation chantier";
                    TempData["success-message"] = $"Le chantier {objFromDb.Name} a bien été desactivé";
                }
            }
            catch (Exception ex)
            {
                this.applicationUnitOfWork.Log.CreateNewEventInlog(ex, User, $"Erreur lors de la modification de l'état du chantier {objFromDb.Name} dans la base de données", "Exception", LogType.Error);
                TempData["error-title"] = "Modification état chantier";
                TempData["error-message"] = $"Erreur lors de la modification du chantier {objFromDb.Name} dans la base de données";
            }

            return Json(new { success = true, message = "Activation/désactivation du chantier réussie" });
        }

        [HttpDelete]
        public IActionResult Delete(string? id)
        {
            if (id is not null)
            {
                try
                {
                    WorkSite workSite = this.applicationUnitOfWork.WorkSite.Get(ws => ws.Id == id);
                    this.applicationUnitOfWork.WorkSite.Delete(workSite);
                    this.applicationUnitOfWork.Save();
                }
                catch (Exception ex)
                {

                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult Close(string? id)
        {
            if (id is not null)
            {
                try
                {
                    WorkSite workSite = this.applicationUnitOfWork.WorkSite.Get(ws => ws.Id == id);
                    workSite.EndDate = DateTime.Now;
                    this.applicationUnitOfWork.WorkSite.Update(workSite);
                    this.applicationUnitOfWork.Save();
                    return View("Index");
                }
                catch (Exception ex)
                {

                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult Open(string? id)
        {
            if (id is not null)
            {
                try
                {
                    WorkSite workSite = this.applicationUnitOfWork.WorkSite.Get(ws => ws.Id == id);
                    workSite.EndDate = null;
                    this.applicationUnitOfWork.WorkSite.Update(workSite);
                    this.applicationUnitOfWork.Save();
                    return View("Index");
                }
                catch (Exception ex)
                {

                }
            }
            return View();
        }
        #endregion
    }
}
