using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using SaaS.DataAccess.Exceptions.Application.Department;
using SaaS.DataAccess.Repository.IRepository;
using SaaS.DataAccess.Services;
using SaaS.DataAccess.Utils;
using SaaS.Domain;
using SaaS.Domain.Identity;
using SaaS.ViewModels.Application.Department;

namespace SaaS.Areas.Application.Controllers
{
    [Area("Application")]
    public class DepartmentController : Controller
    {
        private readonly IApplicationUnitOfWork applicationUnitOfWork;
        private readonly TenantService tenantService;
        private readonly TenantSettings tenantSettings;

        public static List<IndexDepartmentViewModel> Departments = new List<IndexDepartmentViewModel>();
        public static DetailsDepartmentViewModel detailsDepartmentViewModel = new DetailsDepartmentViewModel();
        
        public DepartmentController(IApplicationUnitOfWork applicationUnitOfWork,
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
            CreateDepartmentViewModel departmentViewModel = new CreateDepartmentViewModel()
            {
                SubsidiaryList = this.applicationUnitOfWork.Subsidiary.GetAll().Select(d => new SelectListItem
                {
                    Text = d.Name,
                    Value = d.Id
                }),
                Department = new Department()
            };
            return View(departmentViewModel);
        }

        [HttpPost]
        public IActionResult Create(CreateDepartmentViewModel createDepartmentViewModel)
        {
            if (ModelState.IsValid)
            {
                if (User?.Identity?.Name is null)
                    createDepartmentViewModel.Department.CreatorId = string.Empty;
                else
                    createDepartmentViewModel.Department.CreatorId = this.applicationUnitOfWork.User.Get(u => u.UserName == User.Identity.Name).Id;

                try
                {
                    IEnumerable<Department> departments = this.applicationUnitOfWork.Department.GetAll();
                    foreach (Department dep in departments)
                    {
                        if (dep.Name == createDepartmentViewModel.Department.Name)
                        {
                            throw new DepartmentNameAlreadyExistsException();
                        }
                        if (dep.Code == createDepartmentViewModel.Department.Code)
                        {
                            throw new DepartmentCodeAlreadyExistsException();
                        }
                    }

                    this.applicationUnitOfWork.Department.Add(createDepartmentViewModel.Department);
                    this.applicationUnitOfWork.Save();
                    this.applicationUnitOfWork.Log.CreateNewEventInlog(null, User, $"Le département a bien été ajouté à la base de données", "", LogType.Success);
                    TempData["success-title"] = "Création département";
                    TempData["success-message"] = "Le département a bien été créé";
                    return RedirectToAction("Index");
                }
                catch (DepartmentNameAlreadyExistsException ex)
                {
                    TempData["warning-title"] = "Création département";
                    TempData["warning-message"] = "Le nom de ce département existe déjà";
                    return View(createDepartmentViewModel);
                }
                catch (DepartmentCodeAlreadyExistsException ex)
                {
                    TempData["warning-title"] = "Création département";
                    TempData["warning-message"] = "Le code de ce département existe déjà";
                    return View(createDepartmentViewModel);
                }
                catch (Exception ex)
                {
                    this.applicationUnitOfWork.Log.CreateNewEventInlog(ex, User, "Erreur lors de l'ajout d'un département dans la base de données", "Exception", LogType.Error);
                    TempData["warning-title"] = "Création département";
                    TempData["warning-message"] = "Erreur lors de la création d'un département";
                    return View(createDepartmentViewModel);
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

            detailsDepartmentViewModel.Department = this.applicationUnitOfWork.Department.Get(s => s.Id == id);
            detailsDepartmentViewModel.SubsidiaryList = this.applicationUnitOfWork.Subsidiary.GetAll().Select(d => new SelectListItem
            {
                Text = d.Name,
                Value = d.Id
            });
            
            if (detailsDepartmentViewModel.Department is null)
            {
                return NotFound();
            }
            return View(detailsDepartmentViewModel);
        }

        [HttpPost]
        public IActionResult Update(DetailsDepartmentViewModel detailsDepartmentViewModel)
        {
            if (ModelState.IsValid)
            {
                detailsDepartmentViewModel.Department.UpdatedOn = DateTime.Now;
                detailsDepartmentViewModel.Department.UpdatedBy = User?.Identity.Name;
                this.applicationUnitOfWork.Department.Update(detailsDepartmentViewModel.Department);
                this.applicationUnitOfWork.Save();

                this.applicationUnitOfWork.Log.CreateNewEventInlog(null, User, $"Le département a bien été modifié", "", LogType.Success);
                TempData["success-title"] = "Modification département";
                TempData["success-message"] = $"Le département a bien été modifié";
                return RedirectToAction("Index");
            }
            return View(detailsDepartmentViewModel);
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAllDepartments()
        {
            Departments.Clear();
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
                                var query = "SELECT Department.Id, Department.Code, Department.IsEnable, Department.[Name], COUNT(DISTINCT Job.Id), COUNT(DISTINCT AspNetUsers.Id), Subsidiary.[Name] FROM Subsidiary RIGHT JOIN Department ON Subsidiary.Id = Department.SubsidiaryId LEFT JOIN Job ON Department.Id = Job.DepartmentId LEFT JOIN AspNetUsers ON Job.Id = AspNetUsers.JobId GROUP BY Department.Id, Department.Code, Department.IsEnable, Department.[Name], Subsidiary.[Name];";
                                SqlCommand cmd = new SqlCommand(query, connection);
                                SqlDataReader reader = cmd.ExecuteReader();
                                while (reader.Read())
                                {
                                    Departments.Add(new IndexDepartmentViewModel
                                    {
                                        Id = reader.IsDBNull(0) ? "" : reader.GetString(0),
                                        Code = reader.IsDBNull(0) ? "" : reader.GetString(1),
                                        EmployeesNumber = reader.IsDBNull(0) ? 0 : reader.GetInt32(5),
                                        SubsidiaryName = reader.IsDBNull(0) ? "" : reader.GetString(6),
                                        IsEnable = reader.GetBoolean(2),
                                        JobsNumber = reader.IsDBNull(0) ? 0 : reader.GetInt32(4),
                                        Name = reader.IsDBNull(0) ? "" : reader.GetString(3),
                                    });
                                }
                                reader.Close();
                                connection.Close();
                                return Json(new { data = Departments });
                            }
                            catch (Exception ex)
                            {
                                this.applicationUnitOfWork.Log.CreateNewEventInlog(ex, User, "Une erreur est survenue lors de la récupération des départements", "Exception", LogType.Error);
                                TempData["error-title"] = "Récupération département";
                                TempData["error-message"] = "Une erreur est survenue lors de la récupération des départements";
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
        public IActionResult LockUnlockDepartment([FromBody] string id)
        {
            var objFromDb = new Department();
            try
            {
                objFromDb = this.applicationUnitOfWork.Department.Get(u => u.Id == id);
                if (objFromDb is null)
                    return Json(new { success = false, message = "Une erreur est survenue lors de l'activation/la desactivation du département" });
                if (objFromDb.IsEnable == false)
                    objFromDb.IsEnable = true;
                else
                    objFromDb.IsEnable = false;

                this.applicationUnitOfWork.Department.Update(objFromDb);
                this.applicationUnitOfWork.Save();
                if (objFromDb.IsEnable == true)
                {
                    this.applicationUnitOfWork.Log.CreateNewEventInlog(null, User, $"Le département {objFromDb.Name} a bien été activé", "", LogType.Success);
                    TempData["success-title"] = "Activation département";
                    TempData["success-message"] = $"Le département {objFromDb.Name} a bien été activé";
                }
                else if (objFromDb.IsEnable == false)
                {
                    this.applicationUnitOfWork.Log.CreateNewEventInlog(null, User, $"Le département {objFromDb.Name} a bien été desactivé", "", LogType.Success);
                    TempData["success-title"] = "Désactivation département";
                    TempData["success-message"] = $"Le département {objFromDb.Name} a bien été desactivé";
                }
            }
            catch (Exception ex)
            {
                this.applicationUnitOfWork.Log.CreateNewEventInlog(ex, User, $"Erreur lors de la modification de l'état du département {objFromDb.Name} dans la base de données", "Exception", LogType.Error);
                TempData["error-title"] = "Modification état département";
                TempData["error-message"] = $"Erreur lors de la modification de l'état du département {objFromDb.Name} dans la base de données";
            }

            return Json(new { success = true, message = "Activation/désactivation du département réussie" });
        }
        #endregion
    }
}
