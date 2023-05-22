using SaaS.DataAccess.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaS.DataAccess.Services
{
    public sealed class TenantProvider
    {
        private const string TenantIdHeaderName = "X-TenantId";

        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly TenantSettings tenantSettings;

        public TenantProvider(IHttpContextAccessor httpContextAccessor,
            IOptions<TenantSettings> tenantOptions)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.tenantSettings = tenantOptions.Value;
        }

        public string TenantId => this.httpContextAccessor.HttpContext.Request.Headers[TenantIdHeaderName];

        public string GetConnectionString()
        {
            return this.tenantSettings.Tenants.Single(t => t.Id == TenantId);
        }
    }
}
