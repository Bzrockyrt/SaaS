using SaaS.DataAccess.Data;
using SaaS.DataAccess.Repository.Company.IRepository;
using SaaS.Domain.Company;

namespace SaaS.DataAccess.Repository.Company
{
    public class CompanyFunctionnalitiesRepository : ApplicationRepository<CompanyFunctionnalities>, ICompanyFunctionnalitiesRepository
    {
        private readonly ApplicationDbContext context;

        public CompanyFunctionnalitiesRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Update(CompanyFunctionnalities companyFunctionnalities)
        {
            context.Update(companyFunctionnalities);
        }
    }
}
