using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using SaaS.DataAccess.Repository.IRepository;
using SaaS.DataAccess.Services;
using SaaS.DataAccess.Utils;
using SaaS.Domain;
using SaaS.Domain.Identity;
using SaaS.Utility;
using SaaS.ViewModels.Application.Department;
using SaaS.ViewModels.Application.User;

namespace SaaS.Areas.Application.Controllers
{
    [Area("Application")]
    public class UserController : Controller
    {
        private readonly IApplicationUnitOfWork applicationUnitOfWork;
        private readonly TenantService tenantService;
        private readonly TenantSettings tenantSettings;

        public static List<IndexUserViewModel> Users = new List<IndexUserViewModel>();

        public UserController(IApplicationUnitOfWork applicationUnitOfWork, 
            TenantService tenantService, 
            IOptions<TenantSettings> options)
        {
            this.applicationUnitOfWork = applicationUnitOfWork;
            this.tenantService = tenantService;
            this.tenantSettings = options.Value;
        }

        public IActionResult Index()
        {
            return View();
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
                                        Id = reader.IsDBNull(0) ? "vide" : reader.GetString(0),
                                        UserName = reader.IsDBNull(1) ? "vide" : reader.GetString(1),
                                        Email = reader.IsDBNull(2) ? "vide" : reader.GetString(2),
                                        PhoneNumber = reader.IsDBNull(3) ? "vide" : reader.GetString(3),
                                        IsEnable = reader.GetBoolean(4),
                                        JobName = reader.IsDBNull(5) ? "vide" : reader.GetString(5),
                                        DepartmentName = reader.IsDBNull(6) ? "vide" : reader.GetString(6),
                                        SubsidiaryName = reader.IsDBNull(7) ? "vide" : reader.GetString(7),
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
        #endregion
    }
}
