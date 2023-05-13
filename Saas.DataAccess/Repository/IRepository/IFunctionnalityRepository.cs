using SaaS.Domain.Models;

namespace SaaS.DataAccess.Repository.IRepository
{
    public interface IFunctionnalityRepository : IRepository<Functionnality>
    {
        public void Update(Functionnality functionnality);
    }
}
