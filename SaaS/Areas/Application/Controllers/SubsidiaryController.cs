using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using SaaS.DataAccess.Exceptions.Application.Department;
using SaaS.DataAccess.Exceptions.Application.Subsidiary;
using SaaS.DataAccess.Repository.IRepository;
using SaaS.DataAccess.Services;
using SaaS.DataAccess.Utils;
using SaaS.Domain;
using SaaS.Domain.Identity;
using SaaS.Domain.Work;
using SaaS.ViewModels.Application.Department;
using SaaS.ViewModels.Application.Subsidiary;

namespace SaaS.Areas.Application.Controllers
{
    [Area("Application")]
    public class SubsidiaryController : Controller
    {
        private readonly IApplicationUnitOfWork applicationUnitOfWork;
        private readonly TenantService tenantService;
        private readonly TenantSettings tenantSettings;

        public static List<SubsidiaryViewModel> Subsidiaries = new List<SubsidiaryViewModel>();

        public SubsidiaryController(IApplicationUnitOfWork applicationUnitOfWork,
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
            Subsidiary subsidiary = new Subsidiary();
            return View(subsidiary);
        }

        [HttpPost]
        public IActionResult Create(Subsidiary subsidiary)
        {
            if (ModelState.IsValid)
            {
                if (User?.Identity?.Name is null)
                    subsidiary.CreatorId = string.Empty;
                else
                    subsidiary.CreatorId = this.applicationUnitOfWork.User.GetAll().FirstOrDefault(u => u.UserName == User?.Identity?.Name).Id;

                try
                {
                    IEnumerable<Subsidiary> subsidiaries = this.applicationUnitOfWork.Subsidiary.GetAll();
                    foreach (Subsidiary sub in subsidiaries)
                    {
                        if (sub.Name == subsidiary.Name)
                        {
                            throw new SubsidiaryNameAlreadyExistsException();
                        }
                        if (sub.Code == subsidiary.Code)
                        {
                            throw new SubsidiaryCodeAlreadyExistsException();
                        }
                    }

                    this.applicationUnitOfWork.Subsidiary.Add(subsidiary);
                    this.applicationUnitOfWork.Save();
                    this.applicationUnitOfWork.Log.CreateNewEventInlog(null, User, $"La filliale a bien été ajoutée à la base de données", "", LogType.Success);
                    TempData["success-title"] = "Création filliale";
                    TempData["success-message"] = "La filliale a bien été créée";
                    return RedirectToAction("Index");
                }
                catch (SubsidiaryNameAlreadyExistsException ex)
                {
                    TempData["warning-title"] = "Création filliale";
                    TempData["warning-message"] = "Le nom de cette filliale existe déjà";
                    return View(subsidiary);
                }
                catch (SubsidiaryCodeAlreadyExistsException ex)
                {
                    TempData["warning-title"] = "Création filliale";
                    TempData["warning-message"] = "Le code de cette filliale existe déjà";
                    return View(subsidiary);
                }
                catch (Exception ex)
                {
                    this.applicationUnitOfWork.Log.CreateNewEventInlog(ex, User, "Erreur lors de l'ajout d'une filliale dans la base de données", "Exception", LogType.Error);
                    TempData["warning-title"] = "Création filliale";
                    TempData["warning-message"] = "Erreur lors de la création d'une filliale";
                    return View(subsidiary);
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult Details(string? id)
        {
            if (id == null || string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            Subsidiary subsidiary = this.applicationUnitOfWork.Subsidiary.Get(s => s.Id == id);
            if (subsidiary is null)
            {
                return NotFound();
            }
            return View(subsidiary);
        }

        [HttpPost]
        public IActionResult Update(Subsidiary subsidiary)
        {
            if (ModelState.IsValid)
            {
                subsidiary.UpdatedOn = DateTime.Now;
                subsidiary.UpdatedBy = User?.Identity.Name;
                this.applicationUnitOfWork.Subsidiary.Update(subsidiary);
                this.applicationUnitOfWork.Save();

                this.applicationUnitOfWork.Log.CreateNewEventInlog(null, User, $"La filliale a bien été modifiée", "", LogType.Success);
                TempData["success-title"] = "Modification filliale";
                TempData["success-message"] = $"La filliale a bien été modifiée";
                return RedirectToAction("Index");
            }
            return View(subsidiary);
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAllSubsidiaries()
        {
            Subsidiaries.Clear();
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
                                var query = "SELECT Subsidiary.Id, Subsidiary.Code, Subsidiary.IsEnable, Subsidiary.[Name], COUNT(DISTINCT Department.Id) AS DepartmentsCount, COUNT(DISTINCT Job.Id) AS JobsCount, COUNT(DISTINCT AspNetUsers.Id) AS UsersCount FROM Subsidiary LEFT JOIN Department ON Subsidiary.Id = Department.SubsidiaryId LEFT JOIN Job ON Department.Id = Job.DepartmentId LEFT JOIN AspNetUsers ON Job.Id = AspNetUsers.JobId GROUP BY Subsidiary.Id, Subsidiary.Code, Subsidiary.IsEnable, Subsidiary.[Name];";
                                SqlCommand cmd = new SqlCommand(query, connection);
                                SqlDataReader reader = cmd.ExecuteReader();
                                while (reader.Read())
                                {
                                    Subsidiaries.Add(new SubsidiaryViewModel
                                    {
                                        Id = reader.IsDBNull(0) ? "" : reader.GetString(0),
                                        Code = reader.IsDBNull(0) ? "" : reader.GetString(1),
                                        EmployeesNumber = reader.IsDBNull(0) ? 0 : reader.GetInt32(6),
                                        DepartmentsNumber = reader.IsDBNull(0) ? 0 : reader.GetInt32(4),
                                        IsEnable = reader.GetBoolean(2),
                                        JobsNumber = reader.IsDBNull(0) ? 0 : reader.GetInt32(5),
                                        Name = reader.IsDBNull(0) ? "" : reader.GetString(3),
                                    });
                                }
                                reader.Close();
                                connection.Close();
                                return Json(new { data = Subsidiaries });
                            }
                            catch (Exception ex)
                            {
                                this.applicationUnitOfWork.Log.CreateNewEventInlog(ex, User, "Une erreur est survenue lors de la récupération des filliales", "Exception", LogType.Error);
                                TempData["error-title"] = "Récupération filliales";
                                TempData["error-message"] = "Une erreur est survenue lors de la récupération des filliales";
                                return Json(null);
                            }
                        }
                    }
                }
                return Json(null);
            }
            return Json(null);
        }

