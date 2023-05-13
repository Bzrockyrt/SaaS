using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SaaS.DataAccess.Data;

namespace SaaS.DataAccess
{
    public class TenantMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IConfiguration configuration;

        public TenantMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            this.next = next;
            this.configuration = configuration;
        }

        public async Task Invoke(HttpContext context)
        {
            //Récupère le nom du client à partir de l'URL
            string clientName = context.Request.Host.Value.Split('.')[0];

            //Récupère la chaîne de connexion pour ce client à partir de la configuration
            string connectionString = configuration.GetConnectionString(clientName);

            //Changer la chaîne de connexion pour la base de données
            var dbContext = context.RequestServices.GetService(typeof(ApplicationDbContext)) as ApplicationDbContext;
            dbContext.Database.SetConnectionString(connectionString);

            //Appeler le middleware suivant dans la pipeline
            await next(context);
        }
    }
}
