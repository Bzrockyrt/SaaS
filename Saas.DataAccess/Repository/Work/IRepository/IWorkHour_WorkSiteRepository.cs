using SaaS.DataAccess.Repository.IRepository;
using SaaS.Domain.Work;

namespace SaaS.DataAccess.Repository.Work.IRepository
{
    public interface IWorkHour_WorkSiteRepository : IApplicationRepository<WorkHour_WorkSite>
    {
        public void Update(WorkHour_WorkSite workHour_WorkSite);
    }
}
