using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using SaaS.DataAccess.Utils;

namespace SaaS.DataAccess.Services
{
    public class TenantService
    {
        private TenantSettings tenantSettings;
        private HttpContext httpContext;
        private TenantData tenant;
        private string tenantCode;

        public TenantService(IOptions<TenantSettings> options, IHttpContextAccessor contextAccessor)
        {
            tenantSettings = options.Value;
            httpContext = contextAccessor.HttpContext;

            //If 'company' doesn't correspond to any of the tenant name in the file appsettings, there will be an error...
            if (httpContext.Request.Cookies.TryGetValue("tenant-code", out string site) && tenantSettings.Companies.ContainsKey(site))
            {
                tenantCode = site;
                tenant = tenantSettings.Companies[site];
            }
        }

        public string GetConnectionString()
        {
            return tenant?.connectionString;
        }

        public string GetTenantCode()
        {
            return tenantCode;
        }

        public TenantData GetTenant()
        {
            return tenant;
        }
    }
}
