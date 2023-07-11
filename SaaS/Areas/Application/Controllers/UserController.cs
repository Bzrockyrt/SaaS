using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using SaaS.DataAccess.Repository.IRepository;
using SaaS.DataAccess.Services;
using SaaS.DataAccess.Utils;
using SaaS.Domain;
using SaaS.Domain.Identity;
using SaaS.Utility;
using SaaS.ViewModels.Application.User;

namespace SaaS.Areas.Application.Controllers
{
    [Area("Application")]
    public class UserController : Controller
    {
        private readonly IApplicationUnitOfWork applicationUnitOfWork;
        private readonly TenantService tenantService;
        private readonly TenantSettings tenantSettings;

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
