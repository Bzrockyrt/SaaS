using SaaS.DataAccess.Data;
using SaaS.DataAccess.Repository.Identity.IRepository;
using SaaS.Domain.Identity;

namespace SaaS.DataAccess.Repository.Identity
{
    public class Job_CompanyFunctionnalititesRepository : ApplicationRepository<Job_CompanyFunctionnalities>, IJob_CompanyFunctionnalitiesRepository
    {
        private readonly ApplicationDbContext context;

        public Job_CompanyFunctionnalititesRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Update(Job_CompanyFunctionnalities jobCompanyFunctionnalities)
        {
            context.Update(jobCompanyFunctionnalities);
        }
    }
}
