using SaaS.DataAccess.Data;
using SaaS.DataAccess.Repository.PIPL.IRepository;
using SaaS.Domain.PIPL;

namespace SaaS.DataAccess.Repository.PIPL
{
    public class SuperUserRepository : SuperCompanyRepository<User>, ISuperUserRepository
    {
        private readonly PIPLDeveloppementDbContext context;

        public SuperUserRepository(PIPLDeveloppementDbContext context) : base(context)
        {
            this.context = context;
        }

        public void Save()
        {
            this.context.SaveChanges();
        }

        public void Update(User superUser)
        {
            this.context.Update(superUser);
        }
    }
}
