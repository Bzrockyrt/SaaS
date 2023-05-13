using SaaS.Domain.Models;

namespace SaaS.DataAccess.Repository.IRepository
{
    public interface IWorkSiteRepository : IRepository<WorkSite>
    {
        void Update(WorkSite worksite);

        void Save();
    }
}
