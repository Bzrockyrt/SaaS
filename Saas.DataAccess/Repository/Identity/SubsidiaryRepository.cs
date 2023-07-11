using SaaS.DataAccess.Data;
using SaaS.DataAccess.Repository.Identity.IRepository;
using SaaS.Domain.Identity;

namespace SaaS.DataAccess.Repository.Identity
{
    public class SubsidiaryRepository : ApplicationRepository<Subsidiary>, ISubsidiaryRepository
    {
        private readonly ApplicationDbContext context;

        public SubsidiaryRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Update(Subsidiary subsidiary)
        {
            context.Update(subsidiary);
        }
    }
}
