using Microsoft.AspNetCore.Mvc;
using SaaS.DataAccess.Services;
using SaaS.Domain.Tenant;

namespace SaaS.Areas.SuperCompany.Controllers
{
    [Area("SuperCompany")]
    public class DashboardController : Controller
    {
        /*Contenu du dashboard : 
            - Une carte pour résumer le nombre d'entreprises actives
            - Une carte pour résumer les revenus totaux de l'application
            - Une carte pour résumer le nombre de problèmes rencontrés lors de l'utilisation de l'application
            - Une carte pour résumer le nombre de comptes utilisateurs créés dans l'application
            - */

        private readonly TenantService tenantService;

        public DashboardController(TenantService tenantService)
        {
            this.tenantService = tenantService;
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
            return View();
        }
    }
}
