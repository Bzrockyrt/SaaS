using SaaS.DataAccess.Data;
using SaaS.DataAccess.Repository.Identity.IRepository;
using SaaS.Domain.Identity;

namespace SaaS.DataAccess.Repository.Identity
{
    public class EmploymentContractRepository : ApplicationRepository<EmploymentContract>, IEmploymentContractRepository
    {
        private readonly ApplicationDbContext context;

        public EmploymentContractRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Update(EmploymentContract employmentContract)
        {
            context.Update(employmentContract);
        }
    }
}
