using SaaS.Domain.PIPL;

namespace SaaS.DataAccess.Repository.PIPL.IRepository
{
    public interface IFunctionnalityRepository : ISuperCompanyRepository<Functionnality>
    {
        public void Update(Functionnality functionnality);
    }
}
