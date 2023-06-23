using Microsoft.AspNetCore.Identity;
using SaaS.DataAccess.Data;
using SaaS.DataAccess.Repository.IRepository;
using SaaS.Domain.Models;

namespace SaaS.DataAccess.Repository
{
    public class ApplicationRoleRepository : Repository<IdentityRole>, IApplicationRoleRepository
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
    }
}
