using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using SaaS.DataAccess.Exceptions.Application.Job;
using SaaS.DataAccess.Repository.IRepository;
using SaaS.DataAccess.Services;
using SaaS.DataAccess.Utils;
using SaaS.Domain;
using SaaS.Domain.Company;
using SaaS.Domain.Identity;
using SaaS.ViewModels.Application.Job;

namespace SaaS.Areas.Application.Controllers
{
    [Area("Application")]
    public class JobController : Controller
    {
        private readonly IApplicationUnitOfWork applicationUnitOfWork;
        private readonly TenantService tenantService;
        private readonly TenantSettings tenantSettings;

        public static List<IndexJobViewModel> Jobs = new List<IndexJobViewModel>();
        public static DetailsJobViewModel detailsJobViewModel = new DetailsJobViewModel();

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
                    createJobViewModel.Job.CreatorId = string.Empty;
                else
                    createJobViewModel.Job.CreatorId = this.applicationUnitOfWork.User.GetAll().FirstOrDefault(u => u.UserName == User?.Identity?.Name).Id;

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

        [HttpGet]
        public IActionResult Details(string? id)
        {
            if (id == null || string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            detailsJobViewModel.Job = this.applicationUnitOfWork.Job.Get(s => s.Id == id);
            detailsJobViewModel.DepartmentList = this.applicationUnitOfWork.Department.GetAll().Select(d => new SelectListItem
            {
                Text = d.Name,
                Value = d.Id
            });

            if (detailsJobViewModel.Job is null)
            {
                return NotFound();
            }

            IEnumerable<CompanyFunctionnalities> companyFunctionnalities = this.applicationUnitOfWork.CompanyFunctionnalities.GetAll();

            detailsJobViewModel.HaveFunctionnalities.Clear();
            detailsJobViewModel.DontHaveFunctionnalities.Clear();

            foreach (CompanyFunctionnalities cf in companyFunctionnalities)
            {
                if (this.applicationUnitOfWork.Job_CompanyFunctionnalities.GetAll().Any(jcf => jcf.CompanyFunctionnalitiesId == cf.Id && jcf.JobId == detailsJobViewModel.Job.Id))
                    detailsJobViewModel.HaveFunctionnalities.Add(cf);
                else
                    detailsJobViewModel.DontHaveFunctionnalities.Add(cf);
            }

            return View(detailsJobViewModel);
        }

        [HttpPost]
        public IActionResult Details(DetailsJobViewModel detailsJobViewModel)
        {
            if (ModelState.IsValid)
            {
                detailsJobViewModel.Job.UpdatedOn = DateTime.Now;
                detailsJobViewModel.Job.UpdatedBy = User?.Identity.Name;
                this.applicationUnitOfWork.Job.Update(detailsJobViewModel.Job);
                this.applicationUnitOfWork.Save();

                this.applicationUnitOfWork.Log.CreateNewEventInlog(null, User, $"Le poste a bien été modifié", "", LogType.Success);
                TempData["success-title"] = "Modification poste";
                TempData["success-message"] = $"Le poste a bien été modifié";
                return RedirectToAction("Index");
            }
            return View(detailsJobViewModel);
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
                                var query = "SELECT Job.Id, Job.Code, Job.IsEnable, Job.[Name], COUNT(DISTINCT AspNetUsers.Id), Department.[Name], Subsidiary.[Name] FROM Subsidiary RIGHT JOIN Department ON Subsidiary.Id = Department.SubsidiaryId RIGHT JOIN Job ON Department.Id = Job.DepartmentId LEFT JOIN AspNetUsers ON Job.Id = AspNetUsers.JobId GROUP BY Job.Id, Job.Code, Job.IsEnable, Job.[Name], Department.[Name], Subsidiary.[Name];";
                                SqlCommand cmd = new SqlCommand(query, connection);
                                SqlDataReader reader = cmd.ExecuteReader();
                                while (reader.Read())
                                {
                                    Jobs.Add(new IndexJobViewModel
                                    {
                                        Id = reader.IsDBNull(0) ? "" : reader.GetString(0),
                                        Code = reader.IsDBNull(0) ? "" : reader.GetString(1),
                                        EmployeesNumber = reader.IsDBNull(0) ? 0 : reader.GetInt32(4),
                                        SubsidiaryName = reader.IsDBNull(0) ? "" : reader.GetString(6),
                                        DepartmentName = reader.IsDBNull(0) ? "" : reader.GetString(5),
                                        IsEnable = reader.GetBoolean(2),
                                        Name = reader.IsDBNull(0) ? "" : reader.GetString(3),
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

        [HttpPost]
        public IActionResult AddFunctionnalityToJob(string functionnalityName)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    CompanyFunctionnalities companyFunctionnalities = this.applicationUnitOfWork.CompanyFunctionnalities.Get(f => f.Name == functionnalityName);

                    Job_CompanyFunctionnalities job_CompanyFunctionnalities = new Job_CompanyFunctionnalities()
                    {
                        CompanyFunctionnalitiesId = companyFunctionnalities.Id,
                        JobId = detailsJobViewModel.Job.Id,
                    };

                    this.applicationUnitOfWork.Job_CompanyFunctionnalities.Add(job_CompanyFunctionnalities);
                    this.applicationUnitOfWork.Save();

                    this.applicationUnitOfWork.Log.CreateNewEventInlog(null, User, $"La fonctionnalité {functionnalityName} a bien été ajoutée au poste", "", LogType.Success);
                    TempData["success-title"] = "Ajout fonctionnalité poste";
                    TempData["success-message"] = "La fonctionnalité a bien été ajoutée au poste";
                }
            }
            catch (Exception ex)
            {
                this.applicationUnitOfWork.Log.CreateNewEventInlog(ex, User, $"Erreur lors de l'ajout de la fonctionnalité {functionnalityName} au poste dans la base de données", "Exception", LogType.Error);
                TempData["error-title"] = "Ajout fonctionnalité poste";
                TempData["error-message"] = $"Erreur lors de l'ajout de la fonctionnalité {functionnalityName} au poste dans la base de données";
            }

            return View();
        }

        [HttpPost]
        public IActionResult DeleteFunctionnalityToJob(string functionnalityName)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    CompanyFunctionnalities companyFunctionnalities = this.applicationUnitOfWork.CompanyFunctionnalities.Get(f => f.Name == functionnalityName);

                    Job_CompanyFunctionnalities job_CompanyFunctionnalities = this.applicationUnitOfWork.Job_CompanyFunctionnalities.Get(jcf => jcf.CompanyFunctionnalitiesId == companyFunctionnalities.Id);

                    this.applicationUnitOfWork.Job_CompanyFunctionnalities.Delete(job_CompanyFunctionnalities);
                    this.applicationUnitOfWork.Save();

                    this.applicationUnitOfWork.Log.CreateNewEventInlog(null, User, $"La fonctionnalité {functionnalityName} a bien été supprimée du poste", "", LogType.Success);
                    TempData["success-title"] = "Suppression fonctionnalité poste";
                    TempData["success-message"] = "La fonctionnalité a bien été supprimée du poste";
                }
            }
            catch (Exception ex)
            {
                this.applicationUnitOfWork.Log.CreateNewEventInlog(ex, User, $"Erreur lors de la suppression de la fonctionnalité {functionnalityName} au poste dans la base de données", "Exception", LogType.Error);
                TempData["error-title"] = "Suppression fonctionnalité entreprise";
                TempData["error-message"] = $"Erreur lors de la suppression de la fonctionnalité {functionnalityName} au poste dans la base de données";
            }

            return View();
        }
        #endregion
    }
}
