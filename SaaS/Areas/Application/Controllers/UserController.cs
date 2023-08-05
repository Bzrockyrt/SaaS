using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using SaaS.DataAccess.Repository.IRepository;
using SaaS.DataAccess.Services;
using SaaS.DataAccess.Utils;
using SaaS.Domain;
using SaaS.Domain.Identity;
using SaaS.Domain.Work;
using SaaS.ViewModels.Application.User;

namespace SaaS.Areas.Application.Controllers
{
    [Area("Application")]
    public class UserController : Controller
    {
        private readonly IApplicationUnitOfWork applicationUnitOfWork;
        private readonly IUserStore<IdentityUser> userStore;
        private readonly IUserEmailStore<IdentityUser> emailStore;
        private readonly TenantService tenantService;
        private readonly TenantSettings tenantSettings;
        private readonly UserManager<IdentityUser> userManager;

        public static List<IndexUserViewModel> Users = new List<IndexUserViewModel>();
        public static DetailsUserViewModel detailsUserViewModel = new DetailsUserViewModel();

        public UserController(IApplicationUnitOfWork applicationUnitOfWork, 
            TenantService tenantService,
            IUserStore<IdentityUser> userStore,
            IOptions<TenantSettings> options,
            UserManager<IdentityUser> userManager)
        {
            this.applicationUnitOfWork = applicationUnitOfWork;
            this.tenantService = tenantService;
            this.tenantSettings = options.Value;
            this.userStore = userStore;
            this.userManager = userManager;
            this.emailStore = GetEmailStore();
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            CreateUserViewModel createUserViewModel = new CreateUserViewModel()
            {
                SubsidiaryList = this.applicationUnitOfWork.Subsidiary.GetAll().Select(d => new SelectListItem
                {
                    Text = d.Name,
                    Value = d.Id
                }),
                User = new User()
            };
            return View(createUserViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel createUserViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (createUserViewModel.Password != createUserViewModel.ConfirmPassword)
                    {
                        ModelState.AddModelError(string.Empty, "Les mots de passes ne correspondent pas");
                        return View(createUserViewModel);
                    }

                    var user = CreateUser();
                    //Je crée le nom d'utilisateur à partir du nom et du prénom de l'utilisateur
                    var username = createUserViewModel.User.Lastname.Substring(0, Math.Min(createUserViewModel.User.Lastname.Length, 3))
                        + createUserViewModel.User.Firstname.Substring(0, Math.Min(createUserViewModel.User.Firstname.Length, 3));
                    user.Firstname = createUserViewModel.User.Firstname;
                    user.Lastname = createUserViewModel.User.Lastname;
                    user.JobId = createUserViewModel.JobId;
                    await userStore.SetUserNameAsync(user, username, CancellationToken.None);
                    await emailStore.SetEmailAsync(user, createUserViewModel.User.Email, CancellationToken.None);

                    var result = await this.userManager.CreateAsync(user, createUserViewModel.Password);
                    if (result.Succeeded)
                    {
                        var userId = await this.userManager.GetUserIdAsync(user);
                        var code = await this.userManager.GenerateEmailConfirmationTokenAsync(user);

                        return View("Index");
                    }
                    return View(createUserViewModel);
                }
                catch (Exception ex)
                {
                    return View(createUserViewModel);
                }
            }
            return View(createUserViewModel);
        }

