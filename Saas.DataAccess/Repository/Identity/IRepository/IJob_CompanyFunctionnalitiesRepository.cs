using SaaS.DataAccess.Repository.IRepository;
using SaaS.Domain.Identity;

namespace SaaS.DataAccess.Repository.Identity.IRepository
{
    public interface IJob_CompanyFunctionnalitiesRepository : IApplicationRepository<Job_CompanyFunctionnalities>
    {
        public void Update(Job_CompanyFunctionnalities jobCompanyFunctionnalities);
    }
}
