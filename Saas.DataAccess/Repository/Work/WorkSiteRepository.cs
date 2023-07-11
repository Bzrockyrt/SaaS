using SaaS.DataAccess.Data;
using SaaS.DataAccess.Repository.Work.IRepository;
using SaaS.Domain.Work;

namespace SaaS.DataAccess.Repository.Work
{
    public class WorkSiteRepository : ApplicationRepository<WorkSite>, IWorkSiteRepository
    {
        private readonly ApplicationDbContext context;

        public WorkSiteRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Update(WorkSite workSite)
        {
            context.Update(workSite);
        }
    }
}
