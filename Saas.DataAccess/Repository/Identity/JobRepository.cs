using SaaS.DataAccess.Data;
using SaaS.DataAccess.Repository.Identity.IRepository;
using SaaS.Domain.Identity;

namespace SaaS.DataAccess.Repository.Identity
{
    public class JobRepository : ApplicationRepository<Job>, IJobRepository
    {
        private readonly ApplicationDbContext context;

        public JobRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Update(Job job)
        {
            context.Update(job);
        }
    }
}
