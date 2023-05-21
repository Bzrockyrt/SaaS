using SaaS.DataAccess.Data;
using SaaS.DataAccess.Repository.IRepository;
using SaaS.Domain.Models.Account;

namespace SaaS.DataAccess.Repository
{
    public class AuthorizationRepository : Repository<Authorization>
    {
        private readonly ApplicationDbContext context;

        public AuthorizationRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }

        public void Save()
        {
            this.context.SaveChanges();
        }

        public void Update(Authorization authorization)
        {
            this.context.Update(authorization);
        }
    }
}
