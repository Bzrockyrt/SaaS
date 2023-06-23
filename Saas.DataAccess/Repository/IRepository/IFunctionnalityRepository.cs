using SaaS.Domain.Models;
using SaaS.Domain.PIPL;

namespace SaaS.DataAccess.Repository.IRepository
{
    public interface IFunctionnalityRepository : IRepository<Functionnality>
    {
        public void Update(Functionnality functionnality);
    }
}
