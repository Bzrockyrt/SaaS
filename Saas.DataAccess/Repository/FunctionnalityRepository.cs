using SaaS.DataAccess.Data;
using SaaS.DataAccess.Repository.IRepository;
using SaaS.Domain.Models;
using SaaS.Domain.PIPL;

namespace SaaS.DataAccess.Repository
{
    public class FunctionnalityRepository : Repository<Functionnality>, IFunctionnalityRepository
    {
        private readonly ApplicationDbContext context;

        public FunctionnalityRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }

        public void Save()
        {
            this.context.SaveChanges();
        }

        public void Update(Functionnality functionnality)
        {
            this.context.Update(functionnality);
        }
    }
}
