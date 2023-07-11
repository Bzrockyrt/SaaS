using SaaS.DataAccess.Repository.IRepository;
using SaaS.Domain.Identity;

namespace SaaS.DataAccess.Repository.Identity.IRepository
{
    public interface IEmploymentContractRepository : IApplicationRepository<EmploymentContract>
    {
        public void Update(EmploymentContract employmentContract);
    }
}