        [HttpGet]
        public IActionResult Details(string? id)
        {
            if (id == null || string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            detailsUserViewModel.User = this.applicationUnitOfWork.User.Get(s => s.Id == id);
            detailsUserViewModel.SubsidiaryList = this.applicationUnitOfWork.Subsidiary.GetAll().Select(d => new SelectListItem
            {
                Text = d.Name,
                Value = d.Id
            });

            if (detailsUserViewModel.User is null)
            {
                return NotFound();
            }
            return View(detailsUserViewModel);
        }

        [HttpPost]
        public IActionResult Details(DetailsUserViewModel detailsUserViewModel)
        {
            if (ModelState.IsValid)
            {
                detailsUserViewModel.User.UpdatedOn = DateTime.Now;
                detailsUserViewModel.User.UpdatedBy = User?.Identity.Name;
                this.applicationUnitOfWork.User.Update(detailsUserViewModel.User);
                this.applicationUnitOfWork.Save();

                this.applicationUnitOfWork.Log.CreateNewEventInlog(null, User, $"L'employé a bien été modifié", "", LogType.Success);
                TempData["success-title"] = "Modification employé";
                TempData["success-message"] = $"L'employé a bien été modifié";
                return RedirectToAction("Index");
            }
            return View(detailsUserViewModel);
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            Users.Clear();
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
                                var query = "SELECT AspNetUsers.Id, AspNetUsers.UserName, AspNetUsers.Email, AspNetUsers.PhoneNumber, AspNetUsers.IsEnable, Job.[Name] JobName, Department.[Name] DepartmentName, Subsidiary.[Name] SubsidiaryName FROM Subsidiary RIGHT JOIN Department ON Subsidiary.Id = Department.SubsidiaryId RIGHT JOIN Job ON Department.Id = Job.DepartmentId RIGHT JOIN AspNetUsers ON Job.Id = AspNetUsers.JobId GROUP BY AspNetUsers.Id, AspNetUsers.UserName, AspNetUsers.Email, AspNetUsers.PhoneNumber, AspNetUsers.IsEnable, Job.[Name], Department.[Name], Subsidiary.[Name];";
                                SqlCommand cmd = new SqlCommand(query, connection);
                                SqlDataReader reader = cmd.ExecuteReader();
                                while (reader.Read())
                                {
                                    Users.Add(new IndexUserViewModel
                                    {
                                        Id = reader.IsDBNull(0) ? "" : reader.GetString(0),
                                        UserName = reader.IsDBNull(1) ? "" : reader.GetString(1),
                                        Email = reader.IsDBNull(2) ? "" : reader.GetString(2),
                                        PhoneNumber = reader.IsDBNull(3) ? "" : reader.GetString(3),
                                        IsEnable = reader.GetBoolean(4),
                                        JobName = reader.IsDBNull(5) ? "" : reader.GetString(5),
                                        DepartmentName = reader.IsDBNull(6) ? "" : reader.GetString(6),
                                        SubsidiaryName = reader.IsDBNull(7) ? "" : reader.GetString(7),
                                    });
                                }
                                reader.Close();
                                connection.Close();
                                return Json(new { data = Users });
                            }
                            catch (Exception ex)
                            {
                                this.applicationUnitOfWork.Log.CreateNewEventInlog(ex, User, "Une erreur est survenue lors de la récupération des employés", "Exception", LogType.Error);
                                TempData["error-title"] = "Récupération employés";
                                TempData["error-message"] = "Une erreur est survenue lors de la récupération des employés";
                                return Json(null);
                            }
                        }
                    }
                }
            }

            IEnumerable<User> users = this.applicationUnitOfWork.User.GetAll();

            return Json(new { data = users });
        }

        [HttpPost]
        public IActionResult LockUnlockUser([FromBody] string id)
        {
            var objFromDb = new User();
            try
            {
                objFromDb = this.applicationUnitOfWork.User.Get(u => u.Id == id);
                if (objFromDb is null)
                    return Json(new { success = false, message = "Une erreur est survenue lors de l'activation/la desactivation de l'utilisateur" });
                if(objFromDb.IsEnable == false)
                    objFromDb.IsEnable = true;
                else
                    objFromDb.IsEnable = false;

                this.applicationUnitOfWork.User.Update(objFromDb);
                this.applicationUnitOfWork.Save();
                if(objFromDb.IsEnable == true)
                {
                    this.applicationUnitOfWork.Log.CreateNewEventInlog(null, User, $"L'utilisateur {objFromDb.UserName} a bien été activé", "", LogType.Success);
                    TempData["success-title"] = "Activation utilisateur";
                    TempData["success-message"] = $"L'utilisateur {objFromDb.UserName} a bien été activé";
                }
                else if (objFromDb.IsEnable == false)
                {
                    this.applicationUnitOfWork.Log.CreateNewEventInlog(null, User, $"L'utilisateur {objFromDb.UserName} a bien été desactivé", "", LogType.Success);
                    TempData["success-title"] = "Désactivation utilisateur";
                    TempData["success-message"] = $"L'utilisateur {objFromDb.UserName} a bien été desactivé";
                }
            }
            catch (Exception ex)
            {
                this.applicationUnitOfWork.Log.CreateNewEventInlog(ex, User, $"Erreur lors de la modification de l'état de l'utilisateur {objFromDb.UserName} dans la base de données", "Exception", LogType.Error);
                TempData["error-title"] = "Modification état utilisateur";
                TempData["error-message"] = $"Erreur lors de la modification de l'état de l'utilisateur {objFromDb.UserName} dans la base de données";
            }

            return Json(new { success = true, message = "Activation/désactivation de l'utilisateur réussie" });
        }

        [HttpPost]
        public IActionResult GetDepartments([FromBody] string subsidiaryId)
        {
            List<SelectListItem> departmentList = new List<SelectListItem>();

            List<Department> departments = this.applicationUnitOfWork.Department.GetAll().Where(d => d.SubsidiaryId == subsidiaryId).ToList();
            foreach (Department dep in departments)
            {
                departmentList.Add(new SelectListItem
                {
                    Value = dep.Id,
                    Text = dep.Name,
                });
            }
            return Json(new { data = departmentList });
        }

        [HttpPost]
        public IActionResult GetJobs([FromBody] string departmentId)
        {
            List<SelectListItem> jobList = new List<SelectListItem>();

            List<Job> jobs = this.applicationUnitOfWork.Job.GetAll().Where(j => j.DepartmentId == departmentId).ToList();
            foreach (Job job in jobs)
            {
                jobList.Add(new SelectListItem
                {
                    Value = job.Id,
                    Text = job.Name,
                });
            }
            return Json(new { data = jobList });
        }

        [HttpDelete]
        public IActionResult Delete(string? id)
        {
            if (id is not null)
            {
                try
                {
                    User user = this.applicationUnitOfWork.User.Get(ws => ws.Id == id);
                    this.applicationUnitOfWork.User.Delete(user);
                    this.applicationUnitOfWork.Save();
                }
                catch (Exception ex)
                {

                }
            }
            return View("Index");
        }
        #endregion

        private User CreateUser()
        {
            try
            {
                return Activator.CreateInstance<User>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                    $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<IdentityUser> GetEmailStore()
        {
            if (!this.userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<IdentityUser>)userStore;
        }
    }
}
