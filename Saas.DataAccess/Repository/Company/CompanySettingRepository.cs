using SaaS.DataAccess.Data;
using SaaS.DataAccess.Repository.Company.IRepository;
using SaaS.Domain.Company;

namespace SaaS.DataAccess.Repository.Company
{
    public class CompanySettingRepository : ApplicationRepository<CompanySetting>, ICompanySettingRepository
    {
        private readonly ApplicationDbContext context;

        public CompanySettingRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Update(CompanySetting companySetting)
        {
            context.Update(companySetting);
        }
    }
}
