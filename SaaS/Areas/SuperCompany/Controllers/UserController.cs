using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using SaaS.DataAccess.Repository.PIPL.IRepository;
using SaaS.DataAccess.Services;
using SaaS.DataAccess.Utils;
using SaaS.Domain;
using SaaS.Domain.PIPL;
using SaaS.ViewModels.SuperCompany.User;

namespace SaaS.Areas.SuperCompany.Controllers
{
    [Area("SuperCompany")]
    public class UserController : Controller
    {
        private readonly ISuperCompanyUnitOfWork superCompanyUnitOfWork;
        private readonly TenantService tenantService;
        private readonly TenantSettings tenantSettings;

        public static List<UserViewModel> Users = new List<UserViewModel>();

        public UserController(ISuperCompanyUnitOfWork superCompanyUnitOfWork,
            TenantService tenantService,
            IOptions<TenantSettings> options)
        {
            this.superCompanyUnitOfWork = superCompanyUnitOfWork;
            this.tenantService = tenantService;
            this.tenantSettings = options.Value;
        }

        public IActionResult Index()
        {
            /*if (this.unitOfWork.User.CanUserAccessFunctionnality("Access_User_Index", User))
            {
                return View();
            }
            else 
            {
                return RedirectToAction("AccessDenied", "Connection", new { area = "Application" });
            }*/
            return View();
        }

        #region API CALLS
        /// <summary>
        /// Pour chaque entreprise, je dois récupérer l'ensemble des utilisateurs.
        /// Il faut donc d'abord que je récupère chaque chaîne de connexions de chaque entreprise.
        /// Et ensuite je dois récupérer chaque utilisateurs dans la table User de la DB de chaque entreprise.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAllUsersWithCompanies()
        {
            Users.Clear();
            if (this.tenantSettings.Companies is not null)
            {
                foreach (TenantData tenantData in this.tenantSettings.Companies.Values)
                {
                    using (SqlConnection connection = new SqlConnection(tenantData.connectionString))
                    {
                        try
                        {
                            connection.Open();
                            var query = "SELECT AspNetUsers.Id, AspNetUsers.UserName, Company.[Name], AspNetUsers.IsEnable FROM AspNetUsers INNER JOIN Department ON AspNetUsers.DepartmentId = Department.Id INNER JOIN Company ON Department.CompanyId = Company.Id;";
                            SqlCommand cmd = new SqlCommand(query, connection);
                            SqlDataReader reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                Users.Add(new UserViewModel
                                {
                                    Id = reader.GetString(0),
                                    Username = reader.GetString(1),
                                    CompanyName = reader.GetString(2),
                                    IsEnable = reader.GetBoolean(3),
                                    ConnectionString = tenantData.connectionString,
                                });
                            }
                            reader.Close();
                            connection.Close();
                            return Json(new { data = Users });
                        }
                        catch (Exception ex)
                        {
                            /*using (var engine = new V8ScriptEngine())
                            {
                                engine.Execute("toastr.success('test toastr en C#');");
                            }*/
                            this.superCompanyUnitOfWork.Log.CreateNewEventInlog(ex, User, "Une erreur est survenue lors de la récupération des utilisateurs", "Exception", LogType.Error);
                            TempData["error-title"] = "Modification état entreprise";
                            TempData["error-message"] = "Une erreur est survenue lors de la récupération des utilisateurs";
                            return Json(null);
                        }
                    }
                }
                return Json(null);
            }
            return Json(null);
        }


        /// <summary>
        /// Logique de modification d'état des utilisateurs.
        /// Il est possible d'activer ou de desactiver un utilisateur. Lorsqu'il est desactivé, l'utilisateur correspondant ne
        /// peux plus avoir avvès à son compte personnel.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult LockUnlockUser([FromBody] string id)
        {
            var objFromDb = Users.FirstOrDefault(c => c.Id == id);
            if (objFromDb is null)
                return Json(new { success = false, message = "Une erreur est survenue lors de l'activation/la désactivation de l'utilisateur" });
            else
            {
                using (SqlConnection connection = new SqlConnection(objFromDb.ConnectionString))
                {
                    var query = "";
                    connection.Open();
                    if (objFromDb.IsEnable)
                    {
                        query = $"UPDATE AspNetUsers SET IsEnable = 0 WHERE Id = '{id}';";
                    }
                    else
                    {
                        query = $"UPDATE AspNetUsers SET IsEnable = 1 WHERE Id = '{id}';";
                    }

                    SqlCommand command = new SqlCommand(query, connection);
                    int rowsAffected = command.ExecuteNonQuery();
                    connection.Close();
                }

                return Json(new { success = true, message = "Activation/désactivation de l'entreprise réussie" });
            }
        }
        #endregion
    }
}
