using Microsoft.AspNetCore.Identity;
using SaaS.DataAccess.Data;

namespace SaaS.DataAccess.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ApplicationDbContext context;

        public DbInitializer(UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.context = context;
        }

        public void Initialize()
        {
            /*try
            {
                if (this.context.Database.GetPendingMigrations().Count() > 0)
                    this.context.Database.Migrate();
            }
            catch (Exception ex) { }*/

            //Create roles if they are not created
        }
    }
}
