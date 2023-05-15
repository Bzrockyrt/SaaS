using SaaS.DataAccess.Data;
using SaaS.DataAccess.Repository.IRepository;
using SaaS.Domain.Models;

namespace SaaS.DataAccess.Repository
{
    public class WorkSiteRepository : Repository<WorkSite>, IWorkSiteRepository
    {
        private readonly ApplicationDbContext context;

        public WorkSiteRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }

        public void Save()
        {
            this.context.SaveChanges();
        }

        public void Update(WorkSite worksite)
        {
            this.context.Update(worksite);
        }
    }
}
