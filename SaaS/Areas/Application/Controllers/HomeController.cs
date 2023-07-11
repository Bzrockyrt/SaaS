using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SaaS.DataAccess.Services;
using SaaS.DataAccess.Utils;
using SaaS.Domain.Identity;
using SaaS.Domain.Tenant;

namespace SaaS.Areas.Application.Controllers
{
    [Area("Application")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private TenantSettings tenantSettings;
        private TenantService tenantService;
        private readonly UserManager<IdentityUser> userManager;

        public HomeController(ILogger<HomeController> logger,
            IOptions<TenantSettings> options,
            TenantService tenantService,
            UserManager<IdentityUser> userManager)
        {
            this.logger = logger;
            this.tenantSettings = options.Value;
            this.tenantService = tenantService;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            var company = tenantService.GetTenant();
            if (company != null)
            {
                ViewBag.SelectedSite = new TenantSiteModel
                {
                    Key = tenantService.GetTenantCode(),
                    Logo = company.logo,
                    Name = company.name
                };
            }

            var companies = tenantSettings.Companies.Select(s => new TenantSiteModel
            {
                Key = s.Key,
                Logo = s.Value.logo,
                Name = s.Value.name
            }).ToList();

            return View(companies);
        }

        public async Task<IActionResult> SelectSite(string company)
        {
            if (this.tenantSettings.Companies.ContainsKey(company))
                Response.Cookies.Append("tenant-code", company);

            var user = new User();

            user.UserName = "Test-User1";
            user.Email = "test@user1.com";

            var result = await userManager.CreateAsync(user, "Test123!");

            return RedirectToAction("Index");
        }

        public IActionResult UnselectSite(string company)
        {
            if (tenantSettings.Companies.ContainsKey(company))
                Response.Cookies.Delete("tenant-code");
            return RedirectToAction("Index");
        }
    }
}