        [HttpPost]
        public IActionResult LockUnlockSubsidiary([FromBody] string id)
        {
            var objFromDb = new Subsidiary();
            try
            {
                objFromDb = this.applicationUnitOfWork.Subsidiary.Get(u => u.Id == id);
                if (objFromDb is null)
                    return Json(new { success = false, message = "Une erreur est survenue lors de l'activation/la désactivation de la filliale" });
                if (objFromDb.IsEnable == false)
                    objFromDb.IsEnable = true;
                else
                    objFromDb.IsEnable = false;

                this.applicationUnitOfWork.Subsidiary.Update(objFromDb);
                this.applicationUnitOfWork.Save();
                if (objFromDb.IsEnable == true)
                {
                    this.applicationUnitOfWork.Log.CreateNewEventInlog(null, User, $"La filliale {objFromDb.Name} a bien été activée", "", LogType.Success);
                    TempData["success-title"] = "Activation filliale";
                    TempData["success-message"] = $"La filliale {objFromDb.Name} a bien été activé";
                }
                else if (objFromDb.IsEnable == false)
                {
                    this.applicationUnitOfWork.Log.CreateNewEventInlog(null, User, $"La filliale {objFromDb.Name} a bien été desactivée", "", LogType.Success);
                    TempData["success-title"] = "Désactivation filliale";
                    TempData["success-message"] = $"La filliale {objFromDb.Name} a bien été desactivé";
                }
            }
            catch (Exception ex)
            {
                this.applicationUnitOfWork.Log.CreateNewEventInlog(ex, User, $"Erreur lors de la modification de l'état de la filliale {objFromDb.Name} dans la base de données", "Exception", LogType.Error);
                TempData["error-title"] = "Modification état filliale";
                TempData["error-message"] = $"Erreur lors de la modification de l'état de la filliale {objFromDb.Name} dans la base de données";
            }

            return Json(new { success = true, message = "Activation/désactivation de la filliale réussie" });
        }
        #endregion
    }
}
