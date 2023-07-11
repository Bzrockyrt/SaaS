using Microsoft.AspNetCore.Identity;
using SaaS.DataAccess.Data;

namespace SaaS.DataAccess.Repository
{
    /*public class ApplicationRoleRepository : Repository<IdentityRole>, IApplicationRoleRepository
    {
        private readonly ApplicationDbContext context;

        public ApplicationRoleRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }

        public void Save()
        {
            this.context.SaveChanges();
        }

        public void Update(IdentityRole applicationRole)
        {
            this.context.Update(applicationRole);
        }
    }*/
}
