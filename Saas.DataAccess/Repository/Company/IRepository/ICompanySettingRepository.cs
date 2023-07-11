using SaaS.DataAccess.Repository.IRepository;
using SaaS.Domain.Company;

namespace SaaS.DataAccess.Repository.Company.IRepository
{
    public interface ICompanySettingRepository : IApplicationRepository<CompanySetting>
    {
        public void Update(CompanySetting companySetting);
    }
}
