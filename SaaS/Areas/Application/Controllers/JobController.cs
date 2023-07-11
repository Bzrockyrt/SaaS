using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using SaaS.DataAccess.Exceptions.Application.Department;
using SaaS.DataAccess.Services;
using SaaS.DataAccess.Utils;
using SaaS.Domain;
using SaaS.ViewModels.Application.Department;
using SaaS.DataAccess.Exceptions.Application.Job;
using SaaS.ViewModels.Application.Job;
using Microsoft.AspNetCore.Mvc.Rendering;
using SaaS.Domain.Identity;
using SaaS.DataAccess.Repository.IRepository;

namespace SaaS.Areas.Application.Controllers
{
    [Area("Application")]
    public class JobController : Controller
    {
        private readonly IApplicationUnitOfWork applicationUnitOfWork;
        private readonly TenantService tenantService;
        private readonly TenantSettings tenantSettings;

        public static List<IndexJobViewModel> Jobs = new List<IndexJobViewModel>();

        public JobController(IApplicationUnitOfWork applicationUnitOfWork,
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
            CreateJobViewModel jobViewModel = new CreateJobViewModel()
            {
                DepartmentList = this.applicationUnitOfWork.Department.GetAll().Select(d => new SelectListItem
                {
                    Text = d.Name,
                    Value = d.Id
                }),
                Job = new Job()
            };

            return View(jobViewModel);
        }

        [HttpPost]
        public IActionResult Create(CreateJobViewModel createJobViewModel)
        {
            if (ModelState.IsValid)
            {
                if (User?.Identity?.Name is null)
                    createJobViewModel.Job.CreatedBy = "IPPOLITI Pierre-Louis";
                else
                    createJobViewModel.Job.CreatedBy = User?.Identity?.Name;

                try
                {
                    IEnumerable<Job> jobs = this.applicationUnitOfWork.Job.GetAll();
                    foreach (Job jb in jobs)
                    {
                        if (jb.Name == createJobViewModel.Job.Name)
                        {
                            throw new JobNameAlreadyExistsException();
                        }
                        if (jb.Code == createJobViewModel.Job.Code)
                        {
                            throw new JobCodeAlreadyExistsException();
                        }
                    }

                    this.applicationUnitOfWork.Job.Add(createJobViewModel.Job);
                    this.applicationUnitOfWork.Save();
                    this.applicationUnitOfWork.Log.CreateNewEventInlog(null, User, $"Le poste a bien été ajouté à la base de données", "", LogType.Success);
                    TempData["success-title"] = "Création poste";
                    TempData["success-message"] = "Le poste a bien été créé";
                    return RedirectToAction("Index");
                }
                catch (JobNameAlreadyExistsException ex)
                {
                    TempData["warning-title"] = "Création poste";
                    TempData["warning-message"] = "Le nom de ce poste existe déjà";
                    return View(createJobViewModel);
                }
                catch (JobCodeAlreadyExistsException ex)
                {
                    TempData["warning-title"] = "Création poste";
                    TempData["warning-message"] = "Le code de ce poste existe déjà";
                    return View(createJobViewModel);
                }
                catch (Exception ex)
                {
                    this.applicationUnitOfWork.Log.CreateNewEventInlog(ex, User, "Erreur lors de l'ajout d'un poste dans la base de données", "Exception", LogType.Error);
                    TempData["warning-title"] = "Création poste";
                    TempData["warning-message"] = "Erreur lors de la création d'un poste";
                    return View(createJobViewModel);
                }
            }
            return View();
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAllJobs()
        {
            Jobs.Clear();
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
                                var query = "SELECT Job.Id, Job.Code, Job.IsEnable, Job.[Name], COUNT(DISTINCT AspNetUsers.Id) AS UsersCount FROM Job LEFT JOIN AspNetUsers ON Job.Id = AspNetUsers.JobId GROUP BY Job.Id, Job.Code, Job.IsEnable, Job.[Name];";
                                SqlCommand cmd = new SqlCommand(query, connection);
                                SqlDataReader reader = cmd.ExecuteReader();
                                while (reader.Read())
                                {
                                    Jobs.Add(new IndexJobViewModel
                                    {
                                        Id = reader.GetString(0),
                                        Code = reader.GetString(1),
                                        EmployeesNumber = reader.GetInt32(4),
                                        IsEnable = reader.GetBoolean(2),
                                        Name = reader.GetString(3),
                                    });
                                }
                                reader.Close();
                                connection.Close();
                                return Json(new { data = Jobs });
                            }
                            catch (Exception ex)
                            {
                                this.applicationUnitOfWork.Log.CreateNewEventInlog(ex, User, "Une erreur est survenue lors de la récupération des postes", "Exception", LogType.Error);
                                TempData["error-title"] = "Modification état poste";
                                TempData["error-message"] = "Une erreur est survenue lors de la récupération des postes";
                                return Json(null);
                            }
                        }
                    }
                    return Json(null);
                }
                return Json(null);
            }
            return Json(null);
        }

        [HttpPost]
        public IActionResult LockUnlockJob([FromBody] string id)
        {
            var objFromDb = new Job();
            try
            {
                objFromDb = this.applicationUnitOfWork.Job.Get(u => u.Id == id);
                if (objFromDb is null)
                    return Json(new { success = false, message = "Une erreur est survenue lors de l'activation/la desactivation du poste" });
                if (objFromDb.IsEnable == false)
                    objFromDb.IsEnable = true;
                else
                    objFromDb.IsEnable = false;

                this.applicationUnitOfWork.Job.Update(objFromDb);
                this.applicationUnitOfWork.Save();
                if (objFromDb.IsEnable == true)
                {
                    this.applicationUnitOfWork.Log.CreateNewEventInlog(null, User, $"Le poste {objFromDb.Name} a bien été activé", "", LogType.Success);
                    TempData["success-title"] = "Activation poste";
                    TempData["success-message"] = $"Le poste {objFromDb.Name} a bien été activé";
                }
                else if (objFromDb.IsEnable == false)
                {
                    this.applicationUnitOfWork.Log.CreateNewEventInlog(null, User, $"Le poste {objFromDb.Name} a bien été desactivé", "", LogType.Success);
                    TempData["success-title"] = "Désactivation poste";
                    TempData["success-message"] = $"Le poste {objFromDb.Name} a bien été desactivé";
                }
            }
            catch (Exception ex)
            {
                this.applicationUnitOfWork.Log.CreateNewEventInlog(ex, User, $"Erreur lors de la modification de l'état du poste {objFromDb.Name} dans la base de données", "Exception", LogType.Error);
                TempData["error-title"] = "Modification état poste";
                TempData["error-message"] = $"Erreur lors de la modification de l'état du poste {objFromDb.Name} dans la base de données";
            }

            return Json(new { success = true, message = "Activation/désactivation du poste réussie" });
        }
        #endregion
    }
}
