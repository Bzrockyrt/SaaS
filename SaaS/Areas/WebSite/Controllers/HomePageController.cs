using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SaaS.DataAccess.Repository.IRepository;
using SaaS.DataAccess.Services;
using SaaS.DataAccess.Utils;
using SaaS.ViewModels.Application.Connection;

namespace SaaS.Areas.WebSite.Controllers
{
    [Area("WebSite")]
    public class HomePageController : Controller
    {
        private readonly ILogger<LoginViewModel> logger;
        private readonly IUnitOfWork unitOfWork;
        /*private readonly SignInManager<IdentityUser> signInManager;*/
        private readonly TenantSettings tenantSettings;
        private readonly TenantService tenantService;
        /*private readonly UserManager<IdentityUser> userManager;*/

        public HomePageController(ILogger<LoginViewModel> logger,
            IUnitOfWork unitOfWork,
            /*SignInManager<IdentityUser> signinManager,*/
            IOptions<TenantSettings> options,
            TenantService tenantService
            /*UserManager<IdentityUser> userManager*/)
        {
            this.logger = logger;
            this.unitOfWork = unitOfWork;
            /*this.signInManager = signinManager;*/
            this.tenantSettings = options.Value;
            this.tenantService = tenantService;
            /*this.userManager = userManager;*/
        }

        public IActionResult HomePage()
        {
            return View();
        }


        #region Login
        #endregion

        #region Signup
        #endregion
    }
}
