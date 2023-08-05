using SaaS.DataAccess.Data;
using SaaS.DataAccess.Repository.Work.IRepository;
using SaaS.Domain.Work;

namespace SaaS.DataAccess.Repository.Work
{
    public class WorkHour_WorkSiteRepository : ApplicationRepository<WorkHour_WorkSite>, IWorkHour_WorkSiteRepository
    {
        private readonly ApplicationDbContext context;

        public WorkHour_WorkSiteRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Update(WorkHour_WorkSite workHour_WorkSite)
        {
            context.Update(workHour_WorkSite);
        }
    }
}
