using SaaS.DataAccess.Repository.IRepository;
using SaaS.Domain.Work;

namespace SaaS.DataAccess.Repository.Work.IRepository
{
    public interface IWorkSiteRepository : IApplicationRepository<WorkSite>
    {
        public void Update(WorkSite workSite);
    }
}
