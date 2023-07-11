using SaaS.DataAccess.Repository.IRepository;
using SaaS.Domain.Identity;

namespace SaaS.DataAccess.Repository.Identity.IRepository
{
    public interface ISubsidiaryRepository : IApplicationRepository<Subsidiary>
    {
        public void Update(Subsidiary subsidiary);
    }
}
