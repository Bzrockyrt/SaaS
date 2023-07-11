using SaaS.DataAccess.Data;
using SaaS.DataAccess.Repository.PIPL.IRepository;
using SaaS.Domain.PIPL;

namespace SaaS.DataAccess.Repository.PIPL
{
    public class FunctionnalityRepository : SuperCompanyRepository<Functionnality>, IFunctionnalityRepository
    {
        private readonly PIPLDeveloppementDbContext context;

        public FunctionnalityRepository(PIPLDeveloppementDbContext context) : base(context)
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
