using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SaaS.DataAccess.Utils;

namespace SaaS.DataAccess.Services
{
    public class TenantService
    {
        private TenantSettings tenantSettings;
        private HttpContext httpContext;
        private TenantData tenant;
        private string tenantCode;
        private readonly IWebHostEnvironment hostingEnvironment;

        public TenantService(IOptions<TenantSettings> options, IHttpContextAccessor contextAccessor,
                                IWebHostEnvironment hostingEnvironment)
        {
            this.tenantSettings = options.Value;
            this.httpContext = contextAccessor.HttpContext;
            this.hostingEnvironment = hostingEnvironment;

            //If 'company' doesn't correspond to any of the tenant name in the file appsettings, there will be an error...
            if(contextAccessor.HttpContext != null)
            {
                if (httpContext.Request.Cookies.TryGetValue("tenant-code", out string site) && tenantSettings.Companies.ContainsKey(site))
                {
                    tenantCode = site;
                    tenant = tenantSettings.Companies[site];
                }
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

        public void AddTenant(string entrepriseId, string name, string logo, string connectionString)
        {
            string filePath = Path.Combine(this.hostingEnvironment.ContentRootPath, "appsettings.json");

            JObject appSettings = LoadAppSettings(filePath);
            JObject companies = appSettings["Tenants"]?["Companies"]?.Value<JObject>();

            if (companies != null)
            {
                JObject newTenant = new JObject();
                newTenant["name"] = name;
                newTenant["logo"] = logo;
                newTenant["connectionString"] = connectionString;

                companies[entrepriseId] = newTenant;
            }
            else
            {
                companies = new JObject();
                JObject newTenant = new JObject();
                newTenant["name"] = name;
                newTenant["logo"] = logo;
                newTenant["connectionString"] = connectionString;

                companies[entrepriseId] = newTenant;

                JObject tenants = new JObject();
                tenants["Companies"] = companies;
                appSettings["Tenants"] = tenants;
            }

            SaveAppSettings(filePath, appSettings);
        }

        public void DeleteTenant(string tenant)
        {
            string filePath = Path.Combine(this.hostingEnvironment.ContentRootPath, "appsettings.json");

            JObject appSettings = LoadAppSettings(filePath);
            JObject companies = appSettings["Tenants"]?["Companies"]?.Value<JObject>();

            if (companies != null)
            {
                companies.Remove(tenant);
                SaveAppSettings(filePath, appSettings);
            }
        }
        private JObject LoadAppSettings(string filePath)
        {
            using (StreamReader file = File.OpenText(filePath))
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                return (JObject)JToken.ReadFrom(reader);
            }
        }

        // Méthode utilitaire pour sauvegarder le fichier appsettings.json
        private void SaveAppSettings(string filePath, JObject appSettings)
        {
            using (StreamWriter file = File.CreateText(filePath))
            using (JsonTextWriter writer = new JsonTextWriter(file))
            {
                writer.Formatting = Formatting.Indented;
                appSettings.WriteTo(writer);
            }
        }
    }
}
